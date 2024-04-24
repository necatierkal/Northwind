using FluentValidation;
using FluentValidation.AspNetCore;
using Northwind.Api.Validators;
using Northwind.Persistance.Contexts;

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


builder.Services.AddTransient<NorthwindContext>(); //Herbir istekte yeni instance oluþturur. Ýstek sonlandýðýnda connection sonlanýr.
builder.Services.AddScoped<NorthwindContext>(); //Ýlk istekte oluþturur. On kez ayný istekte bulunulursa ilk oluþturduðu connection ý kullanýr ayrý ayrý on instance üretmez.
builder.Services.AddSingleton<NorthwindContext>();//Sadece bir instance oluþturur. Uygulama ayakta olduðu sürece çalýþýr.


var app = builder.Build();
//---------------------------------------------------------------------------------------------------------
//Buranýn altý middleware larý yazdýðýmýz yer. Üstü ise hertürlü konfigürasyonlarý ve baðýmlýlýklarý yazdýðýmýz yer.
//---------------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason formatýnda wsdl benzeri içerik oluþturur.
    app.UseSwaggerUI();  //Jason formatýnda wsdl benzeri içeriði oluþtururken bir arayüz oluþturur.
}

app.UseAuthorization();

app.MapControllers();//Route larý controller lara göre yapmasýný istedik. //Bu satýr use controller tikini iþaretlediðimiz için geldi. (Yeni Proje oluþtururken.)

app.Run();
