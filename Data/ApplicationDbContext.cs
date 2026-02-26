using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentProfile>()
            .HasIndex(x => new { x.FieldOfStudy, x.JobRole });

        modelBuilder.Entity<StudentProfile>()
            .Property(x => x.CreatedOn)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
