using Hangfire;
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

// ====================== SERVICES ======================

// DbContext
builder.Services.AddDbContext<HRMSDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSConnection"));
});

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity
builder.Services.AddIdentityCore<HRMSUserIdentity>()
    .AddRoles<HRMSRoleIdentity>()
    .AddEntityFrameworkStores<HRMSDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// JWT
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
            var path = context.HttpContext.Request.Path;

            // ? FIX: Case match
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/NotificationRemainderHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// CORS
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

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// HttpClient
builder.Services.AddHttpClient();

// Custom Services
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<AutoJobService>();
// ====================== HANGFIRE ======================

builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("HRMSConnection"));
});

// ? IMPORTANT: Worker
builder.Services.AddHangfireServer();

var app = builder.Build();

// ====================== PIPELINE ======================

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

// SignalR
app.MapHub<NotificationRemainderHub>("/NotificationRemainderHub");

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// ====================== JOB START ======================

using (var scope = app.Services.CreateScope())
{
    //var emailJobService = scope.ServiceProvider.GetRequiredService<EmailJobService>();
    //emailJobService.StartScheduleDailyJobEmail();

    var autoJobService = scope.ServiceProvider.GetRequiredService<AutoJobService>();
    autoJobService.StartAutoJobService();
}

app.MapControllers();

app.Run();