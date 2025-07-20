namespace URLShortener.Models.V1;

public record ShortenUrlResponse
{
    public required string ShortUrl { get; init; }
    public required string ShortCode { get; init; }
    public required string OriginalUrl { get; init; }
}