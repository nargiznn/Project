using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository;
using Service;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Service.Helpers;

var builder = WebApplication.CreateBuilder(args);

// API'de kullanılacak servisler
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Default claims'i temizle

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            ClockSkew = TimeSpan.Zero // Token süresi bittiğinde gecikmeyi ortadan kaldır
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddRepositoryLayer();
builder.Services.AddServiceLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
