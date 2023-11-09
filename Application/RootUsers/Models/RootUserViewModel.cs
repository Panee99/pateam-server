using Application.Common.Models;
using Domain.Entities;
using Mapster;

namespace Application.RootUsers.Models
{
    public class RootUserViewModel : AuditableViewModel, IRegister
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string GithubLink { get; set; } = string.Empty;
        public string LinkedInLink { get; set; } = string.Empty;

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<RootUser, RootUserViewModel>();
        }
    }
}