using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Data;
using StudentPortfolioBuilder.Models;
using StudentPortfolioBuilder.ViewModels;

namespace StudentPortfolioBuilder.Controllers;

public class AccountController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await db.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user is null || !PasswordHelper.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new UserRegistrationViewModel
        {
            CompanyOptions = db.Companies.AsNoTracking().Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegistrationViewModel model)
    {
        model.CompanyOptions = db.Companies.AsNoTracking().Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        if (!ModelState.IsValid) return View(model);

        if (await db.AppUsers.AnyAsync(x => x.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already registered.");
            return View(model);
        }

        if (model.Role == "Admin")
        {
            ModelState.AddModelError(nameof(model.Role), "Admin users can only be created by system seed.");
            return View(model);
        }

        var user = new AppUser
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = PasswordHelper.Hash(model.Password),
            Role = model.Role,
            CompanyId = model.Role == "HR" ? model.CompanyId : null
        };

        db.AppUsers.Add(user);
        await db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Registration successful. Please login.";
        return RedirectToAction(nameof(Login));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "Student";
        var model = new DashboardViewModel
        {
            UserName = User.Identity?.Name ?? "User",
            Role = role,
            Companies = await db.Companies.AsNoTracking().Take(10).ToListAsync(),
            Jobs = await db.JobPostings.AsNoTracking().Include(x => x.Company).OrderByDescending(x => x.CreatedOn).Take(10).ToListAsync(),
            HrUsers = await db.AppUsers.AsNoTracking().Where(x => x.Role == "HR").Take(10).ToListAsync()
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
