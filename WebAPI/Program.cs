using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EfCore;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
});

// Add services to the container.
//Autofac,Ninjet,CastleWindsor,StructureMap,LightInject,DryInject
//AOP yaptýðýmýzda .net in kendi IoC mekanýzmasý yetersiz kalabilir bu yüzden yukardakilerden biri kullanýlýr
builder.Services.AddControllers();
//builder.Services.AddSingleton<IProductService, ProductManager>();
//builder.Services.AddSingleton<IProductDal, EfProductDal>();

// Swagger/OpenAPI servislerini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geliþtirme ortamýnda Swagger UI göster
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <-- Bu, https://localhost:44317/swagger adresini saðlar
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



