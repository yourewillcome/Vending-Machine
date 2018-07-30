using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        #region Member Variables
        decimal _totalSales = 0;
        string _directory = Environment.CurrentDirectory;
        string _filePath = @"..\..\..\etc\vendingmachine.csv";
        string _fullPath = " ";
        List<string> salesReport = new List<string>();
        string salesDirectory = Environment.CurrentDirectory;
        string salesFilePath = "SalesReport.txt";
        string salesFullPath = " ";
        
        

    #endregion

    #region Properties
   
        public decimal CurrentBalance { get; set; }

        public Dictionary<string, InventoryItem> _inventory = new Dictionary<string, InventoryItem>();
        public TransactionFileLog _transactionFileLogger = new TransactionFileLog("Log.txt");
        
       
        public List<InventoryItem>Items
        {
            get
            {
                return _items;
            }
        }
        private List<InventoryItem> _items = new List<InventoryItem>();
        #endregion


        #region Constructor

        public VendingMachine()
        {
            
            CurrentBalance = 0;
            _fullPath = Path.Combine(_directory, _filePath);
        }

        #endregion

         #region Methods

        public void ReadFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(_fullPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] itemArray = line.Split('|');
                        if (itemArray[itemArray.Length - 1] == "Chip")
                        {
                            ChipItem chipsItem = new ChipItem(itemArray[1], decimal.Parse(itemArray[2]));
                            InventoryItem invItem = new InventoryItem(chipsItem, 5);
                            _inventory.Add(itemArray[0], invItem);
                        }
                        else if (itemArray[itemArray.Length - 1] == "Candy")
                        {
                            CandyItem candyItem = new CandyItem(itemArray[1], decimal.Parse(itemArray[2]));
                            InventoryItem invItem = new InventoryItem(candyItem, 5);
                            _inventory.Add(itemArray[0], invItem);
                        }
                        else if (itemArray[itemArray.Length - 1] == "Drink")
                        {
                            BeverageItem drinkItem = new BeverageItem(itemArray[1], decimal.Parse(itemArray[2]));
                            InventoryItem invItem = new InventoryItem(drinkItem, 5);
                            _inventory.Add(itemArray[0], invItem);
                        }
                        else if (itemArray[itemArray.Length - 1] == "Gum")
                        {
                            GumItem gumItem = new GumItem(itemArray[1], decimal.Parse(itemArray[2]));
                            InventoryItem invItem = new InventoryItem(gumItem, 5);
                            _inventory.Add(itemArray[0], invItem);
                        }
                    }
                }
            }
            catch (IOException ie) //catch a specific type of Exception
            {
                Console.WriteLine("Error reading the file");
                Console.WriteLine(ie.Message);
            }
            catch (Exception e) //catch a specific type of Exception
            {
                Console.WriteLine("Unknown Error");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dollars"></param>
        public void FeedMoney(int dollars)
        {
            //string directory = Environment.CurrentDirectory;
            //string filePath = "Log.txt";
            //string fullPath = Path.Combine(directory, filePath);
            if (dollars > 0)
            {
                CurrentBalance += dollars;
            }
            else
            {
                throw new Exception("Please enter a dollar amount greater than 0.");
            }
            _transactionFileLogger.RecordDeposit(dollars, CurrentBalance);

            //using (StreamWriter sw = new StreamWriter(fullPath, true))
            //{
            //    sw.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", $"{DateTime.Now}", " FEED MONEY:", $" ${dollars}", $" ${CurrentBalance}");

            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <returns></returns>
        public InventoryItem GetItem(string slotId)
        {
            //string directory = Environment.CurrentDirectory;
            //string filePath = "Log.txt";
            //string fullPath = Path.Combine(directory, filePath);

            if (_inventory.ContainsKey(slotId) == false)
            {
                throw new InvalidSlotIDException("Invalid product code. Please try again.");
            }
            else if (_inventory[slotId].ItemName.Price > CurrentBalance)
            {
                throw new InsufficientFundsException("Insufficient Funds.Please add more money.");
            }
            else if (_inventory[slotId].Quantity > 0 && _inventory[slotId].ItemName.Price <= CurrentBalance)
            {
                _inventory[slotId].Quantity--;
                CurrentBalance -= _inventory[slotId].ItemName.Price;
                _totalSales += _inventory[slotId].ItemName.Price;
                _transactionFileLogger.RecordGetItem(_inventory[slotId].ItemName.Name, slotId, _inventory[slotId].ItemName.Price, CurrentBalance);
                //using (StreamWriter sw = new StreamWriter(fullPath, true))
                //{
                //   sw.WriteLine("{0,-10}{1,-10}{2,-10}{3,-10}", $"{DateTime.Now}", $" {_inventory[slotId].ItemName.Name} {slotId}", $" ${ _inventory[slotId].ItemName.Price}", $" ${CurrentBalance}");
                //}

                //List<string> salesReport = new List<string>();
                salesReport.Add($"{_inventory[slotId].ItemName.Name}|{ 5 - _inventory[slotId].Quantity}");
                //string salesDirectory = Environment.CurrentDirectory;
                //string salesFilePath = "SalesReport.txt";
                //salesFullPath = Path.Combine(salesDirectory, salesFilePath);
                //using (StreamWriter sw = new StreamWriter(salesFullPath, true))
                //{
                //    foreach (var item in salesReport)
                //    {
                //        sw.WriteLine(salesReport);
                //    }
                //    sw.WriteLine($"**TOTAL SALES** ${_totalSales}");
                //}
                return _inventory[slotId];
            }
            else if (_inventory[slotId].Quantity <= 0)
            {
               
                throw new OutOfStockException("Sold Out! Make another selection.");
            }
            else
            {
                throw new Exception("Something went wrong. Please try again");
            }
           
        }

        public void WriteDictionaryToConsole()
        {
            string vmName = "Vendo-Matic 500";
            Console.SetCursorPosition((Console.WindowWidth - vmName.Length) / 2, Console.CursorTop);
            Console.WriteLine(vmName);

            Console.WriteLine();

            Console.WriteLine("{0, -28}{1, -28}{2, -28}{3, -28}", "SlotID", "Product Name", "Price", "Inventory");
            Console.WriteLine("{0, -28}{1, -28}{2, -28}{3, -28}", "------", "------------", "-----", "---------");

            Console.WriteLine();

            foreach (KeyValuePair<string, InventoryItem> item in _inventory)
            {
                if (item.Value.Quantity == 0)
                {
                    Console.WriteLine("{0, -28}{1, -28}{2, -28}{3, -28}", item.Key, item.Value.ItemName.Name, "$" + item.Value.ItemName.Price, "Sold Out!");
                }
                else
                {
                    Console.WriteLine("{0, -28}{1, -28}{2, -28}{3, -28}", item.Key, item.Value.ItemName.Name, "$" + item.Value.ItemName.Price, item.Value.Quantity + " available");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Select any key to return to the main menu");
        }

        public Change ReturnChange()
        {          
            Change changeReturned = new Change(CurrentBalance);
            _transactionFileLogger.RecordReturnChange(CurrentBalance);
            return changeReturned;
        }

        public void CompletePurchase()
        {
            CurrentBalance = 0;
            salesFullPath = Path.Combine(salesDirectory, salesFilePath);
            using (StreamWriter sw = new StreamWriter(salesFullPath, true))
            {
                for(int i = 0; i<salesReport.Count; i++)
                {
                    sw.WriteLine(salesReport[i]);
                }
                sw.WriteLine($"**TOTAL SALES** ${_totalSales}");
            }
        }
        #endregion
    }
}