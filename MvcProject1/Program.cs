using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mvc.DAL.Data;
using MvcProject1.DAL.Models;
using MvcProject1.PL.Extentions;
using MvcProject1.PL.Helpers;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;

// Add services to the container.
Services.AddControllersWithViews();

Services.AddDbContext<AppDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MVC1"))
    );
Services.AddAppServices();
Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));

Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    //SignIn
    config.SignIn.RequireConfirmedEmail = false;
    config.SignIn.RequireConfirmedPhoneNumber = false;

    //User
    config.User.RequireUniqueEmail = true;
    //config.User.AllowedUserNameCharacters = "ABC";

    //Password
    config.Password.RequireDigit = true;
    config.Password.RequireLowercase = true;
    config.Password.RequireUppercase = true;
    //config.Password.RequireNonAlphanumeric

    //Lockout
    config.Lockout.AllowedForNewUsers = true;
    config.Lockout.MaxFailedAccessAttempts = 3;
    //config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);



})
    .AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Auth/SignIn";
});

Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
Services.Configure<TwilioAppSetting>(builder.Configuration.GetSection("TwilioAppSetting"));
Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
    .AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
{
    IConfiguration configuration = builder.Configuration.GetSection("ExternalLogin:Google");
    o.ClientId = configuration["ClientId"] ?? throw new InvalidOperationException("Google ClientId is not configured.");
    o.ClientSecret = configuration["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 
app.Run();
