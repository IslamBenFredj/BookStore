# BookStore API - .NET 9 Clean Architecture Project

## Project Overview

This is a C# .NET 9 project that follows clean code principles and incorporates the following concepts: OOP principles, LINQ, Dependency Injection, and SOLID principles.

**Proposed approach:** An ASP.NET Core Web API (minimal) named "BookStore" with a simple business domain (books, authors, users). This project will cover OOP, LINQ, DI, ASP.NET Core Identity + JWT Bearer, SOLID principles, unit testing, and some simple patterns (Repository, Service, DTO).

## Key Features Covered

- Object-Oriented Programming (OOP)
- LINQ queries
- Dependency Injection
- ASP.NET Core Identity with JWT Bearer authentication
- SOLID principles
- Unit testing
- Design patterns: Repository, Service, DTO

The BookStore application will manage a simple book inventory system with related authors and user management functionality.

## Technologies & Packages

```bash
🧱 ASP.NET Core Identity & Authentication
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt