using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Helpers;
using Blog.API.Services;
using Blog.Application.Interfaces;
using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Blogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BlogsController(
            IBlogService blogService,
            IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogService.GetAll();
            var blogsVm = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            return Ok(blogsVm);
        }

        [Authorize(Policy = "RequireLoggedIn")]
        [HttpGet("new")]
        public async Task<IActionResult> GetNew()
        {
            var blogs = await _blogService.GetNew();
            var blogsVm = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            return Ok(blogsVm);
        }

        [HttpGet("paging")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetAllPaging(int? categoryId, string keyword, int page, int pageSize = 20)
        {
            var blogs = await _blogService.GetAllPaging(categoryId,keyword);
            var blogsVm = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            var responseData = blogsVm.OrderBy(x => x.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                Items = responseData,
                Page = page,
                TotalItems = blogsVm.Count(),
                PageSize = pageSize
            });
        }

        [HttpGet("{blogId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetById(int blogId)
        {
            var blog = await _blogService.GetById(blogId);
            if (blog == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {blogId}"));
            var blogVm = _mapper.Map<BlogDto>(blog);
            return Ok(blogVm);
        }

        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> Create([FromBody] BlogCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Dữ liệu không hợp lệ"));
            }
            var blog = _mapper.Map<Blogs>(request);
            if (blog.Image != null)
            {
                var arrData = blog.Image.Split(';');
                if (arrData.Length == 3)
                {
                    var savePath = $"{arrData[0]}";
                    blog.Image = $"{savePath}";
                    var fileService = new FileService();
                    fileService.SaveFileFromBase64String(savePath, arrData[2]);
                }
            }
            var result = await _blogService.Add(blog);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Thêm mới không thành công"));
            }
        }

        [HttpPut("{blogId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> Update([FromRoute] int blogId, [FromBody] BlogUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Dữ liệu không hợp lệ"));
            }

            var blog = await _blogService.GetById(blogId);
            if (blog == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {blogId}"));

            if (request.Image != null)
            {
                var arrData = request.Image.Split(';');
                if (arrData.Length == 3)
                {
                    var savePath = $"{arrData[0]}";
                    blog.Image = $"{savePath}";
                    var fileService = new FileService();
                    fileService.SaveFileFromBase64String(savePath, arrData[2]);
                }
            }
            blog.Name = request.Name;
            blog.Description = request.Description;
            blog.CategoryId = request.CategoryId;
            blog.Detail = request.Detail;
            blog.Status = request.Status;
            blog.Url = request.Url;
            blog.IsHot = request.IsHot;
            blog.IsNew = request.IsNew;
            var result = await _blogService.Update(blog);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật không thành công"));
            }
        }

        [HttpDelete("{blogId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> Delete(int blogId)
        {
            var blog = await _blogService.GetById(blogId);
            if (blog == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {blogId}"));

            var result = await _blogService.Delete(blog);

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
