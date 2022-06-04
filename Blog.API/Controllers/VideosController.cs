using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Helpers;
using Blog.API.Services;
using Blog.Application.Interfaces;
using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Videos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;

        public VideosController(
            IVideoService videoService,
            IMapper mapper)
        {
            _videoService = videoService;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _videoService.GetAll();
            var videosVm = _mapper.Map<IEnumerable<VideoDto>>(videos);
            return Ok(videosVm);
        }


        [HttpGet("paging")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllPaging(string keyword, int page, int pageSize = 20)
        {
            var videos = await _videoService.GetAllPaging(keyword);
            var videosVm = _mapper.Map<IEnumerable<VideoDto>>(videos);
            var responseData = videosVm.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                Items = responseData,
                Page = page,
                TotalItems = videosVm.Count(),
                PageSize = pageSize
            });
        }

        [HttpGet("{videoId}")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetById(int videoId)
        {
            var video = await _videoService.GetById(videoId);
            if (video == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {videoId}"));
            var videoVm = _mapper.Map<VideoDto>(video);
            return Ok(videoVm);
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([FromBody] VideoCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Dữ liệu không hợp lệ"));
            }
            var video = _mapper.Map<Video>(request);
            if (video.Image != null)
            {
                var arrData = video.Image.Split(';');
                if (arrData.Length == 3)
                {
                    var savePath = $"{arrData[0]}";
                    video.Image = $"{savePath}";
                    var fileService = new FileService();
                    fileService.SaveFileFromBase64String(savePath, arrData[2]);
                }
            }
            var result = await _videoService.Add(video);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Thêm mới không thành công"));
            }
        }

        [HttpPut("{videoId}")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Update([FromRoute] int videoId, [FromBody] VideoUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Dữ liệu không hợp lệ"));
            }

            var video = await _videoService.GetById(videoId);
            if (video == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {videoId}"));
            if (request.Image != null)
            {
                var arrData = request.Image.Split(';');
                if (arrData.Length == 3)
                {
                    var savePath = $"{arrData[0]}";
                    video.Image = $"{savePath}";
                    var fileService = new FileService();
                    fileService.SaveFileFromBase64String(savePath, arrData[2]);
                }
            }
            video.Name = request.Name;
            video.Description = request.Description;
            video.CategoryId = request.CategoryId;
            video.Detail = request.Detail;
            video.Link = request.Link;
            video.Status = request.Status;
            video.Url = request.Url;
            var result = await _videoService.Update(video);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật không thành công"));
            }
        }

        [HttpDelete("{videoId}")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int videoId)
        {
            var video = await _videoService.GetById(videoId);
            if (video == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {videoId}"));

            var result = await _videoService.Delete(video);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Xóa không thành công"));
            }
        }
    }
}
