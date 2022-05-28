using SurveyOnline.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<int> Add(BlogCategory category);

        Task<int> Update(BlogCategory category);

        Task<int> Delete(BlogCategory category);

        Task<IEnumerable<BlogCategory>> GetAll();

        Task<IEnumerable<BlogCategory>> GetAllPaging(string keyword);

        Task<BlogCategory> GetById(int id);

        void Save();
    }
}
