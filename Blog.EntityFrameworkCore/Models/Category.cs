using System;
using System.Collections.Generic;

#nullable disable

namespace Blog.EntityFrameworkCore.Models
{
    public partial class Category
    {
        public Category()
        {
            Surveys = new HashSet<Survey>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
