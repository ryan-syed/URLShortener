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