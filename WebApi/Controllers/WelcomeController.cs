using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("")]
    public class WelcomeController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Welcome()
        {
            return Ok("Welcome to PATeam");
        }
    }
}