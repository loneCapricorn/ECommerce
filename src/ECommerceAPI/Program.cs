using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ECommerceAPI.Data;
using ECommerceAPI.Services;
using ECommerceAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// determine environment
bool isDevelopment = builder.Environment.IsDevelopment();

// load .env files
Env.Load(isDevelopment ? ".env.local" : ".env");

// add env vars into Config
builder.Configuration.AddEnvironmentVariables();

// read environment variables
var connectionString = builder.Configuration["ECOMMERCE_CONNECTION_STRING"];
var jwtKey = builder.Configuration["JWT_KEY"];
var jwtIssuer = builder.Configuration["JWT_ISSUER"];
var jwtAudience = builder.Configuration["JWT_AUDIENCE"];

// register DbContext
builder.Services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// ensure the roles are generated
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    DatabaseSeeder.SeedRoles(db);
}

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
