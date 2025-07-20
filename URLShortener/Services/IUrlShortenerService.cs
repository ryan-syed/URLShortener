using URLShortener.Models.V1;

namespace URLShortener.Services;

public interface IUrlShortenerService
{
    ShortenUrlResponse ShortenUrl(ShortenUrlRequest request);
}