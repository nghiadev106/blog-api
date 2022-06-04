using Blog.EntityFrameworkCore.Models;
using Blog.Shared.Answers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IAnswerService
    {
        Task<int> Add(AnswerCreateRequest request);

        Task<int> Update(Answer request);

        Task<int> Delete(Answer answer);

        Task<IEnumerable<Answer>> GetAll();

        Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId);

        Task<Answer> GetById(int id);

        void Save();
    }
}
