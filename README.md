# HR Management System

## Overview
This is an **ASP.NET MVC** application that manages employees and departments. The system provides authentication with **ASP.NET Identity**, allows users to log in using **Google external authentication**, and includes features like **image upload, SMS notifications via Twilio, and email notifications**.

## Features
- **User Authentication & Authorization**
  - ASP.NET Identity for managing users.
  - Google external login integration.
- **Employee & Department Management**
  - Create, read, update, and delete (CRUD) operations.
- **File Upload**
  - Upload employee profile pictures.
- **SMS Notifications**
  - Send SMS messages via Twilio.
- **Email Notifications**
  - Send emails for account verification, password resets, etc.

## Technologies Used
- **Backend**: ASP.NET MVC, Entity Framework Core, ASP.NET Identity
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity, Google OAuth
- **File Upload**: Local storage
- **Notifications**: Twilio API (SMS), SMTP (Email)
- **Frontend**: HTML, CSS, JavaScript

## Installation & Setup
### Prerequisites
- .NET SDK
- SQL Server
- Twilio Account (for SMS integration)
- SMTP Email Server
- Google OAuth Credentials

### Steps
1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd <project-folder>
   ```
2. Configure the database in `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;"
   }
   ```
3. Configure Google OAuth in `appsettings.json`:
   ```json
   "Authentication": {
       "Google": {
           "ClientId": "your_client_id",
           "ClientSecret": "your_client_secret"
       }
   }
   ```
4. Configure Twilio credentials:
   ```json
   "Twilio": {
       "AccountSid": "your_account_sid",
       "AuthToken": "your_auth_token",
       "FromPhone": "+1234567890"
   }
   ```
5. Configure email settings:
   ```json
   "EmailSettings": {
       "SmtpServer": "smtp.example.com",
       "Port": 587,
       "SenderEmail": "your-email@example.com",
       "SenderPassword": "your-email-password"
   }
   ```
6. Run migrations:
   ```sh
   dotnet ef database update
   ```
7. Run the application:
   ```sh
   dotnet run
   ```

## API Endpoints
### Authentication
- `POST /api/Auth/Register` – Register a new user
- `POST /api/Auth/Login` – Authenticate user
- `GET /api/Auth/ExternalLogin` – Google login

### Employees
- `GET /api/Employees` – Get all employees
- `GET /api/Employees/{id}` – Get employee by ID
- `POST /api/Employees` – Create a new employee
- `PUT /api/Employees/{id}` – Update an employee
- `DELETE /api/Employees/{id}` – Delete an employee

### Departments
- `GET /api/Departments` – Get all departments
- `GET /api/Departments/{id}` – Get department by ID
- `POST /api/Departments` – Create a new department
- `PUT /api/Departments/{id}` – Update a department
- `DELETE /api/Departments/{id}` – Delete a department

## License
This project is licensed under the MIT License.

