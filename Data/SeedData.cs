using Microsoft.EntityFrameworkCore;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(ApplicationDbContext context)
    {
        await SeedUsersCompaniesAndJobs(context);
        await SeedStudentProfiles(context);
    }

    private static async Task SeedUsersCompaniesAndJobs(ApplicationDbContext context)
    {
        if (!await context.Companies.AnyAsync())
        {
            var companies = new List<Company>
            {
                new() { Name = "TCS", Industry = "IT Services", LogoPath = "/images/default-avatar.svg" },
                new() { Name = "Infosys", Industry = "IT Services", LogoPath = "/images/default-avatar.svg" },
                new() { Name = "Wipro", Industry = "Technology", LogoPath = "/images/default-avatar.svg" },
                new() { Name = "HCL", Industry = "Engineering", LogoPath = "/images/default-avatar.svg" }
            };
            context.Companies.AddRange(companies);
            await context.SaveChangesAsync();
        }

        if (!await context.JobPostings.AnyAsync())
        {
            var companies = await context.Companies.ToListAsync();
            var random = new Random(7);
            for (var i = 1; i <= 20; i++)
            {
                var company = companies[random.Next(companies.Count)];
                context.JobPostings.Add(new JobPosting
                {
                    Title = $"Graduate {HomeLookup.JobRoles[random.Next(HomeLookup.JobRoles.Count)]}",
                    Field = HomeLookup.FieldsOfStudy[random.Next(HomeLookup.FieldsOfStudy.Count)],
                    Location = random.Next(2) == 0 ? "Bengaluru" : "Hyderabad",
                    Openings = random.Next(2, 16),
                    CompanyId = company.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                });
            }
            await context.SaveChangesAsync();
        }

        if (!await context.AppUsers.AnyAsync())
        {
            var firstCompany = await context.Companies.FirstAsync();
            context.AppUsers.AddRange(
                new AppUser
                {
                    FullName = "System Admin",
                    Email = "admin@studentportfolio.local",
                    PasswordHash = PasswordHelper.Hash("Admin@123"),
                    Role = "Admin"
                },
                new AppUser
                {
                    FullName = "Default HR",
                    Email = "hr@studentportfolio.local",
                    PasswordHash = PasswordHelper.Hash("Hr@123"),
                    Role = "HR",
                    CompanyId = firstCompany.Id,
                    ProfilePhotoPath = "/images/default-avatar.svg"
                },
                new AppUser
                {
                    FullName = "Demo Student",
                    Email = "student@studentportfolio.local",
                    PasswordHash = PasswordHelper.Hash("Student@123"),
                    Role = "Student",
                    ProfilePhotoPath = "/images/default-avatar.svg",
                    CvPath = "/uploads/documents/student-cv.pdf"
                });

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedStudentProfiles(ApplicationDbContext context)
    {
        var existingCount = await context.StudentProfiles.CountAsync();
        if (existingCount == 100) return;

        if (existingCount > 0)
        {
            context.StudentProfiles.RemoveRange(context.StudentProfiles);
            await context.SaveChangesAsync();
        }

        var random = new Random(42);
        var firstNames = new[] { "Aarav", "Ishita", "Priya", "Rohan", "Vikram", "Meera", "Sneha", "Karthik", "Nisha", "Rahul", "Ananya", "Arjun", "Neha", "Kavya", "Varun" };
        var lastNames = new[] { "Sharma", "Patel", "Verma", "Reddy", "Iyer", "Singh", "Mishra", "Nair", "Jain", "Gupta", "Khan", "Das" };
        var cities = new[] { "Bengaluru", "Hyderabad", "Chennai", "Pune", "Mumbai", "Delhi", "Kolkata", "Ahmedabad", "Jaipur", "Lucknow" };
        var colleges = new[] { "National Institute of Technology", "ABC Engineering College", "City Science University", "Modern Technical Institute", "Global College of Engineering", "Pioneer University" };

        var profiles = new List<StudentProfile>(100);
        for (var i = 1; i <= 100; i++)
        {
            var first = firstNames[random.Next(firstNames.Length)];
            var last = lastNames[random.Next(lastNames.Length)];
            var city = cities[random.Next(cities.Length)];
            var college = colleges[random.Next(colleges.Length)];
            var field = HomeLookup.FieldsOfStudy[random.Next(HomeLookup.FieldsOfStudy.Count)];
            var role = HomeLookup.JobRoles[random.Next(HomeLookup.JobRoles.Count)];
            var hasVideo = random.NextDouble() > 0.65;
            var hasCert = random.NextDouble() > 0.25;

            profiles.Add(new StudentProfile
            {
                Name = $"{first} {last}",
                Email = $"{first.ToLower()}.{last.ToLower()}{i}@mail.com",
                Address = $"{random.Next(1, 450)}, {city}, India",
                CollegeName = college,
                FieldOfStudy = field,
                JobRole = role,
                Cgpa = Math.Round((decimal)(random.NextDouble() * 3 + 7), 2),
                ImagePath = $"https://i.pravatar.cc/300?img={random.Next(1, 70)}",
                ResumePath = $"/uploads/documents/resume-{i}.pdf",
                DegreeCertificatePath = $"/uploads/documents/degree-{i}.pdf",
                MarksheetPath = $"/uploads/documents/marksheet-{i}.pdf",
                CertificationsPath = hasCert ? $"/uploads/documents/cert-{i}.pdf" : null,
                VideoPath = hasVideo ? $"/uploads/videos/intro-{i}.mp4" : null,
                CreatedOn = DateTime.UtcNow.AddDays(-random.Next(1, 180))
            });
        }

        context.StudentProfiles.AddRange(profiles);
        await context.SaveChangesAsync();
    }
}
