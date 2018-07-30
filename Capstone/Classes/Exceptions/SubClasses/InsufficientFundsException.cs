using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
   public class InsufficientFundsException : SystemException
    {
        
        public InsufficientFundsException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
