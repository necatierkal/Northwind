var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //Controller (MVC patterni kullan�lacak) ile �al��aca��m�z� belirtiyoruz, gerekli ba��ml�l�k ve konfig�rasyonlar� eklemesini istedik. 
//Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer(); //Swagger ile ilgili gerekli konfig�rasyon ve ba��ml�l�klar� ekler
builder.Services.AddSwaggerGen(); //Swagger ile ilgili gerekli konfig�rasyon ve ba��ml�l�klar� ekler

var app = builder.Build();

//Buran�n alt� middleware lar� yazd���m�z yer. �st� ise hert�rl� konfig�rasyonlar� yazd���m�z yer.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Jason format�nda wsdl benzeri i�erik olu�turur.
    app.UseSwaggerUI();  //Jason format�nda wsdl benzeri i�eri�i olu�tururken bir aray�z olu�turur.
}

app.UseAuthorization();

app.MapControllers();//Route lar� conroller lara g�re yapmas�n� istedik. //Bu sat�r use controller tikini i�aretledi�imiz i�in geldi. (Yeni Proje olu�tururken.)

app.Run();
