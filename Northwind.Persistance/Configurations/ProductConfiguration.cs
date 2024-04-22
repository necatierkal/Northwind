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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>   
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ProductID");
            builder.Property(x => x.Name).HasColumnName("ProductName");
        }
    }
}
