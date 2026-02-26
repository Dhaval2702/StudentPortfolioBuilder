using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=studentportfolio.db"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();

    try
    {
        db.Database.ExecuteSqlRaw("SELECT 1 FROM Companies LIMIT 1;");
        db.Database.ExecuteSqlRaw("SELECT ProfilePhotoPath, CvPath FROM AppUsers LIMIT 1;");
        db.Database.ExecuteSqlRaw("SELECT 1 FROM JobPostings LIMIT 1;");
        db.Database.ExecuteSqlRaw("SELECT Cgpa, ResumePath FROM StudentProfiles LIMIT 1;");
    }
    catch (SqliteException)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    await SeedData.EnsureSeededAsync(db);
}

app.Run();
