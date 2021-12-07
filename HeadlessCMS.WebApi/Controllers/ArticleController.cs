﻿using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : GenericController<Article>
    {
        public ArticlesController(ApplicationDbContext context) : base(context)
        {
            genericDbSet = context.Articles;
        }
    }
}