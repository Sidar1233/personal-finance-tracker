using Microsoft.EntityFrameworkCore;
using Payzo.Data;
using Payzo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQLite database - data saved to payzo.db file
builder.Services.AddDbContext<PayzoDb>(options =>
    options.UseSqlite("Data Source=payzo.db"));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FinanceService>();

// Cookie authentication
builder.Services.AddAuthentication("PayzoCookie")
    .AddCookie("PayzoCookie", options =>
    {
        options.LoginPath         = "/Account/Login";
        options.LogoutPath        = "/Account/Logout";
        options.AccessDeniedPath  = "/Account/Login";
        options.Cookie.Name       = "PayzoSession";
        options.ExpireTimeSpan    = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",      p => p.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("SuperAdminOnly", p => p.RequireRole("SuperAdmin"));
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Create DB and seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PayzoDb>();
    db.Database.EnsureCreated();   // creates payzo.db if not exists
    DbSeeder.Seed(db);             // seed only if empty
}

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
