using Application;
using Persistence;
using API.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Identity; // ?? ???? ?????
using Domain.Models; // ?? ???? ?????

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services from other layers.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

// Add FluentValidation validators by scanning the Application assembly.
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

// ?? ???? ???? ?? ?????? ??????? ?????
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Configure CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    });
});

// Configure JWT Authentication.
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key is missing in configuration.");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Register JWT token generator.
builder.Services.AddSingleton<JwtTokenGenerator>();

// Add Swagger/OpenAPI support.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();