using Microsoft.EntityFrameworkCore;
using Northwind.Persistance.Configurations;
using Northwind.Persistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Persistance.Contexts
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration()); //Konfigürasyonları ayrı biryerde yönettiğimiz için burada çağırdık.
            modelBuilder.ApplyConfiguration(new ProductConfiguration()); //Konfigürasyonları ayrı biryerde yönettiğimiz için burada çağırdık.
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= Northwind; Integrated Security=true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
