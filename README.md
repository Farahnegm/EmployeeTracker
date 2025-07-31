#  CodeZone Attendance System
CodeZone Attendance System is a robust and user-friendly ASP.NET Core MVC web application designed to manage employees, departments, and attendance records. Built with clean architecture and modular design, the system ensures maintainability, scalability, and performance.

# Features
## Employee Management
Add, edit, view, and delete employees

Each employee is assigned a unique, randomly generated code

View employee attendance history

## Department Management
Organize employees into departments

Add, edit, and delete departments with intuitive forms and validation

## Attendance Tracking
Record daily attendance with a visual calendar UI

Mark, edit, and view attendance per employee

See attendance summaries at a glance

## Live Search & Filtering
Real-time search and filtering on attendance records

Search by employee, department, or date range

Powered by AJAX for a seamless experience

## AJAX-Powered Operations
Delete employees, departments, and attendance records without page reload

Instantly updates UI with success/failure messages

Validation & Business Rules
FluentValidation for clear server-side and client-side validation

Business logic is cleanly separated into services and validators

## Modular UI
Uses partial views for reusable components

Clean layout using Bootstrap 5

# Technologies Used
✅ ASP.NET Core MVC (.NET 8)

✅ Entity Framework Core

✅ FluentValidation

✅ jQuery & AJAX

✅ Bootstrap 5

✅ Clean Architecture:

UI Layer

BLL (Business Logic Layer)

DAL (Data Access Layer)

#🚀 Getting Started
## Prerequisites
.NET 6 SDK or later

SQL Server (optional; defaults to In-Memory DB for testing)

Visual Studio 2022+ or Visual Studio Code   

# Project Structure
CodeZone.AttendanceSystem/
 #### CodeZone.UI       ASP.NET Core MVC - UI layer (controllers, views, JS)
 #### CodeZone.BLL      Business Logic Layer (services, validation, DTOs)
 #### CodeZone.DAL      Data Access Layer (DbContext, entities, repositories)


# Enjoy using CodeZone Attendance System!


