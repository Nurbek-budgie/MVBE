# MVBE

Movie Venue Booking Engine — a .NET 8 REST API for managing movie theaters, screenings, and seat reservations.

## Tech stack

- C# / .NET 8
- Entity Framework Core 9 with PostgreSQL (Npgsql)
- ASP.NET Identity with JWT Bearer authentication
- AutoMapper
- Swagger / OpenAPI (Swashbuckle)
- Docker & Docker Compose

## Project structure

| Project | Purpose |
| --- | --- |
| `API/` | ASP.NET Core entry point, controllers, middleware (`API/Program.cs`) |
| `BLL/` | Business logic services (movies, screenings, reservations, identity) |
| `DAL/` | EF Core `AppDbContext`, entities, migrations (`DAL/EF/AppDbContext.cs`) |
| `Repository/` | Repository pattern over EF Core |
| `DTO/` | Data transfer objects used by the API |
| `Common/` | Shared enums and utilities (e.g. `ERoles`) |

The solution file is `MVBE.sln`.

## Prerequisites

- .NET 8 SDK
- Docker and Docker Compose (to run PostgreSQL), or a local PostgreSQL 16 instance

## Getting started

1. Clone the repository and restore dependencies:

   ```
   dotnet restore
   ```

2. Start PostgreSQL via Docker Compose (see `compose.yaml` — database `MvDatabase` on port 5432):

   ```
   docker-compose up -d
   ```

3. Run the API:

   ```
   dotnet run --project API
   ```

On startup the database is initialized and seeded (including a default admin user) via `DbInitializer` / `DbSeeder`. In Development, Swagger UI is served at the API root.

## Configuration

Connection strings, JWT options, and CORS settings live in:

- `API/appsettings.json`
- `API/appsettings.Development.json`

The CORS policy `AllowReactApp` is wired up in `API/Program.cs` for the frontend client.

## API overview

| Route group | Description |
| --- | --- |
| `/api/auth` | Login and JWT issuance |
| `/api/users`, `/api/adminusers` | User and admin-user management |
| `/api/movies`, `/api/featured-movies` | Movie catalog and featured movies |
| `/api/theaters`, `/api/screens`, `/api/screenings` | Venues and schedule |
| `/api/reservations` | Seat reservations |

Several endpoints require the `Admin` role via role-based authorization.

## Docker

A multi-stage build is defined in `API/Dockerfile`, and `compose.yaml` provides the PostgreSQL service used during development.
