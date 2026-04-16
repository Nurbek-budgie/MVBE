# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

Build / run (from repo root):

```bash
dotnet restore                         # restore all projects
dotnet build MVBE.sln                  # build whole solution
dotnet run --project API                # run API (http://localhost:5276, https://localhost:7109, Swagger at /swagger)
```

Database (Postgres required — use `docker compose up mvapp_db` to start it on `localhost:5432`, db `MvDatabase`, user/pass `postgres/postgres`). Migrations run automatically on startup via `DbInitializer.Initialize` in `API/Program.cs`.

Required secrets (the API fails fast if `Jwt:Key` is missing or < 32 bytes). For local dev use user-secrets:

```bash
dotnet user-secrets init   --project API
dotnet user-secrets set "Jwt:Key"       "$(openssl rand -base64 48)" --project API
# Optional: seeds an admin user on first boot (Development only)
dotnet user-secrets set "Admin:Email"    "you@example.com" --project API
dotnet user-secrets set "Admin:Password" "<strong password>" --project API
```

In production set `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` via env vars. The admin seeder is gated to `IsDevelopment()`.

EF Core migrations — the DbContext lives in `DAL` but migrations are added/applied with `API` as the startup project:

```bash
dotnet ef migrations add <Name> --project DAL --startup-project API
dotnet ef database update            --project DAL --startup-project API
dotnet ef migrations remove          --project DAL --startup-project API
```

Docker: `docker compose up --build` builds `API/Dockerfile` and starts Postgres together.

No test project exists in the solution.

## Architecture

Classic layered .NET 8 Web API. Dependency direction is strictly one-way: **API → BLL → Repository → DAL**, with **DTO** and **Common** as leaf projects referenced across layers.

- **API** — ASP.NET Core host. Controllers are organised by feature folder (`Controllers/Auth`, `Controllers/Movie`, `Controllers/Reservation`). All service wiring happens in `API/Configurations/ServiceRegistration.cs` — `RegisterServices` is the single entry point called from `Program.cs` and chains `AddDependencyInjection`, `RegisterJWTAuth`, `AddAutoMapperConfiguration`, `AddCorsConfiguration`. Add new services here.
- **BLL** — `Services/<Feature>` implementing `Interfaces/<Feature>` contracts. Services map between DTOs (from `DTO`) and entities (from `DAL.Models`) using the single `AutoMapperProfile` in `BLL/Configurations`.
- **Repository** — Generic `BaseRepository<T,K> : IBaseRepository<T,K>` provides `Create/GetById/GetAll/Update/Delete/Exists`. Feature repositories (e.g. `MovieRepository`) inherit it and add query-specific methods. `Repository/Service/FileStorageService` writes uploads to `API/wwwroot/<folder>/<guid>.<ext>` and returns a relative URL.
- **DAL** — `AppDbContext : IdentityDbContext<User, Role, Guid, ...>` (Npgsql). Identity tables are renamed in `OnModelCreating` (`AspNetUsers` → `Users`, etc.). Models split into `Models/Identity` and `Models/Movie`. Migrations in `DAL/Migrations`.
- **DTO** — Nested DTO classes per feature, typically `MovieDto.Create`, `MovieDto.Read`, `MovieDto.List`, etc. `DTO/Middleware/ApiResponse<T>` is the common success/failure envelope.
- **Common** — Enums only. `ERoles` (Admin/Client/Audience/Manager) drives authorization; `Common/Enums/MovieEnums` holds domain enums (`ScreenType`, `BookingStatus`, `PaymentMethod`, `PaymentStatus`).

## Auth model

- JWT bearer auth configured in `RegisterJWTAuth`. Signing key / issuer / audience come from `appsettings.json`'s `Jwt` section.
- `DbSeeder` (run from `Program.cs`) seeds every `ERoles` value as a role and creates an `admin@admin.com` user (password `string`) in the `Admin` role on first boot.
- Role-gated endpoints use the custom `[AuthorizeRole(ERoles.X, ...)]` attribute in `API/Configurations/AuthorizeRoleAttribute.cs`. It inherits from `AuthorizeAttribute` so the JwtBearer middleware validates the token (signature / issuer / audience / expiry) before the filter runs; the filter then checks `ClaimTypes.Role`/`role` claims on the validated principal. 401 for unauthenticated, 403 for wrong role. Use `AuthorizeRole` for consistency with the rest of the codebase.
- `AuthService.BuildResponse` emits both access (15 min) and refresh (7 day) tokens and includes `theaterId` as a claim when the user has one.

## Error handling

`ErrorHandlerMiddleware` wraps the pipeline and serialises any unhandled exception to `ApiResponse<string>.Failure(ex.Message)` with status 500. Throw exceptions from services/repositories rather than returning error codes; the middleware handles the conversion. Controllers still return `NotFound()` / `BadRequest()` for expected business-rule failures.

## Conventions worth matching

- Controllers are thin: they call the feature service, map `null` → `NotFound()` / `BadRequest()`, and otherwise `Ok(result)`.
- Services take the `IMapper` + feature repository via constructor injection and null-check both.
- Feature repositories are registered as concrete types (e.g. `services.AddScoped<MovieRepository>()`) — no `IMovieRepository` interface — while services are registered against their interface.
- File uploads go through `IFileStorageService.SaveFileAsync(stream, fileName, folder)` which stores under `API/wwwroot/<folder>` and returns the public relative path.
- JSON enum serialisation uses `JsonStringEnumConverter` globally (set in `ServiceRegistration`), so new enums serialise as their name by default.
