using Microsoft.AspNetCore.Identity;

namespace Lumia.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
