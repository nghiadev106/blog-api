using Microsoft.AspNetCore.Mvc;
using Blog.EntityFrameworkCore.Models;
using System.Linq;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly BlogContext _context;

        public ClientController(BlogContext context)
        {
            _context = context;
        }

        [HttpGet("home")]
        public IActionResult GetHome()
        {
            var lstTopBlogs = _context.Blogs.Take(5).OrderByDescending(x => x.CreateDate).ToList();
            var topVideo = _context.Videos.Where(x=>x.IsHot==true).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            var topNewVideo= _context.Videos.Where(x => x.IsNew == true).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            var videos = _context.Videos.ToList();
            if (topVideo != null)
            {
                videos= videos.Where(x =>  x.Id != topVideo.Id).ToList();
            }
            videos = videos.OrderBy(x => x.CreateDate).Take(3).ToList();

            var topNewFeed = _context.Blogs.Where(x => x.IsNew == true).FirstOrDefault();
            var blogs = _context.Blogs.Where(x => x.Id != topNewFeed.Id).OrderByDescending(x => x.CreateDate).Take(10).ToList();

            return Ok(new
            {
                lstTopBlogs = lstTopBlogs,
                topVideo = topVideo,
                topNewVideo = topNewVideo,
                videos = videos,
                topNewFeed = topNewFeed,
                blogs = blogs
            });
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetDetail(int id)
        {
            var detail = _context.Blogs.Where(x => x.Id == id).SingleOrDefault();
            var lstTopBlogs = _context.Blogs.Where(x => x.Id !=detail.Id).OrderByDescending(x => x.CreateDate).Take(5).ToList();
            var topReated= _context.Blogs.Where(x => x.Id != id).FirstOrDefault();
            var related= _context.Blogs.Where(x => x.Id != detail.Id && x.Id!=topReated.Id && x.CategoryId==detail.CategoryId).OrderByDescending(x => x.CreateDate).Take(4).ToList();
            var lstByCategory= _context.Blogs.Where(x => x.Id != detail.Id && x.CategoryId == detail.CategoryId).OrderBy(x=>x.CreateDate).OrderByDescending(x => x.CreateDate).Take(12).ToList();

            return Ok(new
            {
                detail = detail,
                lstTopBlogs = lstTopBlogs,
                topReated = topReated,
                related = related,
                lstByCategory = lstByCategory
            });
        }

        [HttpGet("category/{id}")]
        public IActionResult GetByCategory(int id,int pageSize)
        {
            var bigBlog = _context.Blogs.Where(x => x.CategoryId == id).FirstOrDefault();
            var blogs = _context.Blogs.Where(x=>x.CategoryId==id).OrderByDescending(x => x.CreateDate).ToList();

            if (bigBlog != null)
            {
                blogs = blogs.Where(x => x.Id != bigBlog.Id).ToList();
            }

            return Ok(new
            {
                bigBlog = bigBlog,
                blogs = blogs.Take(pageSize).OrderByDescending(x => x.CreateDate).ToList(),
                totalCount=_context.Blogs.Where(x => x.CategoryId == id).ToList().Count(),
                //categoryName = _context.BlogCategories.Where(x => x.Id == id).SingleOrDefault().Name
            }) ;
        }

        [HttpGet("video")]
        public IActionResult GetVideo(int pageSize)
        {
            var bigVideo = _context.Videos.Where(x => x.IsHot==true&x.IsNew==true).FirstOrDefault();
            var topVideo = _context.Videos.Where(x => x.IsHot == true & x.IsNew == true&& x.Id != bigVideo.Id).FirstOrDefault();
            var videos= _context.Videos.OrderByDescending(x => x.CreateDate).ToList();
            if (bigVideo != null)
            {
                videos = videos.Where(x => x.Id != bigVideo.Id).ToList();
            }
            if (topVideo != null)
            {
                videos = videos.Where(x => x.Id != topVideo.Id).ToList();
            }
            return Ok(new
            {
                bigVideo = bigVideo,
                topVideo = topVideo,
                videos = videos.Take(pageSize).ToList(),
                totalCount = _context.Videos.ToList().Count()
            });
        }

        [HttpGet("video/detail/{id}")]
        public IActionResult GetVideoDetail(int id)
        {
            var video = _context.Videos.Where(x => x.Id==id).SingleOrDefault();
            return Ok(video);
        }
    }
}
