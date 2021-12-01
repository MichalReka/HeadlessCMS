using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HeadlessCMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        private IArticleRepository _articleRepository;

        public ArticleController(ILogger<ArticleController> logger, IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Article> GetAll()
        {
            return _articleRepository.GetAllArticles();
        }
    }
}