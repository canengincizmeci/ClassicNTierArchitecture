using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    //Bu result class ının soyut sınıfı olsun diye üretilen bir interface
    //Temel voidler için başlangıç
    public interface IResult
    {
        bool Success { get; }//Örn:Yapamaya çalıştığın iş True yada False
        string Message { get; }//Örn işlem mesajı 'Ürün eklendi'


    }
}
