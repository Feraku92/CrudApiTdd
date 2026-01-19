# CrudApiTdd - Pokémon Management System

A full-stack CRUD application built with .NET Core and Angular, following Test-Driven Development (TDD) principles. This application allows users to manage a Pokémon collection with complete CRUD operations, user authentication, and JWT-based security.

## 📋 Table of Contents
- [User Story](#user-story)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [Demo Credentials](#demo-credentials)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)

## 👤 User Story

**As a** Pokémon trainer and collector  
**I want to** manage my Pokémon collection through a secure web application  
**So that I** can keep track of all the Pokémon I've captured, view their details, and organize them efficiently.

### Acceptance Criteria:
- ✅ I can register as a new user with my email and password
- ✅ I can log in securely and receive a token for my session
- ✅ I can view all Pokémon in my collection in a clean interface
- ✅ I can search for specific Pokémon by name or Pokédex number
- ✅ I can add new Pokémon to my collection with their details (Pokédex ID, name, and type)
- ✅ I can edit existing Pokémon information if I made a mistake
- ✅ I can delete Pokémon from my collection
- ✅ The system prevents me from adding duplicate Pokédex IDs
- ✅ All my data is secure and requires authentication to access

## ✨ Features

- **User Authentication**: Secure registration and login with JWT tokens
- **CRUD Operations**: Complete Create, Read, Update, Delete functionality for Pokémon
- **Search Functionality**: Find Pokémon by name or Pokédex number
- **Data Validation**: Input validation on both frontend and backend
- **MongoDB Integration**: NoSQL database for flexible data storage
- **Test Coverage**: Comprehensive unit, integration, and API tests
- **CORS Support**: Configured for Angular frontend integration
- **RESTful API**: Well-structured API endpoints following REST principles

## 🛠 Technology Stack

### Backend
- **.NET 8.0**: Latest LTS version of .NET
- **ASP.NET Core Web API**: RESTful API framework
- **MongoDB**: NoSQL database
- **JWT Authentication**: Secure token-based authentication
- **xUnit**: Testing framework
- **Moq**: Mocking framework for unit tests

### Frontend
- **Angular 21**: Modern TypeScript-based framework
- **RxJS**: Reactive programming for HTTP operations
- **Crypto-js**: Client-side encryption utilities
- **Vitest**: Fast unit testing framework

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

1. **.NET 8.0 SDK or later**
   - Download from: https://dotnet.microsoft.com/download
   - Verify installation: `dotnet --version`

2. **MongoDB**
   - Download from: https://www.mongodb.com/try/download/community
   - Verify installation: MongoDB should be running on `mongodb://localhost:27017`
   - Alternative: Use MongoDB Compass for GUI management

3. **Node.js and npm**
   - Download from: https://nodejs.org/ (LTS version recommended)
   - Verify installation: `node --version` and `npm --version`

4. **Angular CLI** (optional but recommended)
   - Install globally: `npm install -g @angular/cli`
   - Verify installation: `ng version`

## 🚀 Setup Instructions

### Backend Setup

1. **Navigate to the backend directory:**
   ```powershell
   cd Backend\CrudApi\CrudApi.Api
   ```

2. **Restore NuGet packages:**
   ```powershell
   dotnet restore
   ```

3. **Ensure MongoDB is running:**
   - Start MongoDB service (usually runs automatically after installation)
   - Or run: `mongod` in a separate terminal
   - Verify connection at: `mongodb://localhost:27017`

4. **Build the project:**
   ```powershell
   dotnet build
   ```

5. **Run the API:**
   ```powershell
   dotnet run
   ```
   
   The API will start at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

### Frontend Setup

1. **Navigate to the frontend directory:**
   ```powershell
   cd Frontend\crudapi-frontend
   ```

2. **Install dependencies:**
   ```powershell
   npm install
   ```

3. **Start the development server:**
   ```powershell
   npm start
   ```
   
   The application will open at: `http://localhost:4200`

### Database Seeding (Optional)

The application will work with an empty database. To create your first user:

1. Navigate to `http://localhost:4200`
2. Click on "Register" 
3. Use the demo credentials provided below or create your own

## 🔑 Demo Credentials

For testing purposes, you can use these credentials:

### Demo User Account
- **Username**: `trainer_ash`
- **Email**: `ash@pokemon.com`
- **Password**: `Pikachu123!`

### Creating Demo Data
After logging in, you can add sample Pokémon:

1. **Pikachu**
   - Pokédex ID: 25
   - Name: Pikachu
   - Type: Electric

2. **Charizard**
   - Pokédex ID: 6
   - Name: Charizard
   - Type: Fire/Flying

3. **Blastoise**
   - Pokédex ID: 9
   - Name: Blastoise
   - Type: Water

## 🧪 Running Tests

### Backend Tests

Run all tests:
```powershell
cd Backend\CrudApi
dotnet test
```

Run specific test projects:
```powershell
# Unit Tests
dotnet test CrudApi.Api.Tests
dotnet test CrudApi.Application.Tests
dotnet test CrudApi.Domain.Tests

# Integration Tests
dotnet test CrudApi.IntegrationTests
```

Run tests with coverage:
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend Tests

```powershell
cd Frontend\crudapi-frontend
npm test
```

## 📚 API Documentation

Once the backend is running, access the interactive API documentation:
- **Swagger UI**: `https://localhost:5001/swagger`

### Main Endpoints

#### Authentication
- `POST /api/User/register` - Register a new user
- `POST /api/User/login` - Login and receive JWT token

#### Pokémon Management (Requires Authentication)
- `GET /api/Pokemon/getall` - Get all Pokémon
- `GET /api/Pokemon/search?name={name}&number={number}` - Search Pokémon
- `POST /api/Pokemon/create` - Create a new Pokémon
- `PUT /api/Pokemon/{id}` - Update a Pokémon
- `DELETE /api/Pokemon/{id}` - Delete a Pokémon

### Example API Requests

**Register User:**
```json
POST /api/User/register
{
  "userName": "trainer_ash",
  "email": "ash@pokemon.com",
  "password": "Pikachu123!"
}
```

**Login:**
```json
POST /api/User/login
{
  "userName": "trainer_ash",
  "password": "Pikachu123!"
}
```

**Create Pokémon (with Bearer token):**
```json
POST /api/Pokemon/create
Authorization: Bearer {your-jwt-token}
{
  "pokedexId": 25,
  "name": "Pikachu",
  "type": "Electric"
}
```

## 🏗 Project Structure

```
CrudApiTdd/
├── Backend/
│   └── CrudApi/
│       ├── CrudApi.Api/              # Web API layer
│       ├── CrudApi.Application/      # Business logic
│       ├── CrudApi.Domain/           # Domain entities
│       ├── CrudApi.Infrastructure/   # Data access
│       ├── CrudApi.Api.Tests/        # API unit tests
│       ├── CrudApi.Application.Tests/# Application tests
│       ├── CrudApi.Domain.Tests/     # Domain tests
│       └── CrudApi.IntegrationTests/ # Integration tests
└── Frontend/
    └── crudapi-frontend/            # Angular application
        ├── src/app/auth/            # Authentication module
        └── src/app/records/         # Pokémon records module
```

## 🔧 Configuration

### Backend Configuration
Edit `Backend\CrudApi\CrudApi.Api\appsettings.json`:
- **MongoDB Connection**: Change `ConnectionStrings:MongoDb` if using a different MongoDB instance
- **JWT Secret**: Configured for development (change in production)
- **CORS**: Configured for `http://localhost:4200`

### Frontend Configuration
Edit `Frontend\crudapi-frontend\src\environments\environment.ts`:
- API URL configuration for different environments

## 🐛 Troubleshooting

### MongoDB Connection Issues
- Ensure MongoDB service is running: `mongod`
- Check if port 27017 is available
- Verify connection string in `appsettings.json`

### CORS Errors
- Ensure the frontend is running on port 4200
- Check CORS configuration in `Program.cs`

### JWT Token Issues
- Tokens expire after a set time (check JWT configuration)
- Re-login to get a new token

## 📝 Notes

- This project follows Clean Architecture principles with clear separation of concerns
- All major features are covered by unit and integration tests
- The application uses TDD methodology - tests were written before implementation
- MongoDB was chosen for its flexibility and ease of setup
- JWT authentication provides stateless, scalable security

**Built with ❤️ using .NET, Angular, and MongoDB**