using AutoMapper;
using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Answers;
using Blog.Shared.BlogCategories;
using Blog.Shared.Blogs;
using Blog.Shared.Categories;
using Blog.Shared.Questions;
using Blog.Shared.Surveies;
using Blog.Shared.Videos;

namespace Blog.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Category, CategoryDto>();
            CreateMap<Survey, SurveyDto>();
            CreateMap<Question, QuestionDto>();
            CreateMap<Answer, AnswerDto>();
            CreateMap<BlogCategory, BlogCategoryDto>();
            CreateMap<BlogCategoryCreateRequest, BlogCategory>();
            CreateMap<Blogs, BlogDto>();
            CreateMap<BlogCreateRequest, Blogs>();
            CreateMap<Video, VideoDto>();
            CreateMap<VideoCreateRequest, Video>();
        }
    }
}
