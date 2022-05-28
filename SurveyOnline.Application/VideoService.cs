using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class VideoService : IVideoService
    {
        private IVideoRepository _videoRepository;
        private IUnitOfWork _unitOfWork;

        public VideoService(IVideoRepository VideoRepository, IUnitOfWork unitOfWork)
        {
            _videoRepository = VideoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Video video)
        {
            video.CreateDate = DateTime.Now;
            await _videoRepository.Add(video);
            var result = await _videoRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Video video)
        {
            _videoRepository.Delete(video);
            var result = await _videoRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await _videoRepository.GetAll();
        }

        public async Task<IEnumerable<VideoDto>> GetAllPaging(string keyword)
        {
            var query = await _videoRepository.GetAllPaging();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.ToLower().IndexOf(keyword) >= 0);

            return query;
        }

        public async Task<Video> GetById(int id)
        {
            return await _videoRepository.GetById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Video request)
        {
            await _videoRepository.Update(request);
            var result = await _videoRepository.Commit();
            return result;
        }
    }
}
