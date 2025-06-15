# SetupCmd Test Framework (xUnit)

This project provides an automated test suite for validating the `SetupCmd.exe` console application using xUnit. The tests focus on verifying the `configureDatabaseConnection` command for supported database types: **MySQL**, **MSSQL**, and **Oracle**.

---

## 🔧 Prerequisites

- .NET 8 SDK
- xUnit test runner (built-in with `dotnet test`)
- A compiled version of `SetupCmd.exe` located in: Fixtures folder

## 📂 Project Structure

/SetupCmd.Tests
│
├── Fixtures/   # Keep all test data releated files here
│ └── DbConnections.json # Test input data for database connections
  └── SetupCmd.exe # Test execution file
│
├── Tests/      # Keep all test and test execution releated files here   

## 🚀 Running the Tests

execute: dotnet test