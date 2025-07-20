# Code Organization Refactoring Plan

## Overview
Refactor the v1 URL shortening code from Program.cs into a proper project structure to enable unit testing and better maintainability.

## Current Issues
- All code is in Program.cs (business logic, models, utilities)
- Static methods can't be easily unit tested
- No separation of concerns
- Difficult to mock dependencies for testing

## Proposed Structure

```
URLShortener/
├── Models/
│   └── V1/
│       ├── ShortenUrlRequest.cs
│       └── ShortenUrlResponse.cs
├── Services/
│   ├── IUrlShortenerService.cs
│   └── V1/
│       └── RandomUrlShortenerService.cs
├── Utilities/
│   ├── IBase62Encoder.cs
│   └── Base62Encoder.cs
├── Extensions/
│   └── WebApplicationExtensions.cs
└── Program.cs (minimal startup code only)
```

## Planned Changes

### 1. Extract Models
**Files**: `Models/V1/ShortenUrlRequest.cs`, `Models/V1/ShortenUrlResponse.cs`
**Purpose**: Separate data contracts from business logic

```csharp
// Models/V1/ShortenUrlRequest.cs
namespace URLShortener.Models.V1;

public record ShortenUrlRequest(string Url);

// Models/V1/ShortenUrlResponse.cs  
namespace URLShortener.Models.V1;

public record ShortenUrlResponse
{
    public required string ShortUrl { get; init; }
    public required string ShortCode { get; init; }
    public required string OriginalUrl { get; init; }
}
```

### 2. Extract Base62 Encoding Utility
**Files**: `Utilities/IBase62Encoder.cs`, `Utilities/Base62Encoder.cs`
**Purpose**: Make encoding logic testable and injectable

```csharp
// Utilities/IBase62Encoder.cs
namespace URLShortener.Utilities;

public interface IBase62Encoder
{
    string Encode(double number);
}

// Utilities/Base62Encoder.cs
namespace URLShortener.Utilities;

public class Base62Encoder : IBase62Encoder
{
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    
    public string Encode(double number)
    {
        var longValue = (long)(number * long.MaxValue);
        if (longValue == 0) return "0";
        
        var result = "";
        while (longValue > 0)
        {
            result = Chars[(int)(longValue % 62)] + result;
            longValue /= 62;
        }
        
        return result;
    }
}
```

### 3. Create URL Shortening Service
**Files**: `Services/IUrlShortenerService.cs`, `Services/V1/RandomUrlShortenerService.cs`
**Purpose**: Encapsulate business logic for URL shortening

```csharp
// Services/IUrlShortenerService.cs
using URLShortener.Models.V1;

namespace URLShortener.Services;

public interface IUrlShortenerService
{
    ShortenUrlResponse ShortenUrl(ShortenUrlRequest request);
}

// Services/V1/RandomUrlShortenerService.cs
using URLShortener.Models.V1;
using URLShortener.Services;
using URLShortener.Utilities;

namespace URLShortener.Services.V1;

public class RandomUrlShortenerService : IUrlShortenerService
{
    private readonly IBase62Encoder _encoder;
    private readonly IConfiguration _configuration;
    
    public RandomUrlShortenerService(IBase62Encoder encoder, IConfiguration configuration)
    {
        _encoder = encoder;
        _configuration = configuration;
    }
    
    public ShortenUrlResponse ShortenUrl(ShortenUrlRequest request)
    {
        var randomNumber = Random.Shared.NextDouble();
        var shortCodeFull = _encoder.Encode(randomNumber);
        var shortCode = shortCodeFull.Substring(0, Math.Min(8, shortCodeFull.Length));
        
        var baseUrl = _configuration["BaseUrl"] ?? "http://localhost:5127";
        
        return new ShortenUrlResponse
        {
            ShortUrl = $"{baseUrl}/{shortCode}",
            ShortCode = shortCode,
            OriginalUrl = request.Url
        };
    }
}
```

### 4. Create Extension Methods for API Registration
**File**: `Extensions/WebApplicationExtensions.cs`
**Purpose**: Keep Program.cs minimal and organize API endpoints

```csharp
using URLShortener.Models.V1;
using URLShortener.Services;

namespace URLShortener.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapUrlShortenerEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello from URL Shortener!");
        
        app.MapPost("/api/v1/urls", (ShortenUrlRequest request, IUrlShortenerService service) =>
        {
            return service.ShortenUrl(request);
        });
        
        return app;
    }
}
```

### 5. Update Program.cs
**File**: `Program.cs`
**Purpose**: Minimal startup code with dependency injection

```csharp
using URLShortener.Extensions;
using URLShortener.Services;
using URLShortener.Services.V1;
using URLShortener.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<IBase62Encoder, Base62Encoder>();
builder.Services.AddSingleton<IUrlShortenerService, RandomUrlShortenerService>();

var app = builder.Build();

// Map endpoints
app.MapUrlShortenerEndpoints();

app.Run();
```

### 6. Add Configuration Support
**File**: `appsettings.json`
**Purpose**: Make base URL configurable

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BaseUrl": "http://localhost:5127"
}
```

## Benefits of This Structure

### Testability
- Each component can be unit tested in isolation
- Dependencies can be mocked using interfaces
- Business logic is separated from infrastructure

### Maintainability
- Clear separation of concerns
- Easy to add new encoding strategies (v2, v3)
- Configuration is externalized
- Code is organized by responsibility

### Extensibility
- New URL shortening strategies can implement IUrlShortenerService
- New encoding methods can implement IBase62Encoder
- Easy to add validation, caching, or persistence layers

## Testing Strategy

### Unit Tests Structure
```
URLShortener.Tests/
├── Models/
│   └── V1/
├── Services/
│   └── V1/
│       └── RandomUrlShortenerServiceTests.cs
├── Utilities/
│   └── Base62EncoderTests.cs
└── Extensions/
    └── WebApplicationExtensionsTests.cs
```

### Key Test Scenarios
1. **Base62Encoder Tests**
   - Encode various numbers correctly
   - Handle edge cases (zero, very large numbers)
   - Verify character set usage

2. **RandomUrlShortenerService Tests**
   - Generate short codes of correct length
   - Use proper base URL from configuration
   - Handle different input URLs correctly

3. **Integration Tests**
   - End-to-end API endpoint testing
   - Verify JSON serialization/deserialization

## Migration Steps

1. Create directory structure
2. Extract models to separate files
3. Create and test Base62Encoder utility
4. Create and test RandomUrlShortenerService
5. Create WebApplicationExtensions
6. Update Program.cs with DI
7. Update configuration files
8. Verify application still works
9. Create unit test project and tests

## Backwards Compatibility
- API endpoints remain unchanged
- Response format stays the same
- Configuration is additive (BaseUrl optional)
- No breaking changes to external interface