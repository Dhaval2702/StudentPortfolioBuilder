using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentPortfolioBuilder.ViewModels;

public class RegistrationViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

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
    [Range(0, 10)]
    [Display(Name = "CGPA")]
    public decimal Cgpa { get; set; }

    [Required]
    [Display(Name = "Profile Image")]
    public IFormFile? Image { get; set; }

    [Required]
    [Display(Name = "CV / Resume")]
    public IFormFile? Resume { get; set; }

    [Required]
    [Display(Name = "Degree Certificate")]
    public IFormFile? DegreeCertificate { get; set; }

    [Required]
    public IFormFile? Marksheet { get; set; }

    [Required]
    public IFormFile? Certifications { get; set; }

    [Display(Name = "Video Introduction (Optional)")]
    public IFormFile? Video { get; set; }

    public List<SelectListItem> FieldsOfStudyOptions { get; set; } = new();
    public List<SelectListItem> JobRolesOptions { get; set; } = new();
}
