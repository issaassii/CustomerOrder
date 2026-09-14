# CustomerOrder API

A layered ASP.NET Core (.NET 8) Web API for managing customers, products, and orders, built as part of a backend internship program. The project follows a clean, layered architecture (API → Business → Persistence → Domain) and was developed incrementally across four weeks, mirroring professional development practices: feature branches, Pull Requests, structured logging, authentication, validation, transactions, and automated testing.

## Tech Stack

- **.NET 8 / ASP.NET Core** — Web API
- **Entity Framework Core** — primary ORM
- **Dapper** — used for two specific reporting stored procedures
- **PostgreSQL** — database
- **Serilog** — structured logging (console + rolling file)
- **JWT Authentication** — token-based auth with role-based authorization
- **FluentValidation** — request validation
- **Swagger / OpenAPI** — interactive API documentation
- **Asp.Versioning** — API versioning
- **xUnit + Moq** — unit testing for the Business layer

## Architecture

```
CustomerOrder.API            → Controllers, Program.cs, Swagger, middleware
CustomerOrder.Business        → Services, business logic
CustomerOrder.Persistence     → EF Core, DbContext, repositories, Unit of Work, Dapper reports
CustomerOrder.Domain           → Entities, DTOs, interfaces (no external dependencies)
CustomerOrder.Business.Tests  → Unit tests for the Business layer (xUnit + Moq)
```

Dependencies flow inward — API depends on Business and Persistence, Business depends on Persistence and Domain, Persistence depends on Domain, and Domain depends on nothing.

```
CustomerOrder/
├── CustomerOrder.slnx
├── src/
│   ├── CustomerOrder.API/
│   ├── CustomerOrder.Business/
│   ├── CustomerOrder.Persistence/
│   └── CustomerOrder.Domain/
├── tests/
│   └── CustomerOrder.Business.Tests/
├── scripts/
│   └── stored_procedures.sql
└── postman/
    └── CustomerOrder API.postman_collection.json
```

## Features

### Customers, Products & Orders
- Full CRUD for Customers and Products, with soft delete (`IsDeleted`) and audit fields
- Orders created with their OrderItems inside a single database transaction — either the whole order saves, or none of it does
- One-to-many relationships (Customer → Orders → OrderItems ← Product) via Data Annotations and EF Core navigation properties
- `Include()` used for loading related data; `AsNoTracking()` used on read-only queries

### Security
- JWT-based login (`POST /api/v1/Auth/login`), with hashed passwords (`PasswordHasher`)
- `[Authorize]` on all Customer/Product/Order endpoints; a custom `AdminOnly` policy restricts sensitive actions (e.g. deleting a customer) to Admin-role users
- Rate limiting on the login endpoint (10 requests/minute) to reduce brute-force risk
- Secrets (connection string, JWT signing key, seed password) are kept out of source control via .NET User Secrets

### Data Access
- EF Core is the default ORM for all CRUD operations
- Dapper is used specifically for two PostgreSQL functions: `GetCustomerOrderSummary` and `SearchOrders` — direct SQL where it's a better fit than LINQ

### Quality & Observability
- FluentValidation for Customer/Product create and update requests
- Serilog structured logging to console and a daily rolling file
- `/health` endpoint with live database connectivity check
- Unit tests for the Business layer (`CustomerService`, `AuthService`), covering both success and failure paths, with `IUnitOfWork` and its repositories mocked via Moq

## Local Setup

1. **Prerequisites:** .NET 8 SDK, PostgreSQL running locally.

2. Clone the repo and restore dependencies:
   ```
   dotnet restore
   ```

3. Configure secrets (never committed to source control):
   ```
   cd src/CustomerOrder.API
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=customerorderdb;Username=postgres;Password=<your-password>"
   dotnet user-secrets set "Jwt:Key" "<a-long-random-secret-32-chars-minimum>"
   dotnet user-secrets set "Seed:AdminPassword" "<admin-password>"
   cd ../..
   ```

4. Apply EF Core migrations:
   ```
   dotnet ef database update --project src/CustomerOrder.Persistence --startup-project src/CustomerOrder.API
   ```

5. Create the PostgreSQL functions used by Dapper:
   Run `scripts/stored_procedures.sql` against your database (via pgAdmin's Query Tool, or `psql -f scripts/stored_procedures.sql`).

6. Run the API:
   ```
   dotnet run --project src/CustomerOrder.API
   ```

7. Open Swagger at `https://localhost:<port>/swagger`. On first run, an admin user is seeded automatically (username `admin`, password from `Seed:AdminPassword`).

## Running Tests

```
dotnet test
```

Runs the Business layer unit tests. Each test mocks `IUnitOfWork` and its repositories via Moq — no real database is touched.

## API Overview

| Area | Endpoints |
|---|---|
| Auth | `POST /api/v1/Auth/login` |
| Customers | `GET/POST /api/v1/Customers`, `GET/PUT/DELETE /api/v1/Customers/{id}` |
| Products | `GET/POST /api/v1/Products`, `GET/PUT/DELETE /api/v1/Products/{id}` |
| Orders | `POST /api/v1/Orders`, `GET /api/v1/Orders/{id}`, `GET /api/v1/Orders/summary/{customerId}`, `GET /api/v1/Orders/search` |
| Health | `GET /health` |

A full Postman collection covering all of the above, including auth token handling, is in `postman/`.

## Git Workflow

Work happens on feature branches, merged into `main` via Pull Request. Notable branches through the project's history:
- `feature/setup-project` — initial scaffolding, Customer CRUD, Swagger, Serilog
- `feature/users` — JWT auth, login, database seeding, secret handling
- `feature/authorization` — `[Authorize]`, `AdminOnly` policy, FluentValidation, health checks, rate limiting
- `feature/orders` — Product/Order/OrderItem entities, relationships, transactions, Dapper reporting
- `feature/testing` — Business layer unit tests

## Notes on Design Decisions

- **Soft delete everywhere** (`IsDeleted`) instead of physical deletes, to preserve history and avoid irreversible data loss.
- **`OrderItem.UnitPrice` is copied at order-creation time**, not read live from `Product.Price` — an order's total should never silently change because a product's price changed later.
- **EF Core stays the default** for all CRUD; Dapper is reserved specifically for the two reporting functions where direct SQL is a better fit than LINQ.
- **Controllers stay thin** — every action calls exactly one Business-layer method and translates the result into an HTTP status code. No business logic or direct data access lives in a controller.
