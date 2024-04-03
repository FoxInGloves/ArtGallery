using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Data.Implementations;
using ArtGallery.Models.Describers;
using ArtGallery.Models.Managers;
using ArtGallery.Models.Services;
using ArtGallery.Models.Structs.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz" +
                                                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
                                                 "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                                 "0123456789" +
                                                 "-._@";
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddSignInManager<ApplicationSignInManager<ApplicationUser>>()
    .AddErrorDescriber<RussianErrorDescriber>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ConverterToDto, ConverterToDto>();
builder.Services.AddScoped<ImageManipulation, ImageManipulation>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Arts}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();