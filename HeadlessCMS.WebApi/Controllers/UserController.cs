using AutoMapper;
using AutoMapper.QueryableExtensions;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IMapper _mapper;
        private readonly DbSet<User> _userDbSet;
        private readonly ApplicationDbContext _applicationDbContext;

        public UsersController(ApplicationDbContext context, IUserAuthService userAuthService, IMapper mapper)
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
