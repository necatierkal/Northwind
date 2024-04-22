using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwind.Persistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Persistance.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        //Konfigürasyonları ayrı bir klasör içinde ayrı bir cs te yaptık.
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CategoryID");   
            builder.Property(x => x.Name).HasColumnName("CategoryName");   
        }
    }
}
