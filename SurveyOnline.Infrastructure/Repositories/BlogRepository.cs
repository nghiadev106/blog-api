using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Blogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<IEnumerable<BlogDto>> GetAllPaging()
        {
            var query = await DbContext.Blogs.Select(p => new BlogDto()
            {
                Name = p.Name,
                Id = p.Id,
                Description = p.Description,
                Detail = p.Detail,
                Status = p.Status,
                CreateDate = p.CreateDate,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsNew=p.IsNew,
                IsHot=p.IsHot,
                CreateBy=p.CreateBy,
                Url=p.Url
            }).ToListAsync();
            return query;
        }

        public async Task<IEnumerable<BlogDto>> GetNew()
        {
            var query = await DbContext.Blogs.Where(x=>x.IsNew==true && x.Status==1).Select(p => new BlogDto()
            {
                Name = p.Name,
                Id = p.Id,
                Description = p.Description,
                Detail = p.Detail,
                Status = p.Status,
                CreateDate = p.CreateDate,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsNew = p.IsNew,
                IsHot = p.IsHot,
                CreateBy = p.CreateBy,
                Url = p.Url
            }).Take(10).ToListAsync();
            return query;
        }
    }
}
