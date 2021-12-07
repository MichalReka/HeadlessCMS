using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : GenericController<UserRole>
    {
        public UserRolesController(ApplicationDbContext context) : base(context)
        {
            genericDbSet = context.UserRoles;
        }
    }
}
