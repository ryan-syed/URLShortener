# Unit Testing Implementation Plan

## Overview
Create comprehensive unit tests for the v1 URL shortening APIs to ensure reliability, maintainability, and proper functionality validation.

## Test Strategy

### Components to Test
1. **Base62Encoder** - Utility component for encoding
2. **RandomUrlShortenerService** - Core business logic
3. **API Endpoints** - Integration testing via WebApplicationFactory
4. **Models** - Data contract validation

### Testing Framework
- **xUnit**: Primary testing framework (standard for .NET)
- **Moq**: Mocking framework for dependencies
- **Microsoft.AspNetCore.Mvc.Testing**: For integration tests
- **FluentAssertions**: For readable assertions

## Planned Test Structure

```
URLShortener.Tests/
├── URLShortener.Tests.csproj
├── Utilities/
│   └── Base62EncoderTests.cs
├── Services/
│   └── V1/
│       └── RandomUrlShortenerServiceTests.cs
├── Integration/
│   └── UrlShorteningEndpointTests.cs
└── TestHelpers/
    └── ConfigurationHelper.cs
```

## Detailed Test Plans

### 1. Base62Encoder Tests
**File**: `Utilities/Base62EncoderTests.cs`
**Purpose**: Test encoding logic with various inputs

**Test Cases**:
```csharp
[Theory]
[InlineData(0.0, "0")]
[InlineData(0.5, ExpectedPattern = "Valid Base62 string")]
[InlineData(1.0, ExpectedPattern = "Valid Base62 string")]
public void Encode_WithValidInput_ReturnsBase62String(double input, string expected)

[Fact]
public void Encode_WithVariousInputs_ReturnsValidBase62Characters()

[Fact]
public void Encode_WithSameInput_ReturnsConsistentOutput()

[Fact]
public void Encode_OutputLength_IsReasonable()
```

### 2. RandomUrlShortenerService Tests
**File**: `Services/V1/RandomUrlShortenerServiceTests.cs`
**Purpose**: Test business logic with mocked dependencies

**Test Cases**:
```csharp
[Fact]
public void ShortenUrl_WithValidRequest_ReturnsValidResponse()

[Fact]
public void ShortenUrl_UsesConfiguredBaseUrl()

[Fact]
public void ShortenUrl_FallsBackToDefaultBaseUrl_WhenConfigNotSet()

[Fact]
public void ShortenUrl_ShortCodeIsMaximum8Characters()

[Fact]
public void ShortenUrl_PreservesOriginalUrl()

[Fact]
public void ShortenUrl_CallsEncoderCorrectly()

[Theory]
[InlineData("https://example.com")]
[InlineData("https://very-long-domain-name.com/with/multiple/path/segments")]
public void ShortenUrl_WithVariousUrls_ReturnsCorrectFormat(string inputUrl)
```

### 3. Integration Tests
**File**: `Integration/UrlShorteningEndpointTests.cs`
**Purpose**: End-to-end API testing

**Test Cases**:
```csharp
[Fact]
public async Task POST_ApiV1Urls_ReturnsOk_WithValidPayload()

[Fact]
public async Task POST_ApiV1Urls_ReturnsCorrectResponseFormat()

[Fact]
public async Task POST_ApiV1Urls_HandlesInvalidJson()

[Fact]
public async Task GET_Root_ReturnsWelcomeMessage()

[Theory]
[InlineData("https://example.com")]
[InlineData("http://test.com/path")]
public async Task POST_ApiV1Urls_WithValidUrls_ReturnsShortened(string url)
```

## Implementation Details

### Project Setup
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="FluentAssertions" Version="6.12.1" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\URLShortener\URLShortener.csproj" />
  </ItemGroup>
</Project>
```

### Mocking Strategy
- Mock `IBase62Encoder` for service tests
- Mock `IConfiguration` for configuration testing
- Use `WebApplicationFactory<Program>` for integration tests

### Test Data Patterns
- Use `[Theory]` with `[InlineData]` for parameterized tests
- Create helper methods for common test data
- Use builders for complex object creation

## Benefits

### Code Quality
- Catch regressions early
- Validate business logic correctness
- Ensure proper error handling

### Maintainability
- Safe refactoring with test coverage
- Documentation through test cases
- Clear component behavior specifications

### Reliability
- Validate edge cases
- Test error conditions
- Ensure consistent behavior

## Test Execution Commands

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "ClassName=Base62EncoderTests"

# Run tests with detailed output
dotnet test --verbosity normal
```

## Migration Steps

1. Create URLShortener.Tests project
2. Add necessary NuGet packages
3. Implement Base62Encoder tests
4. Implement RandomUrlShortenerService tests
5. Add integration tests for API endpoints
6. Set up test helpers and utilities
7. Verify all tests pass
8. Add to CI/CD pipeline (future)

## Test Coverage Goals

- **Unit Tests**: 90%+ coverage on business logic
- **Integration Tests**: All API endpoints
- **Edge Cases**: Null inputs, empty strings, boundary conditions
- **Error Scenarios**: Invalid configurations, mock failures