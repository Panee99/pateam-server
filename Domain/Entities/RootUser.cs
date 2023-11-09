using Domain.Common;

namespace Domain.Entities
{
    public class RootUser : AuditableEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string GithubLink { get; set; } = string.Empty;
        public string LinkedInLink { get; set; } = string.Empty;
    }
}