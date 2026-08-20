using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pickuplay.DTOs;
using Pickuplay.Services;
using Pickuplay.Teams;
using Pickuplay.Teams.Data;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Add DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=E:\\data\\mydb.db")
);

// Controllers
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

// Services
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(x => x.Value?.Errors.Count > 0)
        .Select(x => new
        {
            Field = x.Key,
            Message = x.Value!.Errors.First().ErrorMessage
        });

        return new BadRequestObjectResult(new ApiResponse<object>(
            type : "error",
            message : errors.First().Message,
            data : null
        ));
    };
});

// Load the public key
var publicKeyPem = File.ReadAllText("./resources/certs/public.pem");
var rsa = RSA.Create();
rsa.ImportFromPem(publicKeyPem);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(rsa),

        ValidateIssuer = true,
        ValidIssuer = "self", // must match Spring Boot's issuer claim

        ValidateAudience = false, // set true later if you add audience claims

        ValidateLifetime = true, // checks expiration
        ClockSkew = TimeSpan.Zero // no grace period on expiration
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();