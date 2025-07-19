var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello from URL Shortener!");

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

app.Run();

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

record ShortenUrlRequest(string Url);

record ShortenUrlResponse
{
    public required string ShortUrl { get; init; }
    public required string ShortCode { get; init; }
    public required string OriginalUrl { get; init; }
}