using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Shared.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId);

        Task<IEnumerable<QuestionType>> GetAllQuestionTypes();
    }
}
