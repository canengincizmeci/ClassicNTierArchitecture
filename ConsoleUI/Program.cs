// See https://aka.ms/new-console-template for more information
using Business.Abstract;
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
    //Product sınıfında ICategoryService kulalnılıdğından CategoryManeger ile bağımlılık oldu ondan yazdık
    ProductManager productManager = new ProductManager(new EfProductDal(), new CategoryManager(new EfCategoryDal()));

    var result = productManager.GetProductDetails();
    if (result.Success)
    {
        foreach (var product in result.Data)
        {
            Console.WriteLine(product.ProductName + "/" + product.CategoryName);
        }
    }
    else
    {
        Console.WriteLine(result.Message);
    }
}

static void CategoryTest()
{
    CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
    foreach (var category in categoryManager.GetAll().Data)
    {
        Console.WriteLine(category.CategoryName);
    }
}