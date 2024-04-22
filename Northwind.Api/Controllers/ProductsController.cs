using Microsoft.AspNetCore.Mvc;
using Northwind.Persistance.Contexts;
using Northwind.Persistance.Entities;

namespace Northwind.Api.Controllers
{


        [Route("api/[controller]")]  
        [ApiController] 
        public class ProductsController : ControllerBase 
        {
            [HttpGet]
            [ProducesResponseType(200, Type = typeof(Product[]))]
            public IActionResult Get()

            {
                using (var context = new NorthwindContext())
                {
                    var res = context.Products.ToList();
                    return Ok(res);
                }


            }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            using (var context = new NorthwindContext())
            {

               var product =  context.Products.SingleOrDefault(x=>x.Id==id); 
             
                return Ok(product);

            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(int id)
        {
            using (var context = new NorthwindContext())
            {
                context.Products.Remove(new Product
                {
                    Id = id
                }); //audit log almıyorsak böyle kullanılabilir. Ya da audit log alıyorsak vt dan getirip onu silebiliriz.
                context.SaveChanges();
                return NoContent();//Sildikten sonran veri olmadığı için nocontent yani 204 döneriz.

            }
        }
        
    }
}
