# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 9.0 C# ASP.NET Core web application project for URL shortening. The application uses minimal APIs and is currently in early development with a basic "Hello World" endpoint.

## Development Commands

### Build and Run
```bash
# Build the project
dotnet build

# Run the application in development mode
dotnet run --project URLShortener

# Run with hot reload
dotnet watch --project URLShortener
```

### Testing
```bash
# Run tests (when test projects are added)
dotnet test
```

### Project Management
```bash
# Restore dependencies
dotnet restore

# Clean build artifacts
dotnet clean

# Publish for production
dotnet publish URLShortener -c Release -o ./publish
```

## Application Configuration

- **Development URL**: https://localhost:7102 (HTTPS) or http://localhost:5127 (HTTP)
- **Target Framework**: .NET 9.0
- **Features Enabled**: Nullable reference types, implicit usings
- **Launch Profile**: Configured to open browser automatically in development

## Architecture

The application follows ASP.NET Core minimal API patterns:
- Entry point: `URLShortener/Program.cs` - Contains the main application setup using `WebApplication.CreateBuilder()`
- Currently implements a single GET endpoint at root ("/") returning "Hello World!"
- Standard ASP.NET Core configuration with appsettings.json for different environments
- Uses default logging configuration with Information level for general logs and Warning level for ASP.NET Core logs

## Project Structure

- `URLShortener.sln`: Solution file containing the main URLShortener project
- `URLShortener/`: Main application project
  - `Program.cs`: Application entry point and configuration
  - `URLShortener.csproj`: Project file with .NET 9.0 configuration
  - `Properties/launchSettings.json`: Development server configuration
  - `appsettings.json` & `appsettings.Development.json`: Application configuration files