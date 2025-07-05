// See https://aka.ms/new-console-template for more information
using Business.Concrete;
using DataAccess.Concrete.EfCore;
using DataAccess.Concrete.InMemory;

Console.WriteLine("Hello, World!");


//SOLID 
//Open Closed Principle :Yaptığın yazılıma yeni bir özellik ekliyosan mevcuttaki hiç bir koduna dokunamazsın sadece entityframewrok klasörü ekledik

ProductTest();

//IoC öğrenildiğinde burayı new lemeye gerek kalmayacak
//CategoryTest();

static void ProductTest()
{
    ProductManager productManager = new ProductManager(new EfProductDal());

    foreach (var product in productManager.GetProductDetails())
    {
        Console.WriteLine(product.ProductName + "/" + product.CategoryName);
    }
}

static void CategoryTest()
{
    CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
    foreach (var category in categoryManager.GetAll())
    {
        Console.WriteLine(category.CategoryName);
    }
}