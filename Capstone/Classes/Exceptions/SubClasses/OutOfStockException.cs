using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
   public class OutOfStockException : SystemException
    {
        public OutOfStockException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
