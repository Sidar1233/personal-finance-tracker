using Payzo.Data;
using Payzo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register services as singletons (in-memory store)
builder.Services.AddSingleton<PayzoDb>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<FinanceService>();

// Cookie authentication
builder.Services.AddAuthentication("PayzoCookie")
    .AddCookie("PayzoCookie", options =>
    {
        options.LoginPath    = "/Account/Login";
        options.LogoutPath   = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.Name  = "PayzoSession";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",      p => p.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("SuperAdminOnly", p => p.RequireRole("SuperAdmin"));
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
