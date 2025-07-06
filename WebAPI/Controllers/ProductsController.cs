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

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Dependency chain --(Bağımlılık zinciri)IProductService bir ProductManager a ve ProductManager da EfProductDal a ihtiyaç duyar bu  da bağımlılık zincirine sebep olur bunu istemeyiz
            //IProductService productService = new ProductManager(new EfProductDal());
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);

            }
            return BadRequest(result.Message);
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



    }
}
