var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=JobMaster}/{action=ManageBranch}/{id?}");
    //pattern: "{controller=JobMaster}/{action=ManageDepartment}/{id?}");
    //pattern: "{controller=JobMaster}/{action=ManageDesignation}/{id?}");
    pattern: "{controller=JobMaster}/{action=ManageGrade}/{id?}");

app.Run();
