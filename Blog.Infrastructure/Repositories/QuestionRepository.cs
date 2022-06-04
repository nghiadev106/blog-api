using Microsoft.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using Blog.Infrastructure.Infrastructure;
using Blog.Infrastructure.Repositories.Interfaces;
using Blog.Shared.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId)
        {
            var questions = await DbContext.Questions.Where(x => x.SurveyId == surveyId).OrderBy(x=>x.SortOrder)
                .Select(q => new QuestionDto { 
                    Id=q.Id,
                    Name=q.Name,
                    Description=q.Description,
                    SurveyId=q.SurveyId,
                    SortOrder=q.SortOrder,
                    QuestionTypeId=q.QuestionTypeId,
                    QuestionTypeName=q.QuestionType.Name,
                    CreateDate=q.CreateDate
                }).ToListAsync();            
            return questions;
        }

        public async Task<IEnumerable<QuestionType>> GetAllQuestionTypes()
        {
            var questionTypes = await DbContext.QuestionTypes.ToListAsync();
            return questionTypes;
        }
    }
}
