using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Persistance.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; //Vt da Category Name biz name yaptık. Maplememiz gerekiyor.
    }
}
