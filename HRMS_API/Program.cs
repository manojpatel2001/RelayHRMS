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
using Microsoft.Extensions.Caching.Memory;
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

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/NotificationRemainderHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// ====================== CORS (FIXED) ======================

var allowedOrigins = new[]
{
    "https://localhost:7165",
    "http://15.235.82.113:81",
    "http://164.52.206.29:81"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
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
builder.Services.AddScoped<EmailJobService>();
builder.Services.AddScoped<ResumeScreeningService>();
builder.Services.AddScoped<SalaryFitmentService>();
builder.Services.AddScoped<OfferApprovalService>();
builder.Services.AddScoped<EmployeeConversionService>();
builder.Services.AddMemoryCache();

// ====================== HANGFIRE ======================

var isHangfireEnabled = builder.Configuration.GetValue<bool>("Hangfire:Enabled");

// 👉 Use separate DB if possible
builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("HRMSConnection"));
});

// 👉 Run Hangfire server only when enabled
if (isHangfireEnabled)
{
    builder.Services.AddHangfireServer();
}

var app = builder.Build();

// ====================== PIPELINE ======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// SignalR
app.MapHub<NotificationRemainderHub>("/NotificationRemainderHub");

// ====================== HANGFIRE DASHBOARD ======================
if (app.Environment.IsDevelopment())
{
    // ✅ Local में open (testing के लिए)
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireNoAuthorizationFilter() }
    });
}
else
{
    // 🔒 Live में secure
    var cache = app.Services.GetRequiredService<IMemoryCache>();

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter(cache) }
    });
}
// ====================== JOB START ======================

if (isHangfireEnabled)
{
    using (var scope = app.Services.CreateScope())
    {
        var emailJobService = scope.ServiceProvider.GetRequiredService<EmailJobService>();
        emailJobService.StartScheduleDailyEmail();
        var emailautoJobService = scope.ServiceProvider.GetRequiredService<AutoJobService>();
        emailautoJobService.StartAutoJobService();
    }
}

app.MapControllers();

app.Run();