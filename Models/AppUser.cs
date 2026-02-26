using System.ComponentModel.DataAnnotations;

namespace StudentPortfolioBuilder.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Role { get; set; } = "Student";

    public int? CompanyId { get; set; }
    public Company? Company { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
