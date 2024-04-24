using FluentValidation;
using FluentValidation.AspNetCore;
using Northwind.Api.Validators;
using Northwind.Persistance.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
//Buras�n�n alt� IOC Container d�r.

builder.Services.AddControllers(); //Controller (MVC patterni kullan�lacak) ile �al��aca��m�z� belirtiyoruz, gerekli ba��ml�l�k ve konfig�rasyonlar� eklemesini istedik. 
//Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


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


builder.Services.AddTransient<NorthwindContext>(); //Herbir istekte yeni instance olu�turur. �stek sonland���nda connection sonlan�r.
builder.Services.AddScoped<NorthwindContext>(); //�lk istekte olu�turur. On kez ayn� istekte bulunulursa ilk olu�turdu�u connection � kullan�r ayr� ayr� on instance �retmez.
builder.Services.AddSingleton<NorthwindContext>();//Sadece bir instance olu�turur. Uygulama ayakta oldu�u s�rece �al���r.


var app = builder.Build();
//---------------------------------------------------------------------------------------------------------
//Buran�n alt� middleware lar� yazd���m�z yer. �st� ise hert�rl� konfig�rasyonlar� ve ba��ml�l�klar� yazd���m�z yer.
//---------------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason format�nda wsdl benzeri i�erik olu�turur.
    app.UseSwaggerUI();  //Jason format�nda wsdl benzeri i�eri�i olu�tururken bir aray�z olu�turur.
}

app.UseAuthorization();

app.MapControllers();//Route lar� controller lara g�re yapmas�n� istedik. //Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)

app.Run();
