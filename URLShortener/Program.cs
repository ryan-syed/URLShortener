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