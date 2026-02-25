using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.Controllers;

public class HomeController : Controller
{
    private static readonly List<StudentProfile> Profiles = new();

    private static readonly List<string> FieldsOfStudy =
    [
        "Computer Science",
        "Information Technology",
        "Electrical Engineering",
        "Mechanical Engineering",
        "Civil Engineering",
        "Electronics & Communication",
        "Data Science",
        "Business Administration",
        "Commerce",
        "Biotechnology"
    ];

    private static readonly List<string> JobRoles =
    [
        "Python Developer",
        "C# Developer",
        "Java Developer",
        "Front-End Developer",
        "Full Stack Developer",
        "Data Analyst",
        "Machine Learning Engineer",
        "Cloud Engineer",
        "DevOps Engineer",
        "UI/UX Designer",
        "Electrical Design Engineer",
        "Mechanical Design Engineer",
        "Site Engineer",
        "Quality Assurance Engineer",
        "Business Analyst"
    ];

    [HttpGet]
    public IActionResult Index()
    {
        var model = BuildViewModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(HomePageViewModel model)
    {
        PopulateDropdowns(model);

        if (!ModelState.IsValid)
        {
            model.Profiles = Profiles.OrderByDescending(p => p.CreatedOn).ToList();
            return View(model);
        }

        var profile = new StudentProfile
        {
            Name = model.Input.Name,
            Address = model.Input.Address,
            CollegeName = model.Input.CollegeName,
            FieldOfStudy = model.Input.FieldOfStudy,
            JobRole = model.Input.JobRole,
            ImagePath = await SaveFile(model.Input.Image, "images"),
            DegreeCertificatePath = await SaveFile(model.Input.DegreeCertificate, "documents"),
            MarksheetPath = await SaveFile(model.Input.Marksheet, "documents"),
            CertificationsPath = await SaveFile(model.Input.Certifications, "documents"),
            VideoPath = await SaveFile(model.Input.Video, "videos")
        };

        Profiles.Add(profile);

        TempData["SuccessMessage"] = "Portfolio created successfully!";
        return RedirectToAction(nameof(Index));
    }

    private HomePageViewModel BuildViewModel()
    {
        var vm = new HomePageViewModel
        {
            Profiles = Profiles.OrderByDescending(p => p.CreatedOn).ToList()
        };

        PopulateDropdowns(vm);
        return vm;
    }

    private static void PopulateDropdowns(HomePageViewModel model)
    {
        model.FieldsOfStudy = FieldsOfStudy
            .Select(x => new SelectListItem(x, x))
            .ToList();

        model.JobRoles = JobRoles
            .Select(x => new SelectListItem(x, x))
            .ToList();
    }

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
