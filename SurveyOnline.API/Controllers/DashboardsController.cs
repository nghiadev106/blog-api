using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.EntityFrameworkCore.Models;
using System.Linq;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly SurveyOnlineContext _context;

        public DashboardsController(SurveyOnlineContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult Get()
        {
            var countVideo =  _context.Videos.ToList().Count();
            var countBogs = _context.Blogs.ToList().Count();
            var countUser = _context.AspNetUsers.ToList().Count();
            var countSurvey = _context.Surveys.ToList().Count();
            var countCategory = _context.Categories.ToList().Count();
            var countBlogCategory = _context.BlogCategories.ToList().Count();
            return Ok(new { 
                video=countVideo,
                blog=countBogs,
                user=countUser,
                survey=countSurvey,
                category=countCategory,
                blogCategory=countBlogCategory
            });
        }

    }
}
