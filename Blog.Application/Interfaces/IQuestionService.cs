using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<int> Add(QuestionCreateRequest Question);

        Task<int> Update(Question category);

        Task<int> Delete(Question category);

        Task<IEnumerable<Question>> GetAll();

        Task<Question> GetById(int id);

        Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId);

        Task<IEnumerable<QuestionType>> GetAllQuestionTypes();

        void Save();
    }
}
