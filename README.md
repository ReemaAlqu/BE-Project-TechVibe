# Tec Vibe Backend Project 🚀

## Project Overview ✨

**CareQuest**  is a backend solution for an e-commerce platform specializing in smart devices such as smartphones, wearable devices, and smart home gadgets. Built using .NET 8, this project includes core functionalities like user authentication, product management, cart management, orders, and more.
## Features

**User Managment**

- Register new user
- User authentication with JWT token
- Role-based access control (Admin, Customer).
- Update user information.
- Delete user information.

**Product Management**:

- Create new product
- Delete product
- Update product
- Search Products: Provides functionality to search for products based on filters, such as name, and price range , this includes the ability to search products within a maximum and minimum price range.
- Role-based access control (Admin, Customer)

## Technologies Used

- **.Net 8**: Web API Framework
- **Entity Framework Core**: ORM for database interactions
- **PostgreSQl**: Relational database for storing data
- **JWT**: For user authentication and authorization
- **AutoMapper**: For object mapping
- **Swagger**: API documentation

## Prerequisites

- .NET 8 SDK
- SQL Server
- VSCode
- Postman or similar API testing tools

## Getting Started

### 1. Clone the repository:

```bash
git clone `https://github.com/shuruq25/sda-3-online-Backend_Teamwork.git`


```

### 2. Setup database

- Make sure PostgreSQL Server is running
- Create `appsettings.json` file
- Update the connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "Local": "Server=localhost;Database=your_DBname;User Id=your_username;Password=your_password;"
  }
}
```

- Run migrations to create database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Run the application

```bash
dotnet watch
```

The API will be available at: 'https://be-project-techvibe.onrender.com'

### Swagger

- Navigate to `http://localhost:5125/swagger/index.html` to explore the API endpoints.

## Project structure

```bash
|-- Controllers: #API controllers with request and response
|-- Database # DbContext and Database Configurations
|-- DTOs # Data Transfer Objects
|-- Entities # Database Entities (User, Product)
|-- Middleware # Logging request, response and Error Handler
|-- Repositories # Repository Layer for database operations
|-- Services # Business Logic Layer
|-- Utils # Common logics
|-- Migrations # Entity Framework Migrations
|-- Program.cs # Application Entry Point
```

## API Endpoints

### User

- **POST** `/api/v1/user/signUp` – Register a new user.
- **POST** `/api/v1/user/signIn` – Login and get JWT token.
- **GET** `/api/v1/user` – Get users. (Admin only)
- **GET** `/api/v1/user/{id}` – Get user details by ID. (Admin only)
- **DELETE** `/api/v1/user/{id}` – Delete a user by ID. (Authorized users only)
- **PUT** `/api/v1/user/{id}` – Update user details by ID. (Authorized users only)

### Product

- **POST** `/api/v1/product` - Creating a product. (Admin only)
- **GET** `/api/v1/product` - Viewing all products for both user/admin.
- **GET** `/api/v1/product/Id` - Viewing product to user/admin.
- **GET** `/api/v1/product/search` - Search product based on the name or categeory.
- **GET** `/api/v1/product/sorted-filtered` - filtered and sorted product based on price or name  .
- **PUT** `/api/v1/product/adminId` - Updating product. (Admin only)
- **DELETE** `/api/vi/Product/adminId` - Deleting product. (Admin only)

## Team Members💻✨

- **Leader** : Shuruq Almuhalbidi (@shuruq25)👩‍💻
- Abdullah Alkhwahir(@Abdullah-Khawahir) 👨‍💻
- Hadeel Alghashmari (@hdoll0)👩‍💻
- Raghad Alharbi(@Rad109)👩‍💻
- Reema Algureshie(ReemaAlqu)👩‍💻

## License

This project is licensed under the MIT License.
