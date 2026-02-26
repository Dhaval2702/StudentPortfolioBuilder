using Microsoft.AspNetCore.Mvc.Rendering;
using StudentPortfolioBuilder.Models;

namespace StudentPortfolioBuilder.ViewModels;

public class ProfileListViewModel
{
    public ProfileFilterViewModel Filters { get; set; } = new();
    public List<StudentProfile> Profiles { get; set; } = new();
    public List<SelectListItem> FieldsOfStudy { get; set; } = new();
    public List<SelectListItem> JobRoles { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
