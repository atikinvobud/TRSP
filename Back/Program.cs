using System.Text;
using System.Text.Json;
using Back.Models;
using Back.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<PicrureService>();
builder.Services.AddScoped<BetService>();
//настройка контроллеров
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
       options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // camelcase     

        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // Опционально: Сделать вывод более удобным (без лишних пробелов и строк)
        options.JsonSerializerOptions.WriteIndented = false;
    });

//подключение сваггера часть 1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GigaAuction",
        Version = "v1"
    });
});

//подключение к бд postgres
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));

//подключение к mongodb
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<MongoDbContext>();

//настройка JWT
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();


//подключение сваггера часть 2
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

});
app.UseHttpsRedirection();

//настройка авторизации
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");
//настройка контроллеров
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

