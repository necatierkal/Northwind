using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Northwind.Api.Models;
using Northwind.Persistance.Contexts;
using Northwind.Persistance.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Northwind.Api.Controllers
{

    [Route("api/[controller]")]  
    [ApiController] 
    public class AccountController : ControllerBase 
    {

       
        [HttpPost]    
        public IActionResult Login([FromBody]LoginModel loginModel)
        { 
            if (loginModel.UserName != "salidemirog" && loginModel.Password != "123456") 
            { 
                return BadRequest("Kullanıcı adı veya şifre hatalı");
            }

            var claims = new List<Claim> //JWT token ın payload bölümündeki her bilgiye claim denir.
            //Birden fazla claim olacağı için list oluşturduk. Type ve value dan oluşur herbir claim.
            {
                new Claim("id","1"),
                new Claim("username",loginModel.UserName),
                new Claim("mail","salih@sld.com"),
                new Claim("role","SuperUser"), //Program.cs te RoleClaimType = "role" tanımını yapmasaydık bu şekilde tanımlayamazdık.
               // new Claim(ClaimTypes.Role,"SuperUser"), şeklinde tanımlama yapmalıydık program cs teki konfigürasyon olmasaydı.
                new Claim("role","Moderator"),
                
            }; //Bundan sonraki işlemlerde bu verileri kullanarak token oluşturacağız.


            var issuer = "http://abc.com";
            var key = "komplex_salt_key_+#445fggrgf_fdfd4545454"; //salt key

            //Buranın aşağısı her projede copy paste yapılabilir.
            var credential = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), "HS256");
            var payload = new JwtPayload(issuer, issuer, claims, null, DateTime.Now.AddDays(7.0), DateTime.Now);

            JwtSecurityToken token = new JwtSecurityToken(new JwtHeader(credential), payload);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);



            return Ok(new
            {
                token = jwtToken,
                type = "Bareer"
            });
        }
          

    }

}
