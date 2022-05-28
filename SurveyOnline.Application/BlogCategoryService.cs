using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private IBlogCategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;

        public BlogCategoryService(IBlogCategoryRepository BlogCategoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = BlogCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(BlogCategory category)
        {
            await _categoryRepository.Add(category);
            var result = await _categoryRepository.Commit();
            return result;
        }

        public async Task<int> Delete(BlogCategory category)
        {
            _categoryRepository.Delete(category);
            var result = await _categoryRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<BlogCategory>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<IEnumerable<BlogCategory>> GetAllPaging(string keyword)
        {
            var query = await _categoryRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.ToLower().IndexOf(keyword) >= 0);

            return query;
        }

        public async Task<BlogCategory> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(BlogCategory request)
        {
            await _categoryRepository.Update(request);
            var result = await _categoryRepository.Commit();
            return result;
        }
    }
}
