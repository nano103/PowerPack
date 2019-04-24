using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ACDC;

namespace Energizer.Models
{
    public class EnergizerContext : DbContext
    {
        public EnergizerContext (DbContextOptions<EnergizerContext> options)
            : base(options)
        {
        }

        public DbSet<ACDC.MeasurePoint> MeasurePoint { get; set; }
    }
}
