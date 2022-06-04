using Microsoft.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(IDbFactory dbFactory)
          : base(dbFactory) { }

        public async Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId)
        {
            var answers = await DbContext.Answers.Where(x => x.QuestionId == questionId).ToListAsync();
            return answers;
        }
    }
}
