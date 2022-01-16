using AutoMapper;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using HeadlessCMS.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : GenericController<Media, MediaDto>
    {
        public MediasController(ApplicationDbContext context,
            IBaseEntityRepository baseEntityRepository,
            IMapper mapper)
            : base(context, baseEntityRepository, mapper)
        {
        }
    }
}
