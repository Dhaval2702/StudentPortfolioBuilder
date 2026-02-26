using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobPosting> JobPostings => Set<JobPosting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentProfile>()
            .HasIndex(x => new { x.FieldOfStudy, x.JobRole });

        modelBuilder.Entity<StudentProfile>()
            .Property(x => x.CreatedOn)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<AppUser>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasOne(x => x.Company)
            .WithMany(x => x.HrUsers)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<JobPosting>()
            .HasOne(x => x.Company)
            .WithMany(x => x.JobPostings)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
