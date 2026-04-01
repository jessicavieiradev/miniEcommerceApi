<<<<<<< HEAD
# miniEcommerceApi

## Description
o q faz
com o q foi construido
pq foi construido

## Instalation instructions

### pre requisitos

...

### etapas

blocos de codigo ```bach
npm intall
```
```

## How to use

## Licence
=======
# MiniEcommerce API

REST API developed as a portfolio project to demonstrate backend development skills with ASP.NET Core 8.

The project simulates the backend of an online store — covering user registration and authentication, product catalog, address management, order creation, and payment processing with stock control.

---

## Technical Decisions

**Layered architecture**
The project is organized into Controllers, Services, and Repositories, separating responsibilities and making the codebase easier to maintain and test. Services have no knowledge of infrastructure details — they depend only on interfaces, which allows swapping implementations without affecting the rest of the system.

**Authentication and authorization**
Authentication is implemented with ASP.NET Core Identity, generating JWT tokens with role claims (customer and administrator). Access control is enforced via `[Authorize(Roles = "...")]` on endpoints, ensuring that administrative operations — such as managing stock — are not exposed to regular users.

**Unit tests**
Tests implemented for `AuthService` and `PaymentService` using xUnit, Moq, and FluentAssertions. The database is replaced by an EF Core InMemory provider in tests, isolating business logic from infrastructure. The decision to test only these two services was intentional — they cover the highest-risk flows in the application.

**Docker**
The application and database run in containers orchestrated via Docker Compose. Migrations and database creation are executed automatically on startup, eliminating any manual infrastructure setup.

**External integration**
Products and categories are imported from the [DummyJSON API](https://dummyjson.com/) on startup, simulating a real-world integration with an external supplier.

---

## Stack

| | |
|---|---|
| ASP.NET Core 8 | Main framework |
| Entity Framework Core + SQL Server | Data persistence |
| ASP.NET Core Identity + JWT | Authentication and authorization |
| Docker + Docker Compose | Containerization |
| xUnit + Moq + FluentAssertions | Unit tests |

---

## Getting Started

### Prerequisites
- Docker

### 1. Clone the repository

```bash
git clone https://github.com/jessicavieiradev/miniEcommerceApi
cd miniEcommerceApi
```

### 2. Create the `.env` file

In the project root:

```env
BD_PASSWORD=        # SQL Server password
JWT_SECRET=         # secret key used to sign JWT tokens
ADMIN_EMAIL=        # email of the admin user created on first run
ADMIN_USERNAME=     # admin username
ADMIN_PASSWORD=     # admin password
```

### 3. Configure application secrets

Inside the `miniEcommerceApi` folder:

```bash
dotnet user-secrets init
```

```json
{
  "Jwt": {
    "SecretKey": "<same value as JWT_SECRET>"
  },
  "AdminSeed": [
    {
      "Email": "<same value as ADMIN_EMAIL>",
      "UserName": "<same value as ADMIN_USERNAME>",
      "Password": "<same value as ADMIN_PASSWORD>"
    }
  ],
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=miniecommercebd;User Id=sa;Password=<same value as BD_PASSWORD>;TrustServerCertificate=True;"
  }
}
```

### 4. Start the containers

```bash
docker compose up --build
```

The database is created, migrations are applied, and the admin user is seeded automatically.

### 5. Access the documentation

```
http://localhost:5000/swagger
```
>>>>>>> 484e4d8bba9cabd5ecd247fdc831dd9d7a1ee3ef
