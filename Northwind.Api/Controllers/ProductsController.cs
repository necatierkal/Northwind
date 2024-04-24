using Microsoft.AspNetCore.Mvc;
using Northwind.Persistance.Contexts;
using Northwind.Persistance.Entities;

namespace Northwind.Api.Controllers
{
    //Bir requestte data iki şekilde servera taşınır.
    //1- (URL)Route value (Örnek : products/{id})
    //2- Query String olarak taşınabilir. (Örnek products?name=Eleman&unitPrice=8)
    //Bizim yazdığımız standardımız birincisi.
    // Veri taşımak istiyorsak requestin header ve body kısımları da kullanılabilir.
    //Header da veriler formatında key value pair olarak taşınır. Querystringe benzer bir şekilde olur. İstekle ilgili veriler de burada yollanır.
    //Body de jason formatında veri gönderilir. Sunucu nasıl tasarlanmışsa veri öyle gönderilir. (Header,body, route,querystring den hangisini istiyorsa)
    //Body de istediğimiz yazabiliriz. Header gibi key value pair olmak zorunda değil. Free text bir alandır.
    //Header da Web uygulamalarında form-data formatında, api de json formatında veriyi göndermek bizim standardımız.
    //Server-side applications örneği web uygulamaları ve Client-Side applications Angular.
    //Farklarına bak. Birisinde client ve server aynı yerde tasarlanıyor.(Serverside applications-tamamen server da oluşup html dönüyor.)
    //Diğerinde client ve server ayrı tasarlanır. Json döner html clientta oluşturulur.(Angular gibi)



    /*
     * 
     * 
     * 
    Validasyon, Loglama, hata yönetimi, güvenlik, Authentication , Dependency Injection
    
    Validation: Dışarıdan gelen verinin belirtilen kurallara uygun olup olmadığının kontrolüdür.
                 Güvenli yazılım geliştrimek için dışarıdan gelen hiçbir veriye güvenmemek gerekir.
                 Faklı şekillerde yapılabilir. 
                1-Manuel validasyon (if ile kontrol etmek istediğimiz yerler), 
                2-Data Annotation validasyon (Attribute lar kullanarak validasyon. Model binding yaptıktan sonra bu kurallar entity framework tarafından kontrol edilir.
                  Uymayan birşey varsa Hata mesajı döner. Proudct.cs içindeki QuantityPerUnit üzerine [StringLength(200)] yazdık. Bu bir validasyondur. 
                  Ama Solid prensiplerine uygun değil. Product class ımız hem validasyon hem model binding işinde kullanıldı. Tercih etmiyoruz. 
                3-Başka bir sınıfta fluent validasyon yapacağız. SOLID'e en uygunu bu. Daha nesnel. 
                  AbstractValidator Inherit alınarak oluşturulan bir sınıfın constructorında tek tek validasyon kuralları yazılır. (NotEmpty,NotEqual....)
     
     
     
     */


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
        [ProducesResponseType(StatusCodes.Status200OK)]//Dokümante ettik swagger arayüzünde bu statü kodunu gösterecek.
        public IActionResult Get(int id)
        {
            using (var context = new NorthwindContext())
            {

                var product = context.Products.SingleOrDefault(x => x.Id == id);

                //if (product == null)
                //    return NotFound();
             

                return Ok(product);

            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] Product product)//Buraya yazılan parametrelerin değerleri sırasıyla formfield, request body, route datada, querystring , uploaded files aranır. Default heryere bakar. Biz sadece bir yer belirtip ...ya bak şeklinde konfigüre edebiliriz. (Post metodunda parametrenin önüne [FromQuery],[FromRoute],[FromForm],[FromBody şeklinde belirtilir.])
                                                             //Buraya yazılacak parametreler çok fazlaysa; Product tipini verebiliriz. (Custom bir model yollanırsa karşılaştırmayı modelimi,z göre yapar ve veriyi içindki propertylere göre ister.)
                                                             //Bu örnekte sırayla bakma bodyden bak dedik. Performans ve güvenlik açısından önemli.
                                                             //Model biding sayesinde ototmatik maplendi parametre ve değerleri.
                                                             //Bu metodu kullanmazsak :
                                                             // Clienttan gelen requestların bütün valuelarına HttpContext.Request içerisine RouteValues,querystrings,Cookies,Header yazıp erişebilirdik.
        {

            using var context = new NorthwindContext();
            context.Products.Add(product);
            context.SaveChanges();

            // return Created(string.Empty,null);//Bir uri ve obje istiyor. Response body de dönmesini istediklerimiz buraya eklenir. Biz boş döndüğümüz için böyle yazdık.
            // Uri : Response ın header ına location diye bir data ekler. Bu da create ettiğin veriye nereden ulaşacağının bilgisini ekler.Gereksiz bir kullanım. O yüzden boş gönderdik. Null da da dönüş notu ekleyebiliriz.

            return Created($"products/{product.Id}", null);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]//Swaggerı bilgilendirmek için koyuyoruz.
        //Putta önce ürünü id ye göre getirip sonra güncelliyoruz. //Tamamını güncellemek için Kullanıyoruz. 
                                                        //Seçilen satırın her alanı güncellenmeli.
        public IActionResult Put(int id,[FromBody] Product product)//burada id değereini route tan aldık. Body den gelse de işlemi route taki id ye göre yapacak.
        { 
            //if(product.UnitPrice<0)
            //    return BadRequest("Fiyat 0 dan büyük olmalı"); //Validasyon örneği. Sonucunda badrequest döndük.
            //                                                   //
            using var context = new NorthwindContext();
            var addedProduct = context.Products.Single(x=>x.Id==id);//route tan gelen id
            addedProduct.UnitsInStock = product.UnitsInStock;
            addedProduct.UnitPrice = product.UnitPrice;
            addedProduct.Discontinued = product.Discontinued;
            addedProduct.CategoryId = product.CategoryId;
            addedProduct.QuantityPerUnit = product.QuantityPerUnit;
            addedProduct.Name = product.Name;

            
         
            context.SaveChanges();

         
            return Ok();
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
