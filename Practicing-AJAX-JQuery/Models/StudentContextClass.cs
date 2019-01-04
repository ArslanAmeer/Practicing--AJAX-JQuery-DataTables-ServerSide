using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Practicing_AJAX_JQuery.Models
{
    public class StudentContextClass : DbContext
    {
        public StudentContextClass() : base("ConStr")
        { }
        public DbSet<Student> Students { get; set; }

    }
}