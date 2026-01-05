# GarageERP Project Structure

## Overview

GarageERP is a desktop ERP application for a car garage, built using **.NET 10**, **WPF**, and a **local SQLite database**.  
The solution follows **Clean Architecture principles** with 4 projects.

---

## Projects Summary

| Project | Purpose | Responsibilities | Folder Structure | Dependencies |
|---------|---------|-----------------|-----------------|--------------|
| **GarageERP.Domain** | Defines core business entities | Represents “what exists”; pure C# classes; no DB or UI logic | `Entities/`, `ValueObjects/`, `Enums/` | None |
| **GarageERP.Application** | Implements business rules and workflows | Represents “what happens”; validates data; contains services; does not access DB | `Interfaces/`, `Services/`, `DTOs/` | Domain |
| **GarageERP.Infrastructure** | Data access and technical details | Represents “how it’s done”; EF Core DbContext, repositories, migrations; configures SQLite | `Data/`, `Repositories/`, `Migrations/` | Domain, Application |
| **GarageERP.Wpf** | User Interface using WPF and MVVM | Represents “what the user sees”; XAML views, ViewModels, commands, navigation; interacts only with Application | `Views/`, `ViewModels/`, `Commands/` | Domain, Application, Infrastructure |

---

## Dependency Overview

GarageERP.Domain → no references

GarageERP.Application → references Domain

GarageERP.Infrastructure → references Domain, Application

GarageERP.Wpf → references Domain, Application, Infrastructure

---

## Data & Control Flow
WPF (UI Layer)

↓

Application (Business Logic)

↓

Infrastructure (EF Core / Repositories)

↓

SQLite Database


- **UI never talks to DB directly**
- **Entities are shared across layers**
- **Application contains business rules, not database logic**
