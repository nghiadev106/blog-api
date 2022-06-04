using Blog.EntityFrameworkCore.Models;
using System;

namespace Blog.Infrastructure.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BlogContext Init();
    }
}
