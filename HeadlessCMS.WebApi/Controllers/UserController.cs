using AutoMapper;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Commands;
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
        private readonly IUserService _userService;
        protected IPermissionService _permissionService;

        public UsersController(ApplicationDbContext context, IAuthService userAuthService, IMapper mapper, IPermissionService permissionService, IUserService userService)
        {
            _userAuthService = userAuthService;
            _mapper = mapper;
            _applicationDbContext = context;
            _userDbSet = _applicationDbContext.Set<User>();
            _permissionService = permissionService;
            _userService=userService;
        }

        [HttpGet]
        [EnableQuery]
        public IEnumerable<UserDto> Get()
        {
            var mappedSet = _mapper.Map<UserDto[]>(_applicationDbContext.Users.ToArray());
            return mappedSet;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            try
            {
                _permissionService.ValidateIfAdmin(User);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            userDto.CreatedDate = DateTime.Now;
            userDto.UpdatedDate = DateTime.Now;

            User newUser = _userAuthService.HandleRegister(userDto);

            newUser = _applicationDbContext.Set<User>().Add(newUser).Entity;

            await _applicationDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<UserDto>(newUser));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _permissionService.ValidateIfAdmin(User);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            var entity = _applicationDbContext.Set<User>().First(x => x.Id.ToString() == id);
            if (entity != null)
            {
                _applicationDbContext.Set<User>().Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("ChangePassword")]
        public virtual async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            try
            {
                _permissionService.ValidateIfCanChangeUser(User, command.UserId);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            _userService.UpdateUserPassword(command);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("EditUserData")]
        public virtual async Task<IActionResult> EditUserData([FromBody] UserDto userDto)
        {
            try
            {
                _permissionService.ValidateIfCanChangeUser(User, userDto.Id);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            var user = await _userService.UpdateUserDataAsync(userDto);

            await _applicationDbContext.SaveChangesAsync();
            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}