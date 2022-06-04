using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.EntityFrameworkCore.Models
{
    public partial class BlogCategory
    {
        public BlogCategory()
        {
            Blog = new HashSet<Blogs>();
            Videos = new HashSet<Video>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Blogs> Blog { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}
