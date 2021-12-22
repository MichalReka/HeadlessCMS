using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> LoginAsync([FromBody] Authentication auth)
        {
            try
            {
                return Ok(await _userAuthService.LoginAsync(auth));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "refresh")]
        [HttpPut("accesstoken", Name = "refresh")]
        public async Task<IActionResult> RefreshAsync()
        {
            Claim refreshToken = User.Claims.FirstOrDefault(x => x.Type == "refresh");
            Claim username = User.Claims.FirstOrDefault(x => x.Type == "username");

            try
            {
                return Ok(await _userAuthService.RefreshAsync(username, refreshToken));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}