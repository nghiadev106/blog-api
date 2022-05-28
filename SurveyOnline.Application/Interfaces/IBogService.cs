using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IBlogService
    {
        Task<int> Add(Blog blog);

        Task<int> Update(Blog blog);

        Task<int> Delete(Blog blog);

        Task<IEnumerable<Blog>> GetAll();

        Task<IEnumerable<BlogDto>> GetNew();

        Task<IEnumerable<BlogDto>> GetAllPaging(int? categoryId, string keyword);

        Task<Blog> GetById(int id);

        void Save();
    }
}
