# Delly Belly API - Project Documentation

This project follows the **Clean Architecture** pattern (also known as Onion Architecture). This structure is designed for scalability, maintainability, and ease of testing. It strictly separates concerns, ensuring that the core logic of your bakery business is independent of external frameworks or databases.

## 🏗️ Folder Structure Overview

This visual tree maps your project folders to their purpose:

```text
d:\Freelance Project\Delly Belly - Kuldeep\API\
│
├── 📂 DellyBelly.Domain                (📍 THE CORE)
│   ├── 📂 Entities                     │   └── Database Tables (e.g., Product, Order)
│   ├── 📂 Enums                        │   └── Fixed constants (e.g., OrderStatus)
│   └── 📂 Exceptions                   └── Custom error handling
│
├── 📂 DellyBelly.Application           (🧠 THE BRAINS)
│   ├── 📂 Interfaces                   │   └── Contracts (e.g., IProductService)
│   ├── 📂 Services                     │   └── Business Logic (e.g., ProductService)
│   └── 📂 DTOs                         └── Request/Response models (e.g., CreateOrderDto)
│
├── 📂 DellyBelly.Infrastructure        (🔧 THE TOOLS)
│   ├── 📂 Data                         │   └── Database Context (EF Core)
│   ├── 📂 Migrations                   │   └── Database Schema History
│   └── 📂 Configurations               └── SQL Table configs
│
└── 📂 DellyBellyAPI                    (🔌 THE ENTRY POINT)
    ├── 📂 Controllers                  │   └── API Endpoints (GET/POST)
    ├── 📂 Middlewares                  │   └── Logging, Auth, rate-limiting
    └── 📄 Program.cs                   └── Dependency Injection & Setup
```

---

## 📘 Detailed Explanation

### 1. DellyBelly.Domain (The Core)

**"The Heart of the Bakery"**
This project contains the enterprise logic and types. It has **zero dependencies** on other projects.

- **Entities/**: Database tables (e.g., `Product`, `Category`, `Order`, `ApiLog`).
- **Enums/**: Constant values (e.g., `OrderStatus`, `Role`).
- **Exceptions/**: Custom errors specific to the domain.

### 2. DellyBelly.Application (The Brains)

**"The Business Manager"**
This layer orchestrates how the application works. It depends only on the _Domain_.

- **Interfaces/**: Contracts defining _what_ the system can do, but not _how_.
  - `IProductService`: "We need a way to get products."
- **Services/**: Implementation of the business logic.
  - "When an order is placed, check inventory, calculate total, save to DB, and send email."
- **DTOs/**: (Data Transfer Objects) Simple objects used to pass data between the API and the UI.
  - `CreateProductRequest`: Prevents exposing internal database entities directly to the public.

### 3. DellyBelly.Infrastructure (The Tools)

**"The Worker Logic"**
This layer interacts with outside concerns (Databases, File Systems, 3rd Party APIs).

- **Data/**: Contains `ApplicationDbContext` (Entity Framework Core interaction with SQL Server).
- **Migrations/**: Database history and scripts to create tables.
- **Configurations/**: Specific settings for how tables are built (e.g. `ApiLogConfiguration`).

### 4. DellyBelly.API (The Entry Point)

**"The Storefront Counter"**
This is the only project the outside world (Website, Mobile App) talks to.

- **Controllers/**: API Endpoints (e.g., `GET /api/products`). It receives requests and passes them to the _Application_ layer.
- **Middlewares/**: "Interceptors" that run on every request (e.g., `RequestResponseLoggingMiddleware` which logs every hit to the DB).
- **Program.cs**: The setup file where we wire up Dependency Injection (connecting Interfaces to their Implementations).

---

## 🚀 Capabilities & Future Implementation

### Why this architecture is powerful for "Delly Belly":

1.  **Mobile App Ready (Headless)**

    - **Capability**: You can build a Flutter or React Native mobile app tomorrow.
    - **How**: The Mobile App will simply call the **exact same** `DellyBelly.API` as your website. No new backend code is needed. Your business logic lives in `DellyBelly.Application` and is shared perfectly.

2.  **Database Agnostic**

    - **Capability**: Switch from SQL Server to PostgreSQL or even MongoDB efficiently.
    - **How**: Since the _Domain_ and _Application_ layers don't care which DB you use, you only need to modify `DellyBelly.Infrastructure`.

3.  **High-Performance Logs**

    - **Capability**: Track performance issues immediately.
    - **How**: The centralized `ApiLogs` system we built tracks every millisecond. You can easily query "Which API takes longer than 1 second?" and optimize just that specific part without touching the rest of the system.

4.  **Scalable Team Work**
    - **Capability**: Hire a dedicated backend developer later.
    - **How**: A backend developer can work entirely in `DellyBelly.Application` (Business Logic) without breaking the Frontend, because they are contract-bound by Interfaces.
