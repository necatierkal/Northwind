using Microsoft.AspNetCore.Mvc;
using Northwind.Persistance.Contexts;
using Northwind.Persistance.Entities;

namespace Northwind.Api.Controllers
{

    [Route("api/[controller]")]  //WeatherForecastController i. Yani Domainden sonra yazılacak ilk ifade budur.
                                 //Controllerin adı neyse o olsun dedik burada.
                                 //köşeli paranteziçinde yazmasydık string ifade gibi tırnak için isim verebilirdik.  [Route("wheather-forecast")] diyebilirdik. 
                                 //Bu örnekte domain adından sonra api/Categories oldu.
    [ApiController] //Bir web uygulaması controlleri değil api controlleri olduğunu belirttik. Yani bodyden veri oklu dedik.
    public class CategoriesController : ControllerBase //Controller olabilmesi için ControllerBas den inherit edilmesi gerekiyor.
    {
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(Category[]))]//200 gelirse bu veri tipi dönecek demek. Iactionresult kullanıyorsak bu şekilde dönüş tipi belirtilmeli.
        public IActionResult Get() //Model bu. Normalde Models klasörüne almalıyız. Controller içindeki public metodlara action diyeceğiz. Bu bir action dır.
                          //Varsayılan olarak her türlü http metodlarıyla tetiklenebilir. Post,get, delete hepsiyle tetiklenir. Sadece birini göndermeliyiz.
                          //Örneğin post ve get aynı anda yollanırsa conflict verir. Bir veri dönüyorsa get olarak tasarlanmalı.
                          //O yüzden yukarıyı HttpGet olarak işaretlemeliyiz. Artık yalnızca get metoduyla bu metod çağırılabilir.         [HttpGet]
                          //IActionResult yazarsak swagger dönüş tipini bilemez. Buraya Categories yazmazsak yukarıda ProduceResponseType belirtmeliyiz.

        {
            using (var context = new NorthwindContext())
            {
                var res = context.Categories.ToList();
                return Ok(res);//Parametre olmadığı zaman ok result. İçinde body de olduğu için okobjectresult döndük. IActionresult olarak metodun tipini seçersek hiçbirinde hata vermez.
                //return Ok();
                //return NotFound();
                //Dönmek istediğimiz her actionresult'ı return e yazabiliriz.
            }


        }

        [HttpGet("{id:int}/products")]
        //[Route("3/products")] //böyle ya da yukarıdaki gibi route verilebilir.
        [ProducesResponseType(200, Type = typeof(Product[]))]
        public IActionResult Get(int id) //Bir aksiyon parametre alıyorsa model binding devreye giriyor. Bunun görevi parametreyi buraya bağlamak.
        {
            using (var context = new NorthwindContext())
            {
                var res = context.Products.Where(t=>t.CategoryId==id).ToList();
                return Ok(res);

                /*
                 api/categories/3/products
                oluşan url yukarıdaki gibi : http://localhost:5222/api/Categories/3/products
                burada kategorisi 3 olan product ları listeler
                 

                -----------Uygulama çalışırken instance üretmek: reflection ile olur.---------------
                 
                 */

            }

        }

    }

}
