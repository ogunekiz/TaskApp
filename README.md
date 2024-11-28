# Project Name

A full stack application developed with .NET Core 8.0 API and React, using SQLite database. The project is structured with a layered architecture. **ErrorHandler Middleware** is used for error tracing, **Serilog** is used for logging. Tests are written in **xUnit**.

---

## Features

- **Backend**: .NET Core 8.0 Web API
- **Frontend**: React
- **Database**: SQLite
- **Architecture**: Layered architecture
- **Error Handling**: ErrorHandler Middleware
- **Logging**: Serilog
- **Test Framework**: xUnit

---

## Project Architecture
The project was developed with layered architecture principles:

API Layer: Provides communication with clients.
Business Logic Layer (BLL): Contains business logic.
Data Access Layer (DAL): Manages database operations.
Entity Layer: Contains data models.

---

## Technologies Used
.NET Core 8.0
React
SQLite
Serilog
xUnit

---

## Running the Project

### Required Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)

### Backend Installation

1. **Download and Install API Project**:
```bash
git clone https://github.com/ogunekiz/TaskApp.git
cd TaskApp/Backend/TaskApp.API
dotnet run
````

### Frontend Installation

1. **Download and Install React Project**:
```bash
git clone https://github.com/ogunekiz/TaskApp.git
cd TaskApp/Frontend
npm install
npm start
