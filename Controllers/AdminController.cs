using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Data;
using StudentPortfolioBuilder.Models;
using StudentPortfolioBuilder.ViewModels;

namespace StudentPortfolioBuilder.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public IActionResult Index() => View(new AdminOnboardingViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AdminOnboardingViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var company = await db.Companies.FirstOrDefaultAsync(x => x.Name == model.CompanyName);
        if (company is null)
        {
            company = new Company { Name = model.CompanyName, Industry = model.Industry, LogoPath = "/images/default-avatar.svg" };
            db.Companies.Add(company);
            await db.SaveChangesAsync();
        }

        if (!await db.AppUsers.AnyAsync(x => x.Email == model.HrEmail))
        {
            db.AppUsers.Add(new AppUser
            {
                FullName = model.HrName,
                Email = model.HrEmail,
                PasswordHash = PasswordHelper.Hash(model.HrPassword),
                Role = "HR",
                CompanyId = company.Id
            });
        }

        db.JobPostings.Add(new JobPosting
        {
            Title = model.JobTitle,
            Field = model.JobField,
            Location = model.JobLocation,
            Openings = model.Openings,
            CompanyId = company.Id
        });

        await db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Company, HR and job onboarding completed.";
        return RedirectToAction(nameof(Index));
    }
}
