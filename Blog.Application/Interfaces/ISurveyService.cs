using Blog.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Questions;
using Blog.Shared.Surveies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface ISurveyService
    {
        Task<int> Add(SurveyCreateRequest request);

        Task<int> Update(Survey category);

        Task<int> Delete(Survey category);

        Task<IEnumerable<Survey>> GetAll();
        Task<IEnumerable<Survey>> GetAllPaging(int? categoryId, string keyword);

        Task<Survey> GetById(int id);

        Task<SurveyVm> GetSurveyDetail(int surveyId);

        Task<int> GetUserStatistics(int surveyId);

        List<QuestionVm> GetRatioStatistics(int surveyId);

        void Save();
    }
}
