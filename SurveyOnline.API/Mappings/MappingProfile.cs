using AutoMapper;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.Answers;
using SurveyOnline.Shared.BlogCategories;
using SurveyOnline.Shared.Blogs;
using SurveyOnline.Shared.Categories;
using SurveyOnline.Shared.Questions;
using SurveyOnline.Shared.Surveies;
using SurveyOnline.Shared.Videos;

namespace SurveyOnline.API.Mappings
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
            CreateMap<Blog, BlogDto>();
            CreateMap<BlogCreateRequest, Blog>();
            CreateMap<Video, VideoDto>();
            CreateMap<VideoCreateRequest, Video>();
        }
    }
}
