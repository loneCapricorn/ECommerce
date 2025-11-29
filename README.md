## ECommerce API

Modern, minimal .NET API for a simple e-commerce backend: user authentication (JWT), role-based authorization, product & category management, order placement, and relational data modeling with Entity Framework Core.

---

## Tech Stack

- .NET 8 Web API (minimal hosting model)
- Entity Framework Core (SQL Server)
- JWT Authentication & Authorization (roles: Admin, User)
- Swagger/OpenAPI (dev environment)
- DotNetEnv for environment variable loading

---

## Project Structure

```
ECommerce/
├── ECommerce.sln
├── src/
│   └── ECommerceAPI/
│       ├── Program.cs
│       ├── appsettings.json
│       ├── .env.local (dev env vars - not committed in production)
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── ProductsController.cs
│       │   ├── CategoriesController.cs
│       │   ├── OrdersController.cs
│       │   └── UsersController.cs
│       ├── Data/
│       │   └── ECommerceDbContext.cs
│       ├── DTOs/
│       │   ├── AuthDTOs.cs
│       │   └── OrderDTOs.cs
│       ├── Helpers/
│       │   ├── DatabaseSeeder.cs (Seeds roles)
│       │   └── PasswordHasher.cs
│       ├── Migrations/ (EF Core migrations)
│       ├── Models/
│       │   ├── User.cs / Role.cs / UserRole.cs
│       │   ├── Product.cs / Category.cs / ProductCategory.cs
│       │   ├── Order.cs / OrderItem.cs
│       └── Services/
│           ├── AuthService.cs
│           ├── JwtService.cs
│           ├── ProductService.cs
│           ├── CategoryService.cs
│           ├── UserService.cs
│           └── OrderService.cs
└── README.md
```

---

## Domain Relationships

### One-to-Many

- User → Orders (a user has many orders)
- Order → OrderItems (an order has many items)
- Product → OrderItems (a product can appear in many order items)
- Category → ProductCategories (a category can relate to many product links)

### Many-to-Many (via join tables with composite keys)

- Product ↔ Category through `ProductCategory`
- User ↔ Role through `UserRole`

Composite keys enforced in `OnModelCreating`:

```csharp
modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK installed
- SQL Server instance (local or container)

### Clone

```bash
git clone <repo-url>
cd ECommerce
```

### Environment Variables

Create `src/ECommerceAPI/.env.local` for development:

```env
ECOMMERCE_CONNECTION_STRING="Server=localhost;Database=ECommerceDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
JWT_KEY="super-secret-key-change"
JWT_ISSUER="ECommerceAPI"
JWT_AUDIENCE="ECommerceAPIAudience"
```

### Restore Tools & Dependencies

```bash
dotnet restore
dotnet tool restore   # restores dotnet-ef from .config/dotnet-tools.json
```

### Apply Migrations (First Time)

If migrations already exist (e.g., initial migration in `Migrations/`):

```bash
dotnet ef database update --project src/ECommerceAPI
```

To add a new migration later:

```bash
dotnet ef migrations add <MigrationName> --project src/ECommerceAPI
dotnet ef database update --project src/ECommerceAPI
```

### Run the API

```bash
dotnet run --project src/ECommerceAPI
```

Visit Swagger UI in development at:

```
http://localhost:5252/swagger/index.html
```

Role seeding runs automatically on startup (see `DatabaseSeeder.SeedRoles`).

### Authentication Flow

1. Register user → receive success message
2. Login → receive JWT token `{ token: "<jwt>" }`
3. Use token in header: `Authorization: Bearer <jwt>`

---

## API Endpoints

### Auth (`/api/Auth`)

| Method | Route              | Auth   | Description           |
| ------ | ------------------ | ------ | --------------------- |
| POST   | /api/Auth/register | Public | Register new user     |
| POST   | /api/Auth/login    | Public | Login and receive JWT |

### Products (`/api/Products`)

| Method | Route              | Auth       | Description       |
| ------ | ------------------ | ---------- | ----------------- |
| GET    | /api/Products      | Public     | List products     |
| GET    | /api/Products/{id} | Public     | Get product by id |
| POST   | /api/Products      | Admin role | Create product    |
| DELETE | /api/Products/{id} | Admin role | Delete product    |

### Categories (`/api/Categories`)

| Method | Route                | Auth       | Description        |
| ------ | -------------------- | ---------- | ------------------ |
| GET    | /api/Categories      | Public     | List categories    |
| GET    | /api/Categories/{id} | Public     | Get category by id |
| POST   | /api/Categories      | Admin role | Create category    |
| DELETE | /api/Categories/{id} | Admin role | Delete category    |

### Users (`/api/Users`)

| Method | Route           | Auth       | Description    |
| ------ | --------------- | ---------- | -------------- |
| GET    | /api/Users      | Admin role | List users     |
| GET    | /api/Users/{id} | Admin role | Get user by id |

### Orders (`/api/Orders`)

| Method | Route          | Auth          | Description                   |
| ------ | -------------- | ------------- | ----------------------------- |
| POST   | /api/Orders    | Auth required | Create order for current user |
| GET    | /api/Orders/my | Auth required | List current user's orders    |

---

## Data Model Summary

| Entity          | Key                    | Important Fields        |
| --------------- | ---------------------- | ----------------------- |
| User            | Id                     | Email, PasswordHash     |
| Role            | Id                     | Name                    |
| UserRole        | (UserId,RoleId)        | Join table              |
| Product         | Id                     | Name, Price             |
| Category        | Id                     | Name                    |
| ProductCategory | (ProductId,CategoryId) | Join table              |
| Order           | Id                     | UserId, CreatedAt       |
| OrderItem       | Id                     | OrderId, ProductId, Qty |

---

## Seeding & Initial Data

Currently only roles are seeded (`Admin`, `User`). Extend `DatabaseSeeder` to seed initial products or categories as needed.

---

## Testing (Future)

Add integration tests under a `tests/` project (scaffold not yet present). Suggested: authentication flow, product CRUD (admin), order placement.

---

## Future Improvements

- Pagination & filtering for product listing
- Soft deletes / audit fields
- Refresh tokens & token revocation
- Order status workflow (Pending, Paid, Shipped, etc.)
- Inventory tracking

---

## License

See `LICENSE` file.

---

## Quick Commands Reference

```bash
# Run API
dotnet run --project src/ECommerceAPI

# Add migration
dotnet ef migrations add AddInventory --project src/ECommerceAPI

# Update database
dotnet ef database update --project src/ECommerceAPI
```

---

## Notes

- Ensure connection string trusts certificate if using local SQL Server.
- Protect `.env` in production; use managed secrets or environment variables.
- Swagger UI only enabled when `ASPNETCORE_ENVIRONMENT=Development`.
