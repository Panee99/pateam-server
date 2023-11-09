using Application.Authentications.Commands.Register;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppDbContext _dbContext;
        private readonly JwtService _jwtService;

        public IdentityService(UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService,
            JwtService jwtService, IAppDbContext dbContext)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _jwtService = jwtService;
            _dbContext = dbContext;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await _userManager.Users.FirstAsync(x => x.RootUser.Id == userId);

            return user.UserName;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, UserRole role)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.RootUser.Id == userId);

            return user != null && await _userManager.IsInRoleAsync(user, role.ToString());
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            if (!Enum.IsDefined(typeof(UserRole), role))
            {
                return false;
            }

            return await IsInRoleAsync(userId, Enum.Parse<UserRole>(role));
        }

        public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.RootUser.Id == userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<string> RegisterUserAsync(RegisterCommand request, string role)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null) throw new BadRequestException("Username existed");

            // Create transaction for create AppUser and RootUser
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create RootUser
                var rootUser = request.Adapt<RootUser>();
                _dbContext.RootUsers.Add(rootUser);
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                // Create AppUser
                var newUser = new AppUser()
                {
                    UserName = request.Username,
                    RootUser = rootUser
                };

                await _userManager.CreateAsync(newUser, request.Password);
                // await _userManager.AddToRoleAsync(newUser, role); // todo: uncomment after seed roles

                await transaction.CommitAsync();

                var authUser = new AuthUser() { Id = rootUser.Id, Roles = new List<string>() { role } };

                return _jwtService.GenerateToken(authUser);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userManager.Users.Include(x => x.RootUser)
                .FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null) throw new UnauthorizedException("Username or password incorrect");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect) throw new UnauthorizedException("Username or password incorrect");

            var roles = await _userManager.GetRolesAsync(user);

            return _jwtService.GenerateToken(new AuthUser() { Id = user.RootUser.Id, Roles = roles });
        }
    }
}