using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Shared.Videos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<VideoDto>> GetAllPaging();
    }
}
