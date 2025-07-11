using CRM.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CRM.Data.Entities;
using CRM.Service.Helpers;
using CRM.Service.Interfaces.Repositories;
using CRM.Service.Interfaces.UnitOfWork;
using CRM.Service.Services.Repositories;
using CRM.Service.Services.UnitOfWork;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// 1. DB Context
builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});

// 2. Repositories and Services
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientTaskRepository, ClientTaskService>();
builder.Services.AddScoped<IMachineRepository, MachineService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserMenuRepository, UserMenuService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<LicenseStatusUpdaterService>();
builder.Services.AddHostedService<ScheduleWorker>();
builder.Services.AddHostedService<ApiResponseProcessorService>();


// 3. Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // tell Identity which claim type to use for roles
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
})
    .AddEntityFrameworkStores<ProjectDbContext>()
    .AddDefaultTokenProviders();

// 4. JWT Authentication
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        NameClaimType = ClaimTypes.NameIdentifier
    };
});



// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// 6. Swagger Setup
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] followed by your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// 7. Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
    c.RoutePrefix = ""; // So Swagger loads on root
});

app.Run();


