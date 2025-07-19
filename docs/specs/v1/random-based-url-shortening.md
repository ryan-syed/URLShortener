# Implementation Journal: v1 URL Shortening Endpoint

## Overview
Creating a v1 API endpoint that accepts a long URL and returns a shortened URL using random number generation and Base62 encoding.

## Requirements Analysis
- **Input**: Long URL (e.g., "https://www.example.com/some/very/long/url")
- **Process**: 
  1. Generate random number using `Math.Random()`
  2. Encode using Base62 algorithm
  3. Take first 8 characters as short code
- **Output**: Short URL with the generated code
- **Endpoint**: POST /api/v1/urls (v1 for future versioning)

## Planned Changes

### 1. Base62 Encoding Utility
**File**: `URLShortener/Program.cs`
**Purpose**: Convert random numbers to Base62 strings

```csharp
static string Base62Encode(double number)
{
    const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    
    // Convert double to integer for encoding
    var longValue = (long)(number * long.MaxValue);
    if (longValue == 0) return "0";
    
    var result = "";
    while (longValue > 0)
    {
        result = chars[(int)(longValue % 62)] + result;
        longValue /= 62;
    }
    
    return result;
}
```

**Considerations**:
- Using standard Base62 character set (0-9, A-Z, a-z)
- Converting double to long for mathematical operations
- Need to handle edge case of zero

### 2. Request/Response Models
**File**: `URLShortener/Program.cs`
**Purpose**: Define data contracts for the API

```csharp
record ShortenUrlRequest(string Url);

record ShortenUrlResponse
{
    public required string ShortUrl { get; init; }
    public required string ShortCode { get; init; }
    public required string OriginalUrl { get; init; }
}
```

**Considerations**:
- Using C# records for immutable data structures
- `required` keyword ensures properties are set
- Response includes both short URL and code for flexibility

### 3. API Endpoint Implementation
**File**: `URLShortener/Program.cs`
**Location**: After existing MapGet, before app.Run()

```csharp
app.MapPost("/api/v1/urls", (ShortenUrlRequest request) =>
{
    var randomNumber = Random.Shared.NextDouble();
    var shortCodeFull = Base62Encode(randomNumber);
    var shortCode = shortCodeFull.Substring(0, Math.Min(8, shortCodeFull.Length));
    
    return new ShortenUrlResponse
    {
        ShortUrl = $"http://localhost:5127/{shortCode}",
        ShortCode = shortCode,
        OriginalUrl = request.Url
    };
});
```

**Considerations**:
- Using `Random.Shared` for thread-safe random generation
- Truncating to 8 characters as specified
- Using Math.Min to prevent index errors
- Hardcoded base URL (could be configurable in future)
- v1 path structure for future API versioning

## Technical Decisions

### Random Number Generation
- **Choice**: `Random.Shared.NextDouble()`
- **Rationale**: Thread-safe, built-in .NET approach
- **Alternative**: Could use crypto-random for better uniqueness

### Base62 Implementation
- **Choice**: Custom implementation using modulo arithmetic
- **Rationale**: Simple, no external dependencies
- **Character Set**: 0-9, A-Z, a-z (62 characters total)

### URL Construction
- **Choice**: Hardcoded localhost URL for now
- **Rationale**: Simple for v1, can be made configurable later
- **Future**: Should use configuration or request context

### Data Storage
- **Current**: In-memory only (no persistence)
- **Limitation**: URLs will be lost on restart
- **Future**: Add database storage in later versions

## Potential Issues & Mitigations

1. **Collision Risk**: Random generation could produce duplicate codes
   - **Mitigation**: For v1, accept the risk; future versions will check for uniqueness

2. **URL Validation**: No validation of input URL format
   - **Mitigation**: For v1, trust client input; add validation in future versions

3. **Short Code Length**: Fixed 8 characters might not always be optimal
   - **Mitigation**: v1 constraint; future versions can be dynamic

4. **No Persistence**: URLs lost on application restart
   - **Mitigation**: Known limitation for v1; database integration planned for v2

## Testing Strategy

### Manual Testing
1. Start application with `dotnet run`
2. Send POST request to `/api/v1/urls` with JSON body
3. Verify response contains short URL and code
4. Confirm short code is 8 characters or less
5. Test with various URL formats

### Example Test Request
```bash
curl -X POST http://localhost:5127/api/v1/urls \
  -H "Content-Type: application/json" \
  -d '{"url": "https://www.example.com/some/very/long/url"}'
```

### Expected Response Format
```json
{
  "shortUrl": "http://localhost:5127/AbC12345",
  "shortCode": "AbC12345", 
  "originalUrl": "https://www.example.com/some/very/long/url"
}
```

## Next Steps After Implementation
1. Test endpoint functionality
2. Verify Base62 encoding works correctly
3. Confirm 8-character truncation
4. Plan v2 improvements (persistence, validation, collision handling)