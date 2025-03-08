using Microsoft.AspNetCore.Identity;

namespace WebAPI_Labs.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
