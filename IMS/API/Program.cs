using Application;
using Persistence;
using API.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services from other layers.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

// Add FluentValidation validators by scanning the Application assembly.
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

// Add password hasher for User entity
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Configure CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000", "https://localhost:3001")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});

// Configure JWT Authentication.
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = !string.IsNullOrEmpty(builder.Configuration["Jwt:Issuer"]),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["Jwt:Audience"]),
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register JWT token generator.
builder.Services.AddScoped<JwtTokenGenerator>();

// Add Swagger/OpenAPI support.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Smart Inventory Management API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Inventory Management API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();