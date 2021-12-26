using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ACMWeb.Models;

namespace ACMWeb.Data
{
    public class ACMWebContext : DbContext
    {
        public ACMWebContext (DbContextOptions<ACMWebContext> options)
            : base(options)
        {
        }

        public DbSet<ACMWeb.Models.Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
