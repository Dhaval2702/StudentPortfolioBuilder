using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Index([FromQuery] ProfileFilterViewModel filters, int page = 1, int pageSize = 12)
    {
        if (page < 1) page = 1;

        var query = db.StudentProfiles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Name)) query = query.Where(x => x.Name.Contains(filters.Name));
        if (!string.IsNullOrWhiteSpace(filters.CollegeName)) query = query.Where(x => x.CollegeName.Contains(filters.CollegeName));
        if (!string.IsNullOrWhiteSpace(filters.FieldOfStudy)) query = query.Where(x => x.FieldOfStudy == filters.FieldOfStudy);
        if (!string.IsNullOrWhiteSpace(filters.JobRole)) query = query.Where(x => x.JobRole == filters.JobRole);
        if (filters.OnlyWithVideo) query = query.Where(x => !string.IsNullOrEmpty(x.VideoPath));
        if (filters.OnlyWithCertifications) query = query.Where(x => !string.IsNullOrEmpty(x.CertificationsPath));

        var totalCount = await query.CountAsync();
        var profiles = await query.OrderByDescending(x => x.CreatedOn).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var featuredProfiles = await db.StudentProfiles.AsNoTracking()
            .OrderByDescending(x => x.VideoPath != null && x.VideoPath != "")
            .ThenByDescending(x => x.CertificationsPath != null && x.CertificationsPath != "")
            .ThenByDescending(x => x.CreatedOn)
            .Take(4)
            .ToListAsync();

        var topCompanies = await db.Companies.AsNoTracking()
            .OrderByDescending(x => x.JobPostings.Count)
            .Take(6)
            .ToListAsync();

        var trendingJobs = await db.JobPostings.AsNoTracking()
            .Include(x => x.Company)
            .OrderByDescending(x => x.CreatedOn)
            .Take(6)
            .ToListAsync();

        var categoryCounts = await db.StudentProfiles.AsNoTracking()
            .GroupBy(x => x.FieldOfStudy)
            .Select(g => new { Field = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(8)
            .ToDictionaryAsync(x => x.Field, x => x.Count);

        var model = new HomePageViewModel
        {
            Filters = filters,
            Profiles = profiles,
            FeaturedProfiles = featuredProfiles,
            BrowseProfiles = profiles.Take(4).ToList(),
            FieldsOfStudy = BuildOptions(HomeLookup.FieldsOfStudy),
            JobRoles = BuildOptions(HomeLookup.JobRoles),
            TopCompanies = topCompanies,
            TrendingJobs = trendingJobs,
            CategoryCounts = categoryCounts,
            RecruiterCount = await db.AppUsers.CountAsync(x => x.Role == "HR"),
            JobCount = await db.JobPostings.CountAsync(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile(int id)
    {
        var profile = await db.StudentProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (profile is null) return NotFound();
        return View(profile);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegistrationViewModel
        {
            FieldsOfStudyOptions = BuildOptions(HomeLookup.FieldsOfStudy),
            JobRolesOptions = BuildOptions(HomeLookup.JobRoles)
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistrationViewModel model)
    {
        model.FieldsOfStudyOptions = BuildOptions(HomeLookup.FieldsOfStudy);
        model.JobRolesOptions = BuildOptions(HomeLookup.JobRoles);

        if (!ModelState.IsValid) return View(model);

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

        TempData["SuccessMessage"] = "Profile created successfully.";
        return RedirectToAction(nameof(Profile), new { id = profile.Id });
    }

    [HttpGet] public IActionResult Privacy() => View();
    [HttpGet] public IActionResult Contact() => View();

    private static List<SelectListItem> BuildOptions(IEnumerable<string> source) => source.Select(x => new SelectListItem(x, x)).ToList();

    private async Task<string?> SaveFile(IFormFile? file, string category)
    {
        if (file is null || file.Length == 0) return null;

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
