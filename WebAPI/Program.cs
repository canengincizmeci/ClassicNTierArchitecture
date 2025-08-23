using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EfCore;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Hosting;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Utilities.Security.Encryption;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.DependencyResolvers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
});

// bunu gpt ile ekledim
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// TokenOptions'u appsettings.json'dan oku
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions!.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("AUTH FAILED: " + ctx.Exception?.Message);
                System.Diagnostics.Debug.WriteLine("AUTH FAILED: " + ctx.Exception?.Message);

                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("AUTH SUCCESS: Token validated!");
                System.Diagnostics.Debug.WriteLine("AUTH SUCCESS: Token validated!");

                foreach (var claim in ctx.Principal!.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                    System.Diagnostics.Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");

                }
                return Task.CompletedTask;
            },
            OnMessageReceived = ctx =>
            {
                Console.WriteLine("TOKEN RECEIVED: " + ctx.Token);
                System.Diagnostics.Debug.WriteLine("TOKEN RECEIVED: " + ctx.Token);

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddDependencyResolvers(new ICoreModule[] {
new CoreModule()
});

// Add services to the container.
//Autofac,Ninjet,CastleWindsor,StructureMap,LightInject,DryInject
//AOP yaptýðýmýzda .net in kendi IoC mekanýzmasý yetersiz kalabilir bu yüzden yukardakilerden biri kullanýlýr
builder.Services.AddControllers();
//builder.Services.AddSingleton<IProductService, ProductManager>();
//builder.Services.AddSingleton<IProductDal, EfProductDal>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React tarafýnýn adresi
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



// Swagger/OpenAPI servislerini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<Core.Extensions.ExceptionMiddleware>();
// Geliþtirme ortamýnda Swagger UI göster
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <-- Bu, https://localhost:44317/swagger adresini saðlar
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



