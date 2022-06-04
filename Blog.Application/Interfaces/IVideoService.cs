using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Videos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IVideoService
    {
        Task<int> Add(Video video);

        Task<int> Update(Video video);

        Task<int> Delete(Video video);

        Task<IEnumerable<Video>> GetAll();

        Task<IEnumerable<VideoDto>> GetAllPaging(string keyword);

        Task<Video> GetById(int id);

        void Save();
    }
}
