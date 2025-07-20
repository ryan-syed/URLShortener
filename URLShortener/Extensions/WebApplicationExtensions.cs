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