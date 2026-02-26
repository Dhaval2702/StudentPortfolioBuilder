using System.ComponentModel.DataAnnotations;

namespace StudentPortfolioBuilder.Models;

public class StudentProfile
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string CollegeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string JobRole { get; set; } = string.Empty;

    public string? ImagePath { get; set; }
    public string? DegreeCertificatePath { get; set; }
    public string? MarksheetPath { get; set; }
    public string? CertificationsPath { get; set; }
    public string? VideoPath { get; set; }

    [EmailAddress]
    [MaxLength(180)]
    public string? Email { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public bool HasVideo => !string.IsNullOrWhiteSpace(VideoPath);
    public bool HasCertifications => !string.IsNullOrWhiteSpace(CertificationsPath);
}
