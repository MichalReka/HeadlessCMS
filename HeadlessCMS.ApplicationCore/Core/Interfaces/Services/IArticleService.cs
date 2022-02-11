using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IArticleService
    {
        public Task<Article> CreateArticle(ArticleDto articleDto);
        Task<Article> UpdateArticle(ArticleDto value);
    }
}