using Microsoft.AspNetCore.Identity;

namespace MvcProject1.DAL.Models;

public class AppUser:IdentityUser
{
    public bool IsAgree { get; set; }
}
