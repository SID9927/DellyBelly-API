# Delly Belly Backend API — Architecture & Performance Guide

This guide describes the Clean Architecture design, security practices, and performance caching patterns (like ETag caching) implemented in the Delly Belly backend API.

---

## 🏗️ 1. Architecture Overview

The backend is built following **Clean Architecture / Onion Architecture** principles. This pattern keeps the business rules separate from frameworks, database drivers, and the user interface.

```text
DellyBelly Backend API Solution
│
├── 📂 DellyBelly.Domain                (📍 THE CORE)
│   ├── Entities/                       └── Core database tables (Product, Customer, Category, ImageEntity, SiteSettings)
│   └── Enums/                          └── System constants (OrderStatus, Roles)
│
├── 📂 DellyBelly.Application           (🧠 THE BRAIN)
│   ├── Interfaces/                     └── Service contracts (IProductService, ICategoryService)
│   ├── Services/                       └── Business logic implementations (ProductService, CategoryService)
│   └── DTOs/                           └── Plain request/response objects (CreateProductDto, SiteSettingsDto)
│
├── 📂 DellyBelly.Infrastructure        (🔧 THE TOOLS / DATA ACCESS)
│   ├── Data/                           └── ApplicationDbContext (EF Core Database context)
│   ├── Migrations/                     └── Entity Framework database schema versioning
│   └── Configurations/                 └── Table mapping configuration rules
│
├── 📂 DellyBelly.Shared                (🛠️ SHARED HELPERS)
│   └── Helpers/                        └── DateTimeHelper, ImageHelper
│
└── 📂 DellyBellyAPI                    (🔌 PRESENTATION LAYER / API HOST)
    ├── Controllers/                    └── API endpoints (ProductsController, CustomerAuthController)
    ├── Middlewares/                    └── Error handling & logging middleware
    └── Program.cs                      └── Service registrations and environment startup pipeline
```

### Why this structure is beneficial:
* **Separation of Concerns:** Each layer has a single responsibility.
* **Database Agnostic:** The core logic knows nothing about the SQL Server database; changing DB providers only requires updating the Infrastructure layer.
* **Framework Independent:** Core models are pure C# classes, allowing for easier upgrades or portability.
* **Testability:** Business rules can be tested in isolation without connecting to a live database.

---

## ⚡ 2. ETag Caching (Browser-Side Caching Integration)

To optimize load times and save server resources, **ETag (Entity Tag) caching** has been implemented for all photo retrieval endpoints.

### Endpoints Configured:
1. **Gallery Photos:** `GET /api/gallery/{id}/photo`
2. **Product Photos:** `GET /api/products/{id}/photo/{imageId}`
3. **Category Photos:** `GET /api/categories/{id}/photo`

### How the ETag Flow Works Automatically:
1. **First Request:**
   * The frontend requests a photo.
   * The API executes a lightweight query to get only the metadata (excluding the heavy image binary `Data` block).
   * It generates an ETag header based on the database record's `UploadedAt.Ticks` timestamp (e.g., `ETag: "img-12-638843940280000000"`).
   * It appends the ETag and `Cache-Control: public, max-age=86400, immutable` headers.
   * The browser downloads the image and stores it in its local cache alongside the ETag.

2. **Subsequent Requests:**
   * The browser automatically attaches the ETag to its request using the header:
     ```http
     If-None-Match: "img-12-638843940280000000"
     ```
   * **API Intercept:** The API receives the request, queries only the image's timestamp from the DB, and compares it.
   * **Result:** If they match, the API halts execution and immediately returns **`304 Not Modified`** without retrieving or serving the heavy image file.
   * The browser instantly loads the image directly from its local disk cache. No unnecessary bandwidth is wasted, and database load is dramatically reduced.

---

## 🔒 3. Security & Access Control

The backend is fully secured to prevent unauthorized access or modification of sensitive data:

### Public vs. Private Endpoints
* **Public Data** (fetching products, categories, gallery listing, photos): Publicly accessible to allow customers to view the store's menu without needing login credentials.
* **Private/Admin Data** (modifying products, uploading photos, viewing metrics): Guarded with Role-Based Access Control:
  ```csharp
  [Authorize(Roles = "super_admin,admin,manager,staff")]
  ```
  Unauthenticated requests or requests from users without these roles are automatically blocked with a `401 Unauthorized` or `403 Forbidden` response.

### Transit Security (HTTPS)
In production, all communication is wrapped inside **HTTPS (SSL)**. This ensures all headers, cookies, query parameters, request bodies, and database payloads are encrypted end-to-end between the client browser and your ASP.NET Core server.
