using IntelligenceAgencyManagementSystem;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database connection
builder.Services.AddDbContext<IaDbContext>(option => option.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
));

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
    name: "Departments",
    pattern: "Departments/{action=Index}/{id?}",
    new { controller = "Departments" });

app.MapControllerRoute(
    name: "Operations",
    pattern: "Operations/{action=Index}/{id?}",
    new { controller = "Operations" });


app.MapControllerRoute(
    name: "Archive",
    pattern: "Archive/{action=Index}/{id?}",
    new { controller = "Archive" });

app.MapControllerRoute(
    name: "Workers",
    pattern: "Workers/{action=Index}/{id?}",
    new { controller = "Workers" });

app.MapControllerRoute(
    name: "MilitaryFiles",
    pattern: "MilitaryFiles/{action=Index}/{id?}",
    new { controller = "MilitaryFiles" });

app.MapControllerRoute(
    name: "CoverRoles",
    pattern: "CoverRoles/{action=Index}/{id?}",
    new { controller = "CoverRoles" });

app.MapControllerRoute(
    name: "default",
    pattern: "/{action}",
    new {controller = "Home", action = "Index"});

app.Run();