using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EfCore
{
    //Context : Db tabloları ile proje class'larını bağlamak
    public class NorthWindContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Gerçek projede böyle olur     optionsBuilder.UseSqlServer(@"Server=175.45.2.12");

            optionsBuilder.UseSqlServer(@"Server=DESKTOP-CGDTBSJ\MSSQLSERVER5;Database=Northwind_ing;Trusted_Connection=True;");

            //optionsBuilder.UseSqlServer(@"Server=DESKTOP-CGDTBSJ\\MSSQLSERVER5;Database=Northwind_ing;Trusted_Connection=True;");
            ////optionsBuilder.UseSqlServer(@"Server=DESKTOP-CGDTBSJ;Database=Northwind_ing;Trusted_Connection=True;");


        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }






    }
}
