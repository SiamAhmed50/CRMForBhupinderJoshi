using CRM.Data.DbContext;
using CRM.Data.Entities;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.Repositories;
using CRM.Service.Services;
using CRM.Service.Services.Repositories;
using CRM.UI.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configure identity options as needed
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ProjectDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();





builder.Services.AddHttpClient();
// Configure Identity options
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login"; 
        options.LogoutPath = "/Identity/Account/Logout"; //Added
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";//Added
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

    
});
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

//builder.Services.AddHostedService<LicenseExpirationService>();
// Configure JWT settings
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Add Razor Pages services
builder.Services.AddRazorPages();

// Register ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddHttpContextAccessor(); // Allows accessing HTTP context (cookies, etc.)
builder.Services.AddTransient<CustomAuthorizationHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{

    //client.BaseAddress = new Uri("https://localhost:44300"); // Replace with your API base URL
    client.BaseAddress = new Uri("https://api-monitor.robobotics.eu"); // Replace with your API base URL
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This serves static files, if needed

app.UseRouting();
app.UseCors("AllowAll"); // Configure CORS before authorization and endpoints

app.UseAuthentication();
app.UseAuthorization();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.None,
};
app.UseCookiePolicy(cookiePolicyOptions);
app.UseMiddleware<JwtAuthenticationMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();

    // This ensures any unmatched route falls back to /Index
    endpoints.MapFallbackToPage("/Index");
});

app.Run();