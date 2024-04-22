var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //Controller (MVC patterni kullanýlacak) ile çalýþacaðýmýzý belirtiyoruz, gerekli baðýmlýlýk ve konfigürasyonlarý eklemesini istedik. 
//Bu satýr use controller tikini iþaretlediðimiz için geldi. (Yeni Proje oluþtururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer(); //Swagger ile ilgili gerekli konfigürasyon ve baðýmlýlýklarý ekler
builder.Services.AddSwaggerGen(); //Swagger ile ilgili gerekli konfigürasyon ve baðýmlýlýklarý ekler

var app = builder.Build();

//Buranýn altý middleware larý yazdýðýmýz yer. Üstü ise hertürlü konfigürasyonlarý yazdýðýmýz yer.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason formatýnda wsdl benzeri içerik oluþturur.
    app.UseSwaggerUI();  //Jason formatýnda wsdl benzeri içeriði oluþtururken bir arayüz oluþturur.
}

app.UseAuthorization();

app.MapControllers();//Route larý conroller lara göre yapmasýný istedik. //Bu satýr use controller tikini iþaretlediðimiz için geldi. (Yeni Proje oluþtururken.)

app.Run();
