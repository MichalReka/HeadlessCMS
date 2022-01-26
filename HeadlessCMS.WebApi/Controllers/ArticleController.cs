using AutoMapper;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using HeadlessCMS.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : GenericController<Article, ArticleDto>
    {
        IArticleService _articleService;

        public ArticlesController(ApplicationDbContext context,
            IBaseEntityRepository baseEntityRepository,
            IArticleService articleService,
            IMapper mapper)
            : base(context, baseEntityRepository, mapper)
        {
            _articleService = articleService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public override async Task Post([FromBody] ArticleDto value)
        {
            await _articleService.CreateArticle(value);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}