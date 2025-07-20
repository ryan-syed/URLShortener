# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Workflow

Follow the **Explore, Plan, Code, Commit** workflow:

1. **Explore**: Use search tools (Grep, Glob, Task) to understand the codebase and locate relevant files
2. **Plan**: Use TodoWrite to break down tasks into specific, actionable steps
   - Create implementation specifications in `docs/specs/` before coding
   - Use descriptive names that reflect the approach (e.g., `random-based-url-shortening.md`)
   - Organize specs by API version: `docs/specs/v1/`, `docs/specs/v2/`, etc.
3. **Code**: Implement changes following existing patterns and conventions
4. **Commit**: Only commit when explicitly requested by the user

## Implementation Specifications

Store detailed implementation plans in the `docs/specs/` directory:

- **Location**: `docs/specs/{version}/{feature-name}.md`
- **Purpose**: Document technical decisions, architecture choices, and implementation details
- **Naming**: Use descriptive names that indicate the approach (e.g., `random-based-url-shortening.md`)
- **Versioning**: Organize by API version to track evolution of features
- **Content**: Include requirements analysis, code snippets, testing strategy, and technical decisions

Example structure:
```
docs/
└── specs/
    ├── v1/
    │   └── random-based-url-shortening.md
    └── v2/
        └── hash-based-url-shortening.md
```

## Project Overview

This is a .NET 9.0 C# ASP.NET Core web application project for URL shortening. The application uses minimal APIs with a clean, organized architecture featuring dependency injection, separation of concerns, and comprehensive testability. 

**Current Features:**
- v1 URL shortening API endpoint (`POST /api/v1/urls`)
- Random-based short code generation using Base62 encoding
- Configurable base URL through appsettings.json

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

The application follows a clean, organized architecture with dependency injection:

### **Core Principles:**
- **Separation of Concerns**: Models, Services, Utilities, and Extensions in separate layers
- **Dependency Injection**: All components use interfaces for testability and flexibility
- **Configuration-Driven**: Base URLs and settings externalized to appsettings.json
- **Minimal API**: Clean, focused endpoint definitions

### **Layer Structure:**
- **Models/V1/**: Data contracts (ShortenUrlRequest, ShortenUrlResponse)
- **Services/**: Business logic with interfaces (IUrlShortenerService, RandomUrlShortenerService)
- **Utilities/**: Reusable components (IBase62Encoder, Base62Encoder)
- **Extensions/**: API endpoint organization (WebApplicationExtensions)
- **Program.cs**: Minimal startup with dependency injection registration

### **Current Endpoints:**
- `GET /`: Welcome message
- `POST /api/v1/urls`: URL shortening endpoint (accepts JSON, returns shortened URL)

### **Configuration:**
- Standard ASP.NET Core configuration with appsettings.json
- BaseUrl configurable for different environments
- Default logging: Information level for general logs, Warning level for ASP.NET Core logs

## Project Structure

- `URLShortener.sln`: Solution file containing the main URLShortener project
- `URLShortener/`: Main application project
  - `Program.cs`: Minimal application entry point with dependency injection
  - `URLShortener.csproj`: Project file with .NET 9.0 configuration
  - `Properties/launchSettings.json`: Development server configuration
  - `appsettings.json` & `appsettings.Development.json`: Application configuration files
  - `Models/V1/`: Data contracts and DTOs
    - `ShortenUrlRequest.cs`: Request model for URL shortening
    - `ShortenUrlResponse.cs`: Response model with shortened URL details
  - `Services/`: Business logic layer
    - `IUrlShortenerService.cs`: Interface for URL shortening services
    - `V1/RandomUrlShortenerService.cs`: v1 implementation using random-based short codes
  - `Utilities/`: Reusable utility components
    - `IBase62Encoder.cs`: Interface for Base62 encoding
    - `Base62Encoder.cs`: Implementation of Base62 encoding algorithm
  - `Extensions/`: API and application extensions
    - `WebApplicationExtensions.cs`: Endpoint mapping and API organization
- `docs/`: Documentation and specifications
  - `specs/`: Implementation specifications organized by version
    - `v1/random-based-url-shortening.md`: Original v1 implementation spec
    - `v1/code-organization-refactor.md`: Refactoring specification

## Development Guidelines

### **Adding New Features:**
1. Create specification in `docs/specs/{version}/`
2. Follow existing layer structure (Models, Services, Utilities, Extensions)
3. Use dependency injection patterns with interfaces
4. Add configuration to appsettings.json when needed
5. Update this CLAUDE.md file if architecture changes

### **Testing Strategy:**
- All components are designed for unit testing with dependency injection
- Mock interfaces (IBase62Encoder, IUrlShortenerService, IConfiguration) for isolated testing
- Integration tests can target the full API endpoints
- Future: Add URLShortener.Tests project when implementing unit tests