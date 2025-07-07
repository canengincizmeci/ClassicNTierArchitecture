using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
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
        //Bie etntiy menager kendisi hariç başka bir dal'ı enjecte edemez bu yüzden burada ICategoryDal kullanamayız ancak CategoryService ve diğer service sınıfları kullanılabilir
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
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

            //Bir kategoride en fazla 10 ürün olabilir kuralı için bu yazım yanlıştır aynı kural başka yerlerde de kullanılabilir ve tekrar bunu yazmak gerekir bir değişiklik durumunda ise başka yerdeki kodlar değiştirilmediyse istediğimiz katmanlı mimari yapımız olsun iş kurallarının bu şekilde yazımı gene spagetti code yazmamıza ve kötü bir durum oluşmasına sebep olur
            //var products = _productDal.GetAll(p => p.CategoryId == product.CategoryId);
            //if (products.Count >= 10)
            //{
            //    return new ErrorResult(Messages.ProductCountOfCategoryError);
            //}
            //Bu yüzden gidip aşağıya yazdığımız iş kuralı parçacığı metodumuzu kullanırız.(CheckIfProductCountOfCategoryCorrect metodunu yani)
            //result burda hiçbir hata yoksa null döner hata varasa hatalı logic in kendisi döner
            IResult result = BusinessRules.Run(CheckIfProductExists(product.ProductName), CheckIfProductCountOfCategoryCorrect(product.ProductId), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;

            }


            //Bu şekilde kod yazmak yanlış ve gereksiz bir yaklaşımdır
            //if (!CheckIfProductCountOfCategoryCorrect(product.ProductId).Success)
            //{
            //    return new ErrorResult(Messages.ProductCountOfCategoryError);

            //}
            //if (!CheckIfProductExists(product.ProductName).Success)
            //{
            //    return new ErrorResult(Messages.ProductNameExistsErorr);
            //}

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
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            List<Product> products = _productDal.GetAll(p => p.CategoryId == product.CategoryId);
            if (products.Count >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            throw new NotImplementedException();
        }
        //iş kuralı parçacıkları private olarak yazılır
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var products = _productDal.GetAll(p => p.CategoryId == categoryId);
            if (products.Count >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();

        }
        private IResult CheckIfProductExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameExistsErorr);
            }
            return new SuccessResult();

        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }


            return new SuccessResult();
        }
    }
}
