using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //Repository dışındaki ürüne özel işlemler buraya yazılır
    public interface IProductDal : IEntityRepository<Product>
    {


    }
}
