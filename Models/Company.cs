using System.ComponentModel.DataAnnotations;

namespace StudentPortfolioBuilder.Models;

public class Company
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? LogoPath { get; set; }

    [MaxLength(120)]
    public string Industry { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public List<JobPosting> JobPostings { get; set; } = new();
    public List<AppUser> HrUsers { get; set; } = new();
}
