using System.Reflection.Metadata;
using System.Text;
using BudgetTracker.Application;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Extensions;
using BudgetTracker.Infrastructure.BackgroundServices;
using BudgetTracker.Infrastructure.Database;
using BudgetTracker.Middlewares;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationByAssemblyReference).Assembly,includeInternalTypes:true);
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfireConnection"));
    }));

builder.Services.AddHangfireServer();
builder.Services.AddTransient<ExceptionMiddleware>();
JwtSettings jwtSettings = new JwtSettings();
builder.Services.AddSingleton(jwtSettings);

builder.Configuration.Bind(nameof(JwtSettings),jwtSettings);
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.SaveToken = true;
  options.RequireHttpsMetadata = true;
  options.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),
    ValidIssuer = jwtSettings.Issuer,
    ValidAudience = jwtSettings.Audience,
    ClockSkew = TimeSpan.Zero,
  };
});
builder.Services.AddDomainServices();
builder.Services.AddRepositories();
builder.Services.AddAuthServices();
builder.Services.AddCrudServices();
builder.Services.AddControllers();
builder.Services.AddBackgroundServices();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.SupportNonNullableReferenceTypes();
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
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
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();