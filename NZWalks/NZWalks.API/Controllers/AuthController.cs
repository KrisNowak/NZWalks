using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthController : Controller
    {
        IUserRepository Repository;
        ITokenHandler handler;

        public AuthController(IUserRepository rep, ITokenHandler hand)
        {
            Repository = rep;
            handler = hand;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            var user = await Repository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user != null)
            {
                //Generate JWT

                var token = await handler.CreateTokenAsync(user);

                return Ok(token);
            }

            return BadRequest("Username or password is incorrect.");
        }
    }
}
