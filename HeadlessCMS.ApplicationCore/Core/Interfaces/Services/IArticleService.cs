using HeadlessCMS.Domain.Dtos;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IArticleService
    {
        public Task CreateArticle(ArticleDto articleDto);
    }
}