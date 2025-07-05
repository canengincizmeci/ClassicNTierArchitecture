using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        //Burada this ile succes değeri atayan constructor u çağırırız ve böylece aynı anda iki constructor çalışmış olur ve diğer constructor içinde de tekrar aynı satırı yazmaya gerek kalmaz böylece dry ihlal edilmez
        public Result(bool success, string message) : this(success)
        {
            Message = message;

        }
        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
