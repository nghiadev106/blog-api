using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Shared.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface IBlogRepository : IRepository<Blogs>
    {
        Task<IEnumerable<BlogDto>> GetAllPaging();
        Task<IEnumerable<BlogDto>> GetNew();
    }
}
