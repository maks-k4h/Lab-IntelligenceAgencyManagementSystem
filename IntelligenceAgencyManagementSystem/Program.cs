using IntelligenceAgencyManagementSystem;
using IntelligenceAgencyManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database connections
builder.Services.AddDbContext<IaDbContext>(option => option.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
));

builder.Services.AddDbContext<IdentityContext>(option => option.UseMySql(
    builder.Configuration.GetConnectionString("IdentityConnection") ?? string.Empty,
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("IdentityConnection") ?? string.Empty)
));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

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

app.UseAuthentication();

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
    name: "HumanResources",
    pattern: "HumanResources/{action=Index}/{id?}",
    new { controller = "HumanResources" });

app.MapControllerRoute(
    name: "Workers",
    pattern: "Workers/{action=Index}/{id?}",
    new { controller = "Workers" });

app.MapControllerRoute(
    name: "WorkingInDepartments",
    pattern: "WorkingInDepartments/{action}/{id?}",
    new {controller = "WorkingInDepartment"}
);

app.MapControllerRoute(
    name: "Roles",
    pattern: "Roles/{action=Index}/{id?}",
    new { controller = "Roles" });

app.MapControllerRoute(
    name: "MilitaryFiles",
    pattern: "MilitaryFiles/{action=Index}/{id?}",
    new { controller = "MilitaryFiles" });

app.MapControllerRoute(
    name: "CoverRoles",
    pattern: "CoverRoles/{action=Index}/{id?}",
    new { controller = "CoverRoles" });

app.MapControllerRoute(
    name: "WorkersToOperations",
    pattern: "WorkersToOperations/{action=Index}/{id?}",
    new { controller = "WorkersToOperations" });

app.MapControllerRoute(
    name: "OperationsManagement",
    pattern: "OperationsManagement/{action=Index}/{id?}",
    new {controller = "OperationsManagement"});

app.MapControllerRoute(
    name: "Tasks",
    pattern: "Tasks/{action=Index}/{id?}",
    new {controller = "Tasks"});

app.MapControllerRoute(
    name: "TaskStatuses",
    pattern: "TaskStatuses/{action=Index}/{id?}",
    new {controller = "TaskStatuses"});

app.MapControllerRoute(
    name: "default",
    pattern: "/{action}",
    new {controller = "Home", action = "Index"});

app.Run();