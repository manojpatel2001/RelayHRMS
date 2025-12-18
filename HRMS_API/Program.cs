using DinkToPdf;
using DinkToPdf.Contracts;
using Hangfire;
using Hangfire.MemoryStorage;
using HRMS_API.Midleware;
using HRMS_API.NotificationService.HubService;
using HRMS_API.Services;
using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure DbContext

builder.Services.AddDbContext<HRMSDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSConnection"));
});

// Configure Identity with custom UserRole
builder.Services.AddIdentityCore<HRMSUserIdentity>(options =>
{
    // Optional: Password rules, Lockout, etc.
})
.AddRoles<HRMSRoleIdentity>()
.AddEntityFrameworkStores<HRMSDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for SignalR hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationRemainderHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

var allowedOrigins = new[]
{
    "https://localhost:7165",
    "http://15.235.82.113:81",
    "http://164.52.206.29:81"
};

// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
       policy.SetIsOriginAllowed(_ => true)  // allow any origin
              .AllowAnyMethod()
              .AllowAnyHeader()
               .AllowCredentials()); // needed for SignalR
});

builder.Services.AddSignalR();
// Add Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register FileUploadService
builder.Services.AddScoped<FileUploadService>();

// Add Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseMemoryStorage(); // Use SQL Server in production
});

builder.Services.AddHangfireServer();
//Email Service
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EmailJobService>();
builder.Services.AddScoped<AutoJobService>();


var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<DbUserContextMiddleware>();

app.MapHub<NotificationRemainderHub>("/NotificationRemainderHub");
app.UseHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var emailJobService = scope.ServiceProvider.GetRequiredService<EmailJobService>();
    emailJobService.StartScheduleDailyEmail();
    var autoJobService = scope.ServiceProvider.GetRequiredService<AutoJobService>();
    autoJobService.StartAutoJobService();
}

app.MapControllers();

app.Run();
