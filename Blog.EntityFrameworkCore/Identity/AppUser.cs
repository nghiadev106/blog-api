using Microsoft.AspNetCore.Identity;

namespace Blog.EntityFrameworkCore.Identity
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
