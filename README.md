# Store App API

This is a .NET Core Web API project for a store application that manages products, categories, and user authentication.

## Features

- Product Management (CRUD operations)
- Category Management
- User Authentication with JWT
- Entity Framework Core with SQLite
- Data Transfer Objects (DTOs)
- RESTful API Design

## Project Structure

```plaintext
├── Controllers/
│   ├── AuthController.cs       # Authentication endpoints
│   ├── CategoriesController.cs # Category management
│   └── ProductsController.cs   # Product management
├── Data/
│   └── DbContext.cs           # Database context
├── Dtos/                      # Data Transfer Objects
├── Models/                    # Entity models
├── Services/
│   └── TokenService.cs       # JWT token service
└── Migrations/               # Database migrations
```

## Prerequisites

- .NET 9.0
- SQLite

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## API Endpoints

### Authentication

#### Register

- POST /api/auth/register - Register a new user

```json
{
    "username": "string",
    "password": "string",
    "email": "string"
}
```

#### Login

- POST /api/auth/login - Login user

```json
{
    "username": "string",
    "password": "string"
}
```

### Products

- GET /api/products - Get all products
- GET /api/products/{id} - Get product by ID

- POST /api/products - Create new product

```json
{
    "name": "string",
    "price": 0,
    "description": "string",
    "categoryId": 0,
    "imageUrl": "string"
}
```

- PUT /api/products/{id} - Update product

```json
{
    "name": "string",
    "price": 0,
    "description": "string",
    "categoryId": 0,
    "imageUrl": "string"
}
```

- DELETE /api/products/{id} - Delete product

### Categories

- GET /api/categories - Get all categories
- GET /api/categories/{id} - Get category by ID

- POST /api/categories - Create new category

```json
{
    "name": "string",
    "description": "string"
}
```

- PUT /api/categories/{id} - Update category

```json
{
    "name": "string",
    "description": "string"
}
```

- DELETE /api/categories/{id} - Delete category

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core
- SQLite Database
- JWT Authentication
- AutoMapper (for DTO mappings)

## Security

- JWT token-based authentication
- Password hashing with salt
- Protected routes with authorization
