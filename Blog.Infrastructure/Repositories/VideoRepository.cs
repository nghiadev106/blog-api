using Microsoft.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Infrastructure.Repositories.Interfaces;
using Blog.Shared.Videos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories
{
    public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public VideoRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<IEnumerable<VideoDto>> GetAllPaging()
        {
            var query = await DbContext.Videos.Select(p => new VideoDto()
            {
                Name = p.Name,
                Id=p.Id,
                Description=p.Description,
                Detail=p.Detail,
                Status=p.Status,
                CreateDate=p.CreateDate,
                Link=p.Link,
                CategoryId=p.CategoryId,
                Url=p.Url,
                Image=p.Image,
                IsHot=p.IsHot,
                IsNew=p.IsNew,
                CreateBy=p.CreateBy,
                CategoryName=p.Category.Name
            }).ToListAsync();
            return query;
        }
    }
}
