using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class BlogCategoryRepository : RepositoryBase<BlogCategory>, IBlogCategoryRepository
    {
        public BlogCategoryRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
    }
}
