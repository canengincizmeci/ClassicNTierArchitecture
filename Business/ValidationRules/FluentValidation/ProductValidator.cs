using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {

        public ProductValidator()
        {
            //Burda kurallar tek satırda yazmak solid i ezer ve when kuralları koymayı kısıtlar o yüzden bu şekilde yazarız
            RuleFor(p => p.ProductName).NotEmpty();
            RuleFor(p => p.ProductName).MinimumLength(2);
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
            //Burada kendi metotlarımızı yazma imkanı da elde ederiz
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı");




        }

        //Kendi yazdığımız metot bool tipinde değer döndürür kurala uygunluğa göre true yada false döndürür.'args' ise bizim gönderdiğimiz ProductName'dir
        private bool StartWithA(string arg)
        {
            //Buradaki StartsWith C#'ın içindeki String metotlarından biridir
            return arg.StartsWith("A");
        }
    }
}
