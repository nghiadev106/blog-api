using Blog.Application.Interfaces;
using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Infrastructure.Repositories.Interfaces;
using Blog.Shared.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Application
{
    public class BlogService : IBlogService
    {
        private IBlogRepository _blogRepository;
        private IUnitOfWork _unitOfWork;

        public BlogService(IBlogRepository BlogRepository, IUnitOfWork unitOfWork)
        {
            _blogRepository = BlogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Blogs blog)
        {
            blog.CreateDate = DateTime.Now;
            await _blogRepository.Add(blog);
            var result = await _blogRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Blogs blog)
        {
            _blogRepository.Delete(blog);
            var result = await _blogRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Blogs>> GetAll()
        {
            return await _blogRepository.GetAll();
        }

        public async Task<IEnumerable<BlogDto>> GetNew()
        {
            return await _blogRepository.GetNew();
        }

        public async Task<IEnumerable<BlogDto>> GetAllPaging(int? categoryId, string keyword)
        {
            var query = await _blogRepository.GetAllPaging();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.ToLower().IndexOf(keyword) >= 0);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            return query.OrderByDescending(x=>x.CreateDate).ToList();
        }

        public async Task<Blogs> GetById(int id)
        {
            return await _blogRepository.GetById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Blogs request)
        {
            await _blogRepository.Update(request);
            var result = await _blogRepository.Commit();
            return result;
        }
    }
}
