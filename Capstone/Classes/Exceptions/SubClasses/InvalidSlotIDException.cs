﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class InvalidSlotIDException : SystemException
    {
        public InvalidSlotIDException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
