using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IBlogService
    {
        Task<int> Add(Blogs blog);

        Task<int> Update(Blogs blog);

        Task<int> Delete(Blogs blog);

        Task<IEnumerable<Blogs>> GetAll();

        Task<IEnumerable<BlogDto>> GetNew();

        Task<IEnumerable<BlogDto>> GetAllPaging(int? categoryId, string keyword);

        Task<Blogs> GetById(int id);

        void Save();
    }
}
