using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EfCore;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Ioc Container -- Inversion of Control

        //Loosely coupled(Zayıf bağlılık)
        IProductService _productService;//naming convention(isimlendirme standartı)

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public List<Product> Get()
        {
            //Dependency chain --(Bağımlılık zinciri)IProductService bir ProductManager a ve ProductManager da EfProductDal a ihtiyaç duyar bu  da bağımlılık zincirine sebep olur bunu istemeyiz
            //IProductService productService = new ProductManager(new EfProductDal());
            var result = _productService.GetAll();

            return result.Data;
        }




    }
}
