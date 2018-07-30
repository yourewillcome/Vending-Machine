using System;
using System.Collections.Generic;
using System.IO;
using Capstone.Classes;

namespace Capstone.Other.CLI
{
    public class PurchaseCLI
    {
        private VendingMachine _vm = null;
        public List<string> transactionLogList = new List<string>();

        string inputAmount = "";
        int inputDollar = 0;

        string slotId = "";

        bool nothingPurchased = true;

        public void DisplaySub()
        {
            bool timeToExit = false;
            while (!timeToExit)
            {
                Console.Clear();

                Console.WriteLine("Purchase Menu");
                Console.WriteLine();
                Console.WriteLine("(1) Feed Money");
                Console.WriteLine("(2) Select Product");
                Console.WriteLine("(3) Finish Transaction");
                Console.WriteLine("(R) Return to Main Menu");
                //Console.WriteLine("Money Provided: " + "$" + inputDollar);
                Console.WriteLine($"Current Balance: ${_vm.CurrentBalance}");

                Console.WriteLine();
                Console.Write("What option do you want to select? ");
                char input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("(1) Feed Money");
                        Console.WriteLine();

                        Console.Write("Enter the whole dollar amount you would like to add. $");
                        inputAmount = Console.ReadLine();
                       bool isWholeDollarAmount = int.TryParse(inputAmount, out int inputDollar);
                        if(isWholeDollarAmount)
                        {
                            _vm.FeedMoney(inputDollar);
                            //transactionLogList.Add($"{DateTime.Now} FEED MONEY: {inputDollar} {_vm.CurrentBalance}");
                        }
                        else
                        {                          
                           Console.WriteLine("Please enter a whole dollar amount.");
                            Console.ReadKey();
                        }
                       // inputDollar = int.Parse(inputAmount);
                       // _vm.FeedMoney(inputDollar);
                       // transactionLogList.Add($"{DateTime.Now} FEED MONEY: {inputDollar} {_vm.CurrentBalance}");

                        Console.WriteLine("(R) Return to Main Menu");
                    }
                    catch (Exception eMoney)
                    {
                        //Console.WriteLine("Keep your change. Please feed whole dollar amounts only.");
                        Console.WriteLine(eMoney.Message);
                        Console.ReadKey();
                    }
                }

                else if (input == '2')
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("(2) Select Product");
                        Console.WriteLine();
                        Console.WriteLine("Please enter your SlotID selection:");
                        Console.WriteLine();
                        slotId = Console.ReadLine().ToUpper(); 
                        _vm.GetItem(slotId);
                        nothingPurchased = false;

                        Console.WriteLine("(R) Return to Main Menu");
                    }
                    catch (InvalidSlotIDException isie)
                    { 
                        Console.ReadKey();
                    }
                    catch (InsufficientFundsException ife)
                    {
                        Console.ReadKey();
                    }
                    catch (OutOfStockException oose)
                    {
                        Console.ReadKey();
                    }

                }
                else if (input == '3')
                {
                    
                    Console.Clear();
                    Console.WriteLine("(3) Finish Transaction");
                    Console.WriteLine();
                    Console.WriteLine("Here is your change back.");
                    Console.WriteLine();

                    Console.WriteLine("{0, -30}{1, -30}{2, -30}", "Nickels", "Dimes", "Quarters");
                    Console.WriteLine("{0, -30}{1, -30}{2, -30}", "-------", "-----", "--------");
                    Console.WriteLine("{0, -30}{1, -30}{2, -30}", _vm.ReturnChange().Nickels, +_vm.ReturnChange().Dimes, +_vm.ReturnChange().Quarters);

                    _vm.CompletePurchase();
                    inputDollar = 0;

                    Console.WriteLine();
                    Console.WriteLine();
                    
                    if (nothingPurchased == false)
                    {
                        Console.WriteLine(_vm._inventory[slotId].ItemName.Consume());
                    }
                    Console.WriteLine();

                    Console.WriteLine("(R) Return to Main Menu");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (input == 'R' || input == 'r')
                {
                    if (_vm.CurrentBalance > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Not so fast! You need to complete your purchase first.");
                        Console.WriteLine("");
                        Console.WriteLine("Press any button to return to the previous menu.");
                        Console.ReadKey();
                    }
                    else
                    {
                        timeToExit = true; 
                        break;
                    }
                }

                else if (input != '1' || input != '2' || input != '3' || input != 'R' || input != 'r')
                {
                    Console.Clear();
                    Console.WriteLine("You have selected an invalid option.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the previous screen and make a valid selection.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Please select a valid option.");
                }
            }
        }
        
        public PurchaseCLI(VendingMachine vm)
        {
            _vm = vm;
        }
    }
}