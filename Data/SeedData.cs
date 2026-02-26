using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(ApplicationDbContext context)
    {
        if (context.StudentProfiles.Any())
        {
            return;
        }

        var random = new Random(42);
        var firstNames = new[] { "Aarav", "Ishita", "Priya", "Rohan", "Vikram", "Meera", "Sneha", "Karthik", "Nisha", "Rahul", "Ananya", "Arjun", "Neha", "Kavya", "Varun" };
        var lastNames = new[] { "Sharma", "Patel", "Verma", "Reddy", "Iyer", "Singh", "Mishra", "Nair", "Jain", "Gupta", "Khan", "Das" };
        var cities = new[] { "Bengaluru", "Hyderabad", "Chennai", "Pune", "Mumbai", "Delhi", "Kolkata", "Ahmedabad", "Jaipur", "Lucknow" };
        var colleges = new[] { "National Institute of Technology", "ABC Engineering College", "City Science University", "Modern Technical Institute", "Global College of Engineering", "Pioneer University" };
        var fields = HomeLookup.FieldsOfStudy;
        var jobs = HomeLookup.JobRoles;

        var profiles = new List<StudentProfile>(1000);
        for (var i = 1; i <= 1000; i++)
        {
            var first = firstNames[random.Next(firstNames.Length)];
            var last = lastNames[random.Next(lastNames.Length)];
            var city = cities[random.Next(cities.Length)];
            var college = colleges[random.Next(colleges.Length)];
            var field = fields[random.Next(fields.Count)];
            var role = jobs[random.Next(jobs.Count)];
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
                ImagePath = "/images/default-avatar.svg",
                DegreeCertificatePath = $"/uploads/documents/degree-{i}.pdf",
                MarksheetPath = $"/uploads/documents/marksheet-{i}.pdf",
                CertificationsPath = hasCert ? $"/uploads/documents/cert-{i}.pdf" : null,
                VideoPath = hasVideo ? $"/uploads/videos/intro-{i}.mp4" : null,
                CreatedOn = DateTime.UtcNow.AddDays(-random.Next(1, 360))
            });
        }

        context.StudentProfiles.AddRange(profiles);
        await context.SaveChangesAsync();
    }
}
