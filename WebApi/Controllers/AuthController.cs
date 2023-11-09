using Application.Authentications.Commands.Login;
using Application.Authentications.Commands.Register;
using Application.Authentications.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationViewModel>> Register(RegisterCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationViewModel>> Login(LoginCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}