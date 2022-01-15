using AutoMapper;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _userAuthService;
        private readonly IMapper _mapper;
        private readonly DbSet<User> _userDbSet;
        private readonly ApplicationDbContext _applicationDbContext;

        public UsersController(ApplicationDbContext context, IAuthService userAuthService, IMapper mapper)
        {
            _userAuthService = userAuthService;
            _mapper = mapper;
            _applicationDbContext = context;
            _userDbSet = _applicationDbContext.Set<User>();
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<UserDto>> Get()
        {
            return await _mapper.ProjectTo<UserDto>(_applicationDbContext.Users).ToListAsync();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [HttpGet("CurrentUser")]
        public IActionResult GetCurrentUser()
        {
            var id = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            if (id != null)
            {
                var user = _applicationDbContext.Users.First(x => x.Id == Guid.Parse(id));
                return Ok(_mapper.Map<UserDto>(user));

            }
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme), ]
        [HttpPost]
        public async Task Post([FromBody] UserDto userDto)
        {
            userDto.CreatedDate = DateTime.Now;
            userDto.UpdatedDate = DateTime.Now;

            User newUser = _userAuthService.HandleRegister(userDto);

            _applicationDbContext.Set<User>().Add(newUser);

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
