using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    //T yerine kısıtlama yazmıyoruz çünlü sadece entity değil exception vs. de dönülebilir
    public interface IDataResult<T> : IResult
    {
        T Data { get; }


    }
}
