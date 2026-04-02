using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HospitalApiProject.Models;
using HospitalApiProject.Data; 
using Microsoft.EntityFrameworkCore;

namespace HospitalApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // 1. REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                return Ok(new { message = "User registered successfully as Admin!" });
            }
            return BadRequest(result.Errors);
        }

        // 2. LOGIN (Now returns Refresh Token)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Generate Access Token
                var jwtToken = await GenerateJwtToken(user);
                
                // Generate Refresh Token string
                var refreshTokenString = Guid.NewGuid().ToString();

                // Save Refresh Token to DB
                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshTokenString,
                    UserId = user.Id,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                    IsUsed = false,
                    IsRevoked = false
                };

                await _context.RefreshTokens.AddAsync(refreshTokenEntity);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    refreshToken = refreshTokenString,
                    expiration = jwtToken.ValidTo
                });
            }
            return Unauthorized();
        }

        // 3. REFRESH (The Bonus Logic)
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestDto tokenRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }

            // Mark current token as used (security best practice: one-time use)
            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            
            // Fix: Null check to satisfy CS8604
            if (user == null)
            {
                return Unauthorized(new { message = "User no longer exists." });
            }

            var newJwtToken = await GenerateJwtToken(user);
            var newRefreshTokenString = Guid.NewGuid().ToString();

            await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = newRefreshTokenString,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newJwtToken),
                refreshToken = newRefreshTokenString,
                expiration = newJwtToken.ValidTo
            });
        }

        // 4. HELPER: JWT GENERATION
        private async Task<JwtSecurityToken> GenerateJwtToken(IdentityUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(15), // Access tokens should be short-lived
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }

    // --- DTOs ---
    public class RegisterDto 
    { 
        [Required][EmailAddress] public string Email { get; set; } = ""; 
        [Required][MinLength(6)] public string Password { get; set; } = ""; 
    }

    public class LoginDto 
    { 
        [Required][EmailAddress] public string Email { get; set; } = ""; 
        [Required] public string Password { get; set; } = ""; 
    }

    public class TokenRequestDto 
    {
        [Required] public string AccessToken { get; set; } = "";
        [Required] public string RefreshToken { get; set; } = "";
    }
}