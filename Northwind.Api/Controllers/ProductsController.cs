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
        
    }
}
