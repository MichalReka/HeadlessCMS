using HeadlessCMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HeadlessCMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Article> GetAll()
        {
            return 
            .ToArray();
        }
    }
}