using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using TravelAPI.Data;
using TravelAPI.Models;
using TravelAPI.Profiles;
using TravelAPI.Services;

var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddDbContext<TravelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173" // Vite
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

#region JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT ERROR: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };
    });
#endregion

#region Authorization
builder.Services.AddAuthorization();
#endregion

#region Services
builder.Services.AddScoped<JwtService>();
builder.Services.AddHttpClient();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile));
#endregion

#region Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Travel API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

#region HttpClient
builder.Services.AddHttpClient<RestCountriesService>(client =>
{
    client.BaseAddress = new Uri("https://restcountries.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
#endregion

var app = builder.Build();

#region Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); // 🔥 CORS burada

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion


#region Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
    db.Database.Migrate();
}
#endregion

#region Seed Logic
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TravelDbContext>();

    if (!context.Users.Any())
    {
        var admin = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("539421"),
            Role = "Admin"
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}
#endregion

app.Run();
