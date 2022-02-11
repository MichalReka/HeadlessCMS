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
            IMapper mapper,
            IPermissionService permissionService)
            : base(context, baseEntityRepository, mapper, permissionService)
        {
            _articleService = articleService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public override async Task<ArticleDto> Post([FromBody] ArticleDto value)
        {
            var article = await _articleService.CreateArticle(value);
            await applicationDbContext.SaveChangesAsync();
            return _mapper.Map<ArticleDto>(article);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public override async Task<IActionResult> PutAsync([FromBody] ArticleDto value)
        {
            try
            {
                _permissionService.ValidateOwnership<Article>(User, value.Id.ToString());
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            var article = _mapper.Map<Article>(value);
            article = await _articleService.UpdateArticle(value);
            await applicationDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ArticleDto>(article));
        }
    }
}