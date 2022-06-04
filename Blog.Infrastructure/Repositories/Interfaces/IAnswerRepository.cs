using Blog.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId);
    }
}
