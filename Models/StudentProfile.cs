using System.ComponentModel.DataAnnotations;

namespace StudentPortfolioBuilder.Models;

public class StudentProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

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

    [Display(Name = "Profile Image")]
    public string? ImagePath { get; set; }

    [Display(Name = "Degree Certificate")]
    public string? DegreeCertificatePath { get; set; }

    [Display(Name = "Marksheet")]
    public string? MarksheetPath { get; set; }

    [Display(Name = "Certifications")]
    public string? CertificationsPath { get; set; }

    [Display(Name = "Video Introduction")]
    public string? VideoPath { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
