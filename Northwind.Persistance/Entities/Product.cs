using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Persistance.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string QuantityPerUnit { get; set; } = null!; // VT nullable ama biz not null yaptık. 
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
