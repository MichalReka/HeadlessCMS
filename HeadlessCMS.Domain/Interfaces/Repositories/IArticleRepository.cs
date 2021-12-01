using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.Domain.Interfaces.Repositories
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllArticles();
    }
}