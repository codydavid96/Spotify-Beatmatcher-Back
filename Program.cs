using BeatMatcher;
using BeatMatcher.Interfaces;
using BeatMatcher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISpotifyAccountService, SpotifyAccountService>();
builder.Services.AddScoped<ISpotifySearchService, SpotifySearchService>();
builder.Services.AddScoped<ITrackSelectService, TrackSelectService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:3000") // Replace with your frontend domain
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

// Add the Spotify client id and secret to the service container
var configuration = builder.Configuration.GetSection("Spotify");
var clientId = configuration.GetValue<string>("ClientId");
var clientSecret = configuration.GetValue<string>("ClientSecret");
builder.Services.AddSingleton(new Secrets { ClientId = clientId, ClientSecret = clientSecret });

var app = builder.Build();

app.UseDeveloperExceptionPage();


app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
