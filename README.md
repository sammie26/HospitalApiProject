# Hospital Management System API


**Submitted by:** Sama Ahmed 211005402

---


## Technologies Used

Technology Description:                                                          

ASP.NET Core 9.0  
High-performance, cross-platform framework for building RESTful APIs 
Entity Framework Core 
ORM for database access using code-first approach                    
SQL Server            
Relational database for persistent data storage                      
JWT (JSON Web Tokens) 
Stateless authentication mechanism for securing API endpoints       
Swagger (OpenAPI)     
API documentation and testing interface  
Hangfire - A library for background job processing, used in this project to automate database maintenance and appointment cleanup.                          

---

## Authentication & Security

The API uses **JWT Bearer Token Authentication** to secure endpoints.

### How it works:

* Users authenticate via `/api/Auth/login`
* A JWT token is generated and returned
* The token must be included in requests using:

```
Authorization: Bearer {token}
```

### Security Features:

* Token validation (issuer, audience, signing key)
* Protected endpoints using `[Authorize]`
* Role-based access control (e.g., Admin-only operations)

---

## API Endpoints

### Authentication

* `POST /api/Auth/register`
  Registers a new user

* `POST /api/Auth/login`
  Authenticates user and returns JWT token

---

### Doctors Management

* `GET /api/Doctors`
  Retrieve all doctors

* `GET /api/Doctors/{id}`
  Retrieve a specific doctor

* `POST /api/Doctors`
  Add a new doctor *(Admin only)*

* `PUT /api/Doctors/{id}`
  Update doctor information

* `DELETE /api/Doctors/{id}`
  Delete doctor *(Returns 204 No Content)*

---

### Patients Management

* `GET /api/Patients`
  Retrieve all patients

* `GET /api/Patients/{id}`
  Retrieve a specific patient

* `POST /api/Patients`
  Create a new patient

* `PUT /api/Patients/{id}`
  Update patient information

* `DELETE /api/Patients/{id}`
  Delete a patient

---

### Appointments Management

* `GET /api/Appointments`
  Retrieve all appointments

* `POST /api/Appointments`
  Book a new appointment *(Requires valid DoctorId and PatientId)*

* `DELETE /api/Appointments/{id}`
  Cancel an appointment

---

## Project Architecture

The project follows a clean layered architecture:

* **Controllers** → Handle HTTP requests
* **Services** → Business logic
* **Interfaces** → Abstractions for services
* **Models** → Database entities
* **DTOs** → Data transfer objects

---

## Setup and Installation

### 1. Configure Database

Update your connection string in:


appsettings.json


---

### 2. Apply Migrations

Run:


dotnet ef database update


---

### 3. Run the Application

dotnet run


---

### 4. Access Swagger UI


https://localhost:<port>/swagger


Use Swagger to:

* Explore endpoints
* Test API requests
* Authorize using JWT

---

## Testing Authentication in Swagger

1. Login via `/api/Auth/login`
2. Copy the returned token
3. Click **Authorize** in Swagger
4. Enter:

```
your_token_here  << I don't add Bearer because it's already in my code>>
```

---

## Key Features

* RESTful API design
* JWT authentication
* Role-based authorization
* Clean architecture (Controller-Service pattern)
* Entity Framework Core integration
* Swagger documentation

---

## Future Improvements

* Implement refresh tokens
* Add role management system
* Integrate background jobs (e.g., Hangfire)
* Add frontend client (Angular / React)

---


