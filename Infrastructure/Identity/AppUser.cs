using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public RootUser RootUser { get; set; } = null!;
    }
}