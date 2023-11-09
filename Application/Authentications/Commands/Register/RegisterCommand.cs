using Application.Authentications.Models;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentications.Commands.Register
{
    public class RegisterCommand : IRequest<AuthenticationViewModel>, IRegister
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string GithubLink { get; set; } = string.Empty;
        public string LinkedInLink { get; set; } = string.Empty;

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<RegisterCommand, RootUser>();
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationViewModel>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticationViewModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.RegisterUserAsync(request, UserRole.User.ToString());

            return new AuthenticationViewModel() { Token = token };
        }
    }
}