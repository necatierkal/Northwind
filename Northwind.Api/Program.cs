var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //Controller (MVC patterni kullanılacak) ile çalışacağımızı belirtiyoruz, gerekli bağımlılık ve konfigürasyonları eklemesini istedik. 
//Bu satır use controller tikini işaretlediğimiz için geldi. (Yeni Proje oluştururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer(); //Swagger ile ilgili gerekli konfigürasyon ve bağımlılıkları ekler
builder.Services.AddSwaggerGen(); //Swagger ile ilgili gerekli konfigürasyon ve bağımlılıkları ekler

var app = builder.Build();

//Buranın altı middleware ları yazdığımız yer. Üstü ise hertürlü konfigürasyonları yazdığımız yer.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason formatında wsdl benzeri içerik oluşturur.
    app.UseSwaggerUI();  //Jason formatında wsdl benzeri içeriği oluştururken bir arayüz oluşturur.
}

app.UseAuthorization();

app.MapControllers();//Route ları conroller lara göre yapmasını istedik. //Bu satır use controller tikini işaretlediğimiz için geldi. (Yeni Proje oluştururken.)

app.Run();
