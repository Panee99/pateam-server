using Application.Authentications.Models;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Authentications.Commands.Login
{
    public class LoginCommand : IRequest<AuthenticationViewModel>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationViewModel>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthenticationViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _identityService.LoginAsync(request.Username, request.Password);
            
            return new AuthenticationViewModel() { Token = token };
        }
    }
}