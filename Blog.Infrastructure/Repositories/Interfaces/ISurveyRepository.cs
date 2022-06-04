using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Shared.Questions;
using Blog.Shared.Surveies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        Task<SurveyVm> GetSurveyDetail(int surveyId);
        Task<int> GetUserStatistics(int surveyId);
        List<QuestionVm> GetRatioStatistics(int surveyId);
    }
}
