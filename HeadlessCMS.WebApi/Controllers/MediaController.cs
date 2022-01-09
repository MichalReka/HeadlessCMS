﻿using HeadlessCMS.ApplicationCore.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : GenericController<Media>
    {
        public MediasController(ApplicationDbContext context, IUserService userSerivce) : base(context, userSerivce)
        {
        }
    }
}
