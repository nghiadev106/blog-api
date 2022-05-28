using SurveyOnline.EntityFrameworkCore.Models;

namespace SurveyOnline.Infrastructure.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private SurveyOnlineContext _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public SurveyOnlineContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        public virtual void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
