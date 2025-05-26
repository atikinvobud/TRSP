using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Back.Models;
using Back.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// КОРС
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4221")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// JWT config section
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Сервисы
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<PicrureService>();
builder.Services.AddScoped<BetService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton<WebSocketService>();

// Controllers + JSON
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = false;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GigaAuction",
        Version = "v1"
    });
});

// PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));

// MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var mongoConnectionString = configuration.GetConnectionString("MongoDb");
    return new MongoClient(mongoConnectionString);
});
builder.Services.AddSingleton<MongoDbContext>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("JwtSettings:SecretKey") ?? "default_secret_key"
            )),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };

        // Получение токена из cookie или query
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                else
                {
                    context.Token = context.Request.Cookies["jwt"];
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseHttpsRedirection();

// Auth + WebSockets
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets(); 
app.UseMiddleware<WebSocketMiddleware>(); 

// CORS
app.UseCors("AllowAll");

// Контроллеры
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
