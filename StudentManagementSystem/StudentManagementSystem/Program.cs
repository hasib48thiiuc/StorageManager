using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Entities;
using StudentManagementSystem.Models.Domain;
using StudentManagementSystem.Services;
using StudentManagementSystem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddDbContext<SMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
 builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebModule());

});

builder.Services
.AddIdentity<ApplicationUser, ApplicationRole>()
.AddEntityFrameworkStores<SMSDbContext>()
.AddUserManager<ApplicationUserManager>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
       {
           options.LoginPath = new PathString("/Account/Login");
           options.AccessDeniedPath = new PathString("/Account/Login");
           options.LogoutPath = new PathString("/Account/Logout");
           options.Cookie.Name = "FirstDemoPortal.Identity";
           options.SlidingExpiration = true;
           options.ExpireTimeSpan = TimeSpan.FromHours(1);


       });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});


builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();,in idd custom identity it will be cut down..

app.Run();
