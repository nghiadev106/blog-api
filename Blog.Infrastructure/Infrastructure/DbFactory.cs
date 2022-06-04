using Microsoft.EntityFrameworkCore;
using QuestionOnline.Infrastructure.Infrastructure;
using Blog.EntityFrameworkCore;
using Blog.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        DbContextOptions<BlogContext> _options;
        BlogContext dbContext;

        public DbFactory(DbContextOptions<BlogContext> options)
        {
            _options = options;
        }

        public BlogContext Init()
        {
            return dbContext ?? (dbContext = new BlogContext(_options));
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
