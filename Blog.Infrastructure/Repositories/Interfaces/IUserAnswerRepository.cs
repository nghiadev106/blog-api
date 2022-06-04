using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface IUserAnswerRepository : IRepository<UsersAnswer>
    {
        Task<UsersAnswer> CheckUserAnswer(string userId, int surveyId);
        Task<UsersAnswer> CheckAnswer(string userId, int surveyId, int questionId, int? answerId);
        Task<List<UsersAnswer>> GetUserAnswerBySurveyId(int surveyId);    
    }
}
