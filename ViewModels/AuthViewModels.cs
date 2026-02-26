using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class UserRegistrationViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Profile Photo")]
    public IFormFile? ProfilePhoto { get; set; }

    [Display(Name = "CV / Resume")]
    public IFormFile? CvFile { get; set; }

    public string Role { get; set; } = "Student";
    public int? CompanyId { get; set; }
    public List<SelectListItem> CompanyOptions { get; set; } = new();
}

public class DashboardViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? UserPhotoPath { get; set; }
    public List<Company> Companies { get; set; } = new();
    public List<JobPosting> Jobs { get; set; } = new();
    public List<AppUser> HrUsers { get; set; } = new();
}

public class AdminOnboardingViewModel
{
    [Required] public string CompanyName { get; set; } = string.Empty;
    [Required] public string Industry { get; set; } = string.Empty;
    [Required] public string HrName { get; set; } = string.Empty;
    [Required, EmailAddress] public string HrEmail { get; set; } = string.Empty;
    [Required] public string HrPassword { get; set; } = string.Empty;
    [Required] public string JobTitle { get; set; } = string.Empty;
    [Required] public string JobField { get; set; } = string.Empty;
    [Required] public string JobLocation { get; set; } = string.Empty;
    [Range(1, 1000)] public int Openings { get; set; } = 5;
}
