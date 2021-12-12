using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeadlessCMS.WebApi.Controllers
{
    public class TokenController : Controller
    {
        [ApiController]
        [Route("[controller]")]
        public class TokensController : ControllerBase
        {
            private readonly ILogger<TokensController> _logger;
            private readonly IUserAuthService _userAuthService;

            public TokensController(ILogger<TokensController> logger, IUserAuthService userAuthService)
            {
                _logger = logger;
                _userAuthService = userAuthService;
            }

            [HttpPost("accesstoken", Name = "login")]
            public IActionResult Login([FromBody] Authentication auth)
            {
                try
                {
                    return Ok(_userAuthService.Login(auth));
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }

            [Authorize(AuthenticationSchemes = "refresh")]
            [HttpPut("accesstoken", Name = "refresh")]
            public IActionResult Refresh()
            {
                Claim refreshtoken = User.Claims.FirstOrDefault(x => x.Type == "refresh");
                Claim username = User.Claims.FirstOrDefault(x => x.Type == "username");

                try
                {
                    return Ok(_userAuthService.Refresh(username, refreshtoken));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
