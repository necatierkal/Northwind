using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Northwind.Api.Validators;
using Northwind.Persistance.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;


/*Bütün sunucu ayarlarý baðlantý ve baðýmlýlýk ayarlarý middleware ayarlarý burada yapýlacak. Tek bir noktadan yapýlmalý.*/



var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
//Burasýnýn altý IOC Container dýr.

builder.Services.AddControllers(); //Controller (MVC patterni kullanýlacak) ile çalýþacaðýmýzý belirtiyoruz, gerekli baðýmlýlýk ve konfigürasyonlarý eklemesini istedik. 
//Bu satýr use controller tikini iþaretlediðimiz için geldi. (Yeni Proje oluþtururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer(); //Swagger ile ilgili gerekli konfigürasyon ve baðýmlýlýklarý ekler 
builder.Services.AddSwaggerGen(); //Swagger ile ilgili gerekli konfigürasyon ve baðýmlýlýklarý ekler

 

//builder.Services.AddFluentValidation();//Fluent Validation konfigürasyonunu ekledik. Deprecated olmuþ yani kaldýrýlacak bu þekilde kullanmamalýyýz.
//Yukarýdaki yerine bu þekilde yazdýk
builder.Services.AddFluentValidationAutoValidation(t =>
{
    t.DisableDataAnnotationsValidation = true; //Data annotion konfigürasyonu da yapmýþtýk. Bu deðer true yapýlýrsa data annotion konfigürasyonlarýný görmez. Sadece fluent mapping yapar.
}).AddValidatorsFromAssemblyContaining<ProductValidator>();//Category için tekrar yapmaya gerek yok çünkü ayný assemblyde. Product validator ýn tanýmlý olduðu tüm validasyonlarý gör dedik.
//AddValidatorsFromAssemblyContaining<Program>();// Program nesnesinin tanýmlý olduðu assembly i verdik yani northwind.api.Productvalidator da diyebilirdik. yine northwind api assemblisini vermiþ olurduk.
//Bu kod bloðu sayesinde kurallarýný Northwind api içerisinde Validators içinde geçen tüm classlarda (ProdcutValidator,CategoryValidator....)
//data annotion yoluyla belirlediðimiz kurallarý ototmatik yönetir.

//Hata yönetim standartlarýný belirlemek için RFC7231 standartlarý var.


builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    //1.yöntem
    //var connStr = builder.Configuration.GetSection("ConnectionStrings")["NorthwindConnection"]; //appsetting içerisindeki her süslü parantez arasý bir section dýr.
                                                                                                //Connection string altýndaki northwindconnection deðerini verdi.
    //2.yöntem
    //var connStr = builder.Configuration.GetValue<string>("ConnectionStrings:NorthwindConnection");  //Connection string altýndaki northwindconnection deðerini verdi.

    //3.yöntem
    var connStr = builder.Configuration.GetConnectionString("NorthwindConnection");//1.yöntemin kýsayoludur. (Extension-shorthand)

    opt.UseSqlServer(connStr);//appsetting.json içerisindeki connection okundu.

   // opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= Northwind; Integrated Security=true");//Burada deðil appsettings içerisinde yazýlmalý
});//AddContext hem addscope yaptý hem de options ý build etti. 



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //Asp.net core da birden fazla authentication ile çalýþýlabilir.
                                                                           //Default olarak jwt ile çalýþacaðýz dedik burada. Fakat Jwt nin de bir takým konfigürasyonlara ihtiyacý var.
                                                                           //JWT token authorization headrýnda gider.
    .AddJwtBearer(opt=>
                                                                
    {
        var issuer="http://abc.com";
        var key = "komplex_salt_key_+#445fggrgf_fdfd4545454";
        SymmetricSecurityKey issuerSigninigKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        opt.TokenValidationParameters = new TokenValidationParameters //Gelen token ý doðrulama kontrollerinin konfigürasyonu
        {
            ValidIssuer = issuer,
            IssuerSigningKey=issuerSigninigKey,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "role", //Burada bu konfigürasyonu yapmazsak AccountController içindeki  new Claim("role","Moderator"), tanýmýný role keywordü ile yapamazdýk. Bunun yerine ClaimTyper.Role kullanýrdýk role yerine
            NameClaimType = "username",
        };
    }); 
                                                                          




//builder.Services.AddTransient<NorthwindContext>(); //Herbir istekte yeni instance oluþturur. Ýstek sonlandýðýnda connection sonlanýr. Transaction yoðun iþlemlerde bu kullanýlabilir. 
//builder.Services.AddScoped<NorthwindContext>(); //Northwindcontext için runtime da instance oluþtur.
//Ýlk istekte oluþturur. On kez ayný istekte bulunulursa ilk oluþturduðu instance ý kullanýr
                                                //ayrý ayrý on instance üretmez. Genelde bunu kullanacaðýz.
//builder.Services.AddSingleton<NorthwindContext>();//Sadece bir instance oluþturur. Uygulama ayakta olduðu sürece çalýþýr.
                                                  //Sakýncasý herkes ayný kullanýcýymýþ gibi gelir  loglarda istemediðimiz bir durum.
                                                  //Bu üçünü tekrar araþtýr.


var app = builder.Build();
//---------------------------------------------------------------------------------------------------------
//Buranýn altý middleware larý yazdýðýmýz yer. Üstü ise hertürlü konfigürasyonlarý ve baðýmlýlýklarý yazdýðýmýz yer.
//---------------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason formatýnda wsdl benzeri içerik oluþturur.
    app.UseSwaggerUI();  //Jason formatýnda wsdl benzeri içeriði oluþtururken bir arayüz oluþturur.
}

app.UseAuthentication(); //Bu bir middllware dir. Belirlediðimiz authentication standardýnda her istekte kullanýlmasýný saðlar.
app.UseAuthorization(); //Yetkilendirme varsa çalýþýr. 

app.MapControllers();//Route larý controller lara göre yapmasýný istedik. //Bu satýr use controller tikini iþaretlediðimiz için geldi. (Yeni Proje oluþtururken.)

app.Run();
