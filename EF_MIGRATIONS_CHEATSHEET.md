# 📋 EF Core Migrations Cheatsheet — DellyBelly API
> Run all commands from the **Package Manager Console (PMC)** in Visual Studio
> **Tools → NuGet Package Manager → Package Manager Console**
>
> ⚠️ Always use `-Project` and `-StartupProject` explicitly to avoid "migrations assembly mismatch" errors.
> - `-Project`        → `DellyBelly.Infrastructure`  (where migration FILES live)
> - `-StartupProject` → `DellyBelly.API`              (has appsettings.json + DbContext config)

---

## ✅ Add a New Migration

```powershell
Add-Migration <MigrationName> -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

**Example:**
```powershell
Add-Migration AddApiLogColumnConstraints -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## ✅ Apply Migrations to Database

```powershell
Update-Database -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## ✅ Apply Up to a Specific Migration

```powershell
Update-Database <MigrationName> -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

**Example (rollback to a previous migration):**
```powershell
Update-Database AddInitialSchema -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## ✅ Remove the Last Migration (if NOT yet applied to DB)

```powershell
Remove-Migration -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## ✅ List All Migrations & Their Status

```powershell
Get-Migration -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## ✅ Generate SQL Script (without applying)

Useful to review what SQL will be executed before applying.

```powershell
Script-Migration -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

**From a specific migration to latest:**
```powershell
Script-Migration <FromMigration> -Project DellyBelly.Infrastructure -StartupProject DellyBelly.API
```

---

## 🔑 Key Rules

| Concept                        | Detail                                                                 |
|-------------------------------|------------------------------------------------------------------------|
| Migration files location       | `DellyBelly.Infrastructure/Migrations/`                               |
| Always use `-Project`          | `DellyBelly.Infrastructure` — never leave this to the PMC dropdown    |
| Always use `-StartupProject`   | `DellyBelly.API` — this is the runnable project with `appsettings.json` |
| Safe to re-run `Update-Database` | Yes — EF tracks which migrations are already applied in `__EFMigrationsHistory` table |
| Rollback = run `Update-Database <OlderName>` | EF will call `Down()` on all newer migrations automatically |

---

## ⚠️ Common Errors & Fixes

| Error | Fix |
|---|---|
| `Your target project doesn't match your migrations assembly` | Always add `-Project DellyBelly.Infrastructure` |
| `No migrations configuration type was found` | Make sure Default project in PMC is Infrastructure OR use `-Project` flag |
| `Unable to create an object of type 'ApplicationDbContext'` | Make sure `-StartupProject` is set to `DellyBelly.API` |
| `The migration has already been applied` | Run `Get-Migration` to check status; use `Update-Database <PrevName>` to rollback first |
