using Microsoft.AspNetCore.Identity;

namespace Task4.Models
{
    public class User : IdentityUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsBlocked { get; set; }
    }
}
