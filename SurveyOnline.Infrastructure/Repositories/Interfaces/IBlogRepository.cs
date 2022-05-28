using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Shared.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<IEnumerable<BlogDto>> GetAllPaging();
        Task<IEnumerable<BlogDto>> GetNew();
    }
}
