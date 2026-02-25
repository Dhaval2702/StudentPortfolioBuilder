using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Data;
using StudentPortfolioBuilder.Models;
using StudentPortfolioBuilder.ViewModels;

namespace StudentPortfolioBuilder.Controllers;

public class HomeController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProfileFilterViewModel filters)
    {
        var query = db.StudentProfiles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            query = query.Where(x => x.Name.Contains(filters.Name));
        }

        if (!string.IsNullOrWhiteSpace(filters.CollegeName))
        {
            query = query.Where(x => x.CollegeName.Contains(filters.CollegeName));
        }

        if (!string.IsNullOrWhiteSpace(filters.FieldOfStudy))
        {
            query = query.Where(x => x.FieldOfStudy == filters.FieldOfStudy);
        }

        if (!string.IsNullOrWhiteSpace(filters.JobRole))
        {
            query = query.Where(x => x.JobRole == filters.JobRole);
        }

        if (filters.OnlyWithVideo)
        {
            query = query.Where(x => x.VideoPath != null && x.VideoPath != "");
        }

        if (filters.OnlyWithCertifications)
        {
            query = query.Where(x => x.CertificationsPath != null && x.CertificationsPath != "");
        }

        var model = new HomePageViewModel
        {
            Filters = filters,
            Profiles = await query.OrderByDescending(x => x.CreatedOn).Take(200).ToListAsync(),
            FieldsOfStudy = BuildOptions(HomeLookup.FieldsOfStudy),
            JobRoles = BuildOptions(HomeLookup.JobRoles)
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegistrationViewModel
        {
            FieldsOfStudyOptions = BuildOptions(HomeLookup.FieldsOfStudy),
            JobRolesOptions = BuildOptions(HomeLookup.JobRoles)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationViewModel model)
    {
        model.FieldsOfStudyOptions = BuildOptions(HomeLookup.FieldsOfStudy);
        model.JobRolesOptions = BuildOptions(HomeLookup.JobRoles);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var profile = new StudentProfile
        {
            Name = model.Name,
            Email = model.Email,
            Address = model.Address,
            CollegeName = model.CollegeName,
            FieldOfStudy = model.FieldOfStudy,
            JobRole = model.JobRole,
            ImagePath = await SaveFile(model.Image, "images") ?? "/images/default-avatar.svg",
            DegreeCertificatePath = await SaveFile(model.DegreeCertificate, "documents"),
            MarksheetPath = await SaveFile(model.Marksheet, "documents"),
            CertificationsPath = await SaveFile(model.Certifications, "documents"),
            VideoPath = await SaveFile(model.Video, "videos")
        };

        db.StudentProfiles.Add(profile);
        await db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Registration completed and profile highlighted on landing page.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Login()
    {
        TempData["InfoMessage"] = "Login flow UI placeholder added. Integrate ASP.NET Identity for full auth.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult GoogleLogin()
    {
        TempData["InfoMessage"] = "Google login button is added in UI. OAuth integration can be enabled next.";
        return RedirectToAction(nameof(Index));
    }

    private static List<SelectListItem> BuildOptions(IEnumerable<string> source) =>
        source.Select(x => new SelectListItem(x, x)).ToList();

    private async Task<string?> SaveFile(IFormFile? file, string category)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", category);
        Directory.CreateDirectory(uploadsRoot);

        var extension = Path.GetExtension(file.FileName);
        var uniqueName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadsRoot, uniqueName);

        await using var stream = System.IO.File.Create(fullPath);
        await file.CopyToAsync(stream);

        return $"/uploads/{category}/{uniqueName}";
    }
}
