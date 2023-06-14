using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Demo.Infrastructure.Entity
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
