using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modisette.Models;

namespace Modisette.Data
{
    public class ContactFormContext : DbContext
    {
        public ContactFormContext (DbContextOptions<ContactFormContext> options)
            : base(options)
        {
        }

        public DbSet<Modisette.Models.Contact> Contact { get; set; } = default!;
    }
}
