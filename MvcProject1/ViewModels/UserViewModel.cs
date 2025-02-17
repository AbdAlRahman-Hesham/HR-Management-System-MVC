using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    public string Id { get; set; }

    [Display(Name = "User Name")]
    public string UserName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public IEnumerable<string>? UserRolesNames { get; set; }

    [Display(Name = "Selected Roles")]
    public List<string> SelectedRoles { get; set; } = new List<string>();
}
