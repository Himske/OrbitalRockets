# Orbital Flight Systems

An ASP.NET Core MVC application for managing rockets and the crews assigned to them. The interface uses a dark, starfield-inspired mission-control theme.

## Features

- Create, view, edit, and delete rocket records, including manufacturer details.
- Organize rocket crews and associate them with rockets.
- Manage crew members and their roles within a crew.
- Browse related rocket, crew, and crew-member details.

## Technology

- .NET 10 / ASP.NET Core MVC
- Entity Framework Core 10
- SQL Server
- Bootstrap 5

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server or SQL Server Express
- Entity Framework Core CLI (`dotnet-ef`) to apply database migrations

Install the EF CLI if it is not already available:

```powershell
dotnet tool install --global dotnet-ef
```

## Configuration

The default connection string is in `appsettings.json` and targets a local SQL Server Express instance:

```text
Server=localhost\SQLEXPRESS;Database=space_port_mvc;Trusted_Connection=True;TrustServerCertificate=True;
```

Make sure the SQL Server instance is running and accessible. To use a different instance or database, update the `WebAppMVCContext` connection string. You can also override it without editing the settings file by setting the `ConnectionStrings__WebAppMVCContext` environment variable.

## Run locally

From the project directory, restore dependencies, apply migrations, and start the application:

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

The configured launch profiles use:

- HTTP: `http://localhost:5148`
- HTTPS: `https://localhost:7015`

The default route opens the Mission Control home page. Use the navigation to open the rocket registry, flight crews, and crew roster.

## Build

```powershell
dotnet build
```

## Project structure

```text
Controllers/    MVC controllers for rockets, crews, and crew members
Models/         Entity models and crew-role definitions
ViewModels/     Form models used by create and edit pages
Views/          Razor pages and the shared layout
Migrations/     Entity Framework Core database migrations
wwwroot/        CSS, JavaScript, and frontend libraries
```
