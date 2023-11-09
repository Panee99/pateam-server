namespace Infrastructure.Identity
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}