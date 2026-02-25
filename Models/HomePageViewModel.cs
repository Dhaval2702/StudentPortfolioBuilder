using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentPortfolioBuilder.Models;

public class HomePageViewModel
{
    public StudentProfileInput Input { get; set; } = new();
    public List<StudentProfile> Profiles { get; set; } = new();
    public List<SelectListItem> FieldsOfStudy { get; set; } = new();
    public List<SelectListItem> JobRoles { get; set; } = new();
}

public class StudentProfileInput
{
    [Required]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Display(Name = "College Name")]
    public string CollegeName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Field of Study")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Preferred Job Role")]
    public string JobRole { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Profile Image")]
    public IFormFile? Image { get; set; }

    [Required]
    [Display(Name = "Degree Certificate")]
    public IFormFile? DegreeCertificate { get; set; }

    [Required]
    public IFormFile? Marksheet { get; set; }

    [Required]
    public IFormFile? Certifications { get; set; }

    [Display(Name = "Video Introduction (Optional)")]
    public IFormFile? Video { get; set; }
}
