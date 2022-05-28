using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Shared.Videos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<VideoDto>> GetAllPaging();
    }
}
