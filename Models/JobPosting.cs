using System.ComponentModel.DataAnnotations;

namespace StudentPortfolioBuilder.Models;

public class JobPosting
{
    public int Id { get; set; }

    [Required]
    [MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Field { get; set; } = string.Empty;

    [Required]
    [MaxLength(180)]
    public string Location { get; set; } = string.Empty;

    public int Openings { get; set; } = 1;

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
