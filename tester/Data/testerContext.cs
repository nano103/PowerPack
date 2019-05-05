using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tester.Models
{
    public class testerContext : DbContext
    {
        public testerContext (DbContextOptions<testerContext> options)
            : base(options)
        {
        }

        public DbSet<tester.Models.TestObject> TestObject { get; set; }
    }
}
