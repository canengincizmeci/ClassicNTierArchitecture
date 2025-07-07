using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //params ile istediğimiz kadar IResult tipinde veri verebiliriz
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                //Başarısız olan iş kuralları business tarafına geri gönderilir ve böylece hata bildirimi yapılır
                if (!logic.Success)
                {
                    return logic;
                }

            }


            return null;
        }
        //Şu şekilde de olabilirdi tamamen yazım farklılığı
        //public static List<IResult> Run(params IResult[] logics)
        //{
        //    List<IResult> errorResults = new List<IResult>();
        //    foreach (var logic in logics)
        //    {
        //        //Başarısız olan iş kuralları business tarafına geri gönderilir ve böylece hata bildirimi yapılır
        //        if (!logic.Success)
        //        {
        //            errorResults.Add(logic);
        //        }

        //    }


        //    return errorResults;
        //}


    }
}
