using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;

namespace Modisette.Data
{
    public class SiteContext : DbContext
    {
        public SiteContext (DbContextOptions<SiteContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<CourseDocument> CourseDocuments { get; set; } = default!;
    }
}
