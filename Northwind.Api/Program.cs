using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Northwind.Api.Validators;
using Northwind.Persistance.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;


/*B�t�n sunucu ayarlar� ba�lant� ve ba��ml�l�k ayarlar� middleware ayarlar� burada yap�lacak. Tek bir noktadan yap�lmal�.*/



var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
//Buras�n�n alt� IOC Container d�r.

builder.Services.AddControllers(); //Controller (MVC patterni kullan�lacak) ile �al��aca��m�z� belirtiyoruz, gerekli ba��ml�l�k ve konfig�rasyonlar� eklemesini istedik. 
//Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region CORSSETTINGS

builder.Services.AddCors(options =>
options.AddDefaultPolicy(policy =>
                        policy.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       //.AllowCredentials()
//.WithOrigins() dersek t�rnak i�inde domain belirtip iste�in gelece�i adresleri belirtebiliriz. Yukar�da bizim konfig�rasyonda her�eye izin verdik
//Bunu salih hocayla de�il Tamer Alb ile yapt�k. Salih Hoca ile yazd���m�z api leri t�ketebilmek i�in bu konfig�rasyonlara ihtiyac�m�z var.
));
#endregion


builder.Services.AddEndpointsApiExplorer(); //Swagger ile ilgili gerekli konfig�rasyon ve ba��ml�l�klar� ekler 
builder.Services.AddSwaggerGen(); //Swagger ile ilgili gerekli konfig�rasyon ve ba��ml�l�klar� ekler

 

//builder.Services.AddFluentValidation();//Fluent Validation konfig�rasyonunu ekledik. Deprecated olmu� yani kald�r�lacak bu �ekilde kullanmamal�y�z.
//Yukar�daki yerine bu �ekilde yazd�k
builder.Services.AddFluentValidationAutoValidation(t =>
{
    t.DisableDataAnnotationsValidation = true; //Data annotion konfig�rasyonu da yapm��t�k. Bu de�er true yap�l�rsa data annotion konfig�rasyonlar�n� g�rmez. Sadece fluent mapping yapar.
}).AddValidatorsFromAssemblyContaining<ProductValidator>();//Category i�in tekrar yapmaya gerek yok ��nk� ayn� assemblyde. Product validator �n tan�ml� oldu�u t�m validasyonlar� g�r dedik.
//AddValidatorsFromAssemblyContaining<Program>();// Program nesnesinin tan�ml� oldu�u assembly i verdik yani northwind.api.Productvalidator da diyebilirdik. yine northwind api assemblisini vermi� olurduk.
//Bu kod blo�u sayesinde kurallar�n� Northwind api i�erisinde Validators i�inde ge�en t�m classlarda (ProdcutValidator,CategoryValidator....)
//data annotion yoluyla belirledi�imiz kurallar� ototmatik y�netir.

//Hata y�netim standartlar�n� belirlemek i�in RFC7231 standartlar� var.


builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    //1.y�ntem
    //var connStr = builder.Configuration.GetSection("ConnectionStrings")["NorthwindConnection"]; //appsetting i�erisindeki her s�sl� parantez aras� bir section d�r.
                                                                                                //Connection string alt�ndaki northwindconnection de�erini verdi.
    //2.y�ntem
    //var connStr = builder.Configuration.GetValue<string>("ConnectionStrings:NorthwindConnection");  //Connection string alt�ndaki northwindconnection de�erini verdi.

    //3.y�ntem
    var connStr = builder.Configuration.GetConnectionString("NorthwindConnection");//1.y�ntemin k�sayoludur. (Extension-shorthand)

    opt.UseSqlServer(connStr);//appsetting.json i�erisindeki connection okundu.

   // opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= Northwind; Integrated Security=true");//Burada de�il appsettings i�erisinde yaz�lmal�
});//AddContext hem addscope yapt� hem de options � build etti. 



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //Asp.net core da birden fazla authentication ile �al���labilir.
                                                                           //Default olarak jwt ile �al��aca��z dedik burada. Fakat Jwt nin de bir tak�m konfig�rasyonlara ihtiyac� var.
                                                                           //JWT token authorization headr�nda gider.
    .AddJwtBearer(opt=>
                                                                
    {
        var issuer="http://abc.com";
        var key = "komplex_salt_key_+#445fggrgf_fdfd4545454";
        SymmetricSecurityKey issuerSigninigKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        opt.TokenValidationParameters = new TokenValidationParameters //Gelen token � do�rulama kontrollerinin konfig�rasyonu
        {
            ValidIssuer = issuer,
            IssuerSigningKey=issuerSigninigKey,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "role", //Burada bu konfig�rasyonu yapmazsak AccountController i�indeki  new Claim("role","Moderator"), tan�m�n� role keyword� ile yapamazd�k. Bunun yerine ClaimTyper.Role kullan�rd�k role yerine
            NameClaimType = "username",
        };
    });





//builder.Services.AddTransient<NorthwindContext>(); //Herbir istekte yeni instance olu�turur. �stek sonland���nda connection sonlan�r. Transaction yo�un i�lemlerde bu kullan�labilir. 
//builder.Services.AddScoped<NorthwindContext>(); //Northwindcontext i�in runtime da instance olu�tur.
//�lk istekte olu�turur. On kez ayn� istekte bulunulursa ilk olu�turdu�u instance � kullan�r
//ayr� ayr� on instance �retmez. Genelde bunu kullanaca��z.
//builder.Services.AddSingleton<NorthwindContext>();//Sadece bir instance olu�turur. Uygulama ayakta oldu�u s�rece �al���r.
//Sak�ncas� herkes ayn� kullan�c�ym�� gibi gelir  loglarda istemedi�imiz bir durum.
//Bu ���n� tekrar ara�t�r.





var app = builder.Build();
//---------------------------------------------------------------------------------------------------------
//Buran�n alt� middleware lar� yazd���m�z yer. �st� ise hert�rl� konfig�rasyonlar� ve ba��ml�l�klar� yazd���m�z yer.
//---------------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason format�nda wsdl benzeri i�erik olu�turur.
    app.UseSwaggerUI();  //Jason format�nda wsdl benzeri i�eri�i olu�tururken bir aray�z olu�turur.
}

app.UseCors();//CORS Uygulama. Bunu yazmazsak UseCors ta belirtti�imiz konfig�rasyon �al��maz. Tamer Alb. ile yapt�k.Authentication �zerinde olmal�

app.UseAuthentication(); //Bu bir middllware dir. Belirledi�imiz authentication standard�nda her istekte kullan�lmas�n� sa�lar.
app.UseAuthorization(); //Yetkilendirme varsa �al���r. 

app.MapControllers();//Route lar� controller lara g�re yapmas�n� istedik. //Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)

app.Run();
