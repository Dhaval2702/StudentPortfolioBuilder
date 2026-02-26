using Microsoft.AspNetCore.Mvc.Rendering;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.ViewModels;

public class HomePageViewModel
{
    public ProfileFilterViewModel Filters { get; set; } = new();
    public List<StudentProfile> Profiles { get; set; } = new();
    public List<StudentProfile> FeaturedProfiles { get; set; } = new();
    public List<StudentProfile> BrowseProfiles { get; set; } = new();
    public List<SelectListItem> FieldsOfStudy { get; set; } = new();
    public List<SelectListItem> JobRoles { get; set; } = new();
    public List<Company> TopCompanies { get; set; } = new();
    public List<JobPosting> TrendingJobs { get; set; } = new();
    public Dictionary<string, int> CategoryCounts { get; set; } = new();
    public int RecruiterCount { get; set; }
    public int JobCount { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

public class ProfileFilterViewModel
{
    public string? Name { get; set; }
    public string? CollegeName { get; set; }
    public string? FieldOfStudy { get; set; }
    public string? JobRole { get; set; }
    public bool OnlyWithVideo { get; set; }
    public bool OnlyWithCertifications { get; set; }
}
