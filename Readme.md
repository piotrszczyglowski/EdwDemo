# Document Access Management API

## Overview
This API provides a secure way to manage access requests for internal documents in an enterprise environment. It implements a workflow for requesting, approving, and tracking document access.

## Features
- User authentication and role-based authorization
- Document access request submission and tracking
- Approval workflow for document access
- Real-time notifications for request status changes
- Swagger API documentation

## Technical Stack
- ASP.NET Core 8.0
- Entity Framework Core with SQLite
- JWT Authentication
- Swagger/OpenAPI
- Background Service for notifications

## Project Structure
- `Edw.Domain`: Contains domain entities and interfaces
- `Edw.Infrastructure`: Implementation of data access and services
- `EdwDemo.Api`: REST API endpoints and DTOs
- `EdwDemo.Tests`: Unit tests

## Getting Started
- Application should run without additional configuration.
- Docker desktop is required for running the application in a containerized environment.
- For testing i have created 3 separate user accounts - without password: user@edw.com, approver@edw.com, admin@edw.com
- New user can be registered but then admin needs to assign role to the user.


### Prerequisites
- .NET 8.0 SDK
- SQLite

### Installation
1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet restore`
4. Run `dotnet ef database update`
5. Start the application:

      dotnet run --project EdwDemo.Api
   
### Authentication
The API uses JWT Bearer tokens for authentication. To obtain a token:
1. Make a POST request to `/api/auth/login`
2. Include the token in subsequent requests in the Authorization header:

      Authorization: Bearer <token>
   
## API Documentation
Access the Swagger UI at `/swagger` when running in development mode.

## Testing
Run the test suite:

dotnet test

## Future Improvements
- Email notification integration
- Document searching
- Paging
- Allow user share own documents
- Request analytics dashboard
- Access expiration management
- Change documentId to guid
- Full documentation
- Test for all endpoints and handlers
- Integration tests