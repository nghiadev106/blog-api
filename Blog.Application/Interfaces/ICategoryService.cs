using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<int> Add(CategoryCreateRequest Category);

        Task<int> Update(Category category);

        Task<int> Delete(Category category);

        Task<IEnumerable<Category>> GetAll();

        Task<IEnumerable<Category>> GetAllPaging(string keyword);

        Task<Category> GetById(int id);

        void Save();
    }
}
