using Microsoft.AspNetCore.Identity;

namespace Web.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string Lastname { get; set; } = string.Empty;
        public virtual ICollection<Job>? Jobs { get; set; }
    }
}
