using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        //Bir iş sınıfı başka sınıfları new lemez bunun yerine injection kullanıcaz
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            //if (product.ProductName.Length < 2)
            //{
            //    //magic string : Bu string değerleri ayrı ayrı metotlar içine "..." olarak yazmak ileride hataya sebep olabilir çünkü ifadeler değişirse her yerde tek tek düzeltmek gerekecekdir
            //    return new ErrorResult(Messages.ProductNameInvalid);
            //}

            //???Buradaki context aynen efcore gibi iligili bir thread i anlatır?
          




            _productDal.Add(product);
            return new SuccessResult("Ürün Eklendi");
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 22)
            {
                //MaintenanceTime=Bakım zamanı
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new DataResult<List<Product>>(_productDal.GetAll(), true, Messages.ProductsListed);

        }

        public IDataResult<List<Product>> GetAllByCategory(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 21)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);

            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
    }
}
