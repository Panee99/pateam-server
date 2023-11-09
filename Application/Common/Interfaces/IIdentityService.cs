using Application.Authentications.Commands.Register;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, UserRole role);
        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<bool> AuthorizeAsync(Guid userId, string policyName);

        // Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
        //
        // Task<Result> DeleteUserAsync(Guid userId);
        Task<string> RegisterUserAsync(RegisterCommand request, string role);

        Task<string> LoginAsync(string username, string password);
    }
}