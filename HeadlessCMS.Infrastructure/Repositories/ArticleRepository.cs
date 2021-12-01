using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces.Repositories;
using HeadlessCMS.Persistence;

namespace HeadlessCMS.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private ApplicationDbContext context;

        public ArticleRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Article> GetAllArticles()
        {
            return context.Articles.ToList();
        }

        public void AddArticle(Article article)
        {
            context.Articles.Add(article);
        }
    }
}