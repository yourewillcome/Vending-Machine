using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;

namespace Capstone.Classes
{
    public class Change
    {
        #region Member Variables

        
        private int _dimes = 0;
        private int _nickels = 0;
        private int _quarters = 0;

        
        #endregion

        #region Properties

        public int Dimes
        {
            get
            {
               return _dimes;
            }
        }
        public int Nickels
        {
            get
            {
                return _nickels;
            }
        }
        public int Quarters
        {
            get
            {
                return _quarters;
            }
        }
       
        #endregion

        #region Constructors

        public Change(decimal amountInDollars)
        {
             
           MakeChange(amountInDollars);

            
        }

        #endregion

        #region Methods
        private void MakeChange(decimal amountInDollars)
        {
            //string directory = Environment.CurrentDirectory;
            //string filePath = "Log.txt";
            //string fullPath = Path.Combine(directory, filePath);

            int amountInCents = (int)(amountInDollars * 100);
            _quarters = amountInCents / 25;     
            _dimes = (amountInCents % 25) / 10;
            _nickels = ((amountInCents % 25) % 10) / 5;

            //using (StreamWriter sw = new StreamWriter(fullPath, true))
            //{
            //    sw.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", $"{DateTime.Now}", " GIVE CHANGE:", $" ${amountInDollars}", " $0.00");
            //}
        }
        #endregion
    }
}
