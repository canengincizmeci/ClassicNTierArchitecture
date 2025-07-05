using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EfCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Autofac,Ninjet,CastleWindsor,StructureMap,LightInject,DryInject
//AOP yapt���m�zda .net in kendi IoC mekan�zmas� yetersiz kalabilir bu y�zden yukardakilerden biri kullan�l�r
builder.Services.AddControllers();
builder.Services.AddSingleton<IProductService, ProductManager>();
builder.Services.AddSingleton<IProductDal, EfProductDal>();

// Swagger/OpenAPI servislerini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geli�tirme ortam�nda Swagger UI g�ster
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <-- Bu, https://localhost:44317/swagger adresini sa�lar
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
