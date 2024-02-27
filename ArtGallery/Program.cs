using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models.Describers;
using ArtGallery.Models.Structs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz" +
                                                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
                                                 "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                                 "0123456789" +
                                                 "-._@+";
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<RussianErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

/*builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddErrorDescriber<RussianErrorDescriber>()
   .AddEntityFrameworkStores<ApplicationDbContext>();*/

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

/*app.MapControllerRoute(
    name: "IdentityArea",
    pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}");*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Arts}/{action=Index}/{id?}");

/*app.MapAreaControllerRoute(
    name: "IdentityArea",
    areaName: "Identity",
    pattern: "Identity/{controller=Account}/{action=Index}/{id?}");*/

app.MapRazorPages();

app.Run();