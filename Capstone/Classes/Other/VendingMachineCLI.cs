using System;
using System.Collections.Generic;
using System.Threading;
using Capstone.Classes;
using Capstone.Other.CLI;

namespace Capstone.CLI
{
    public class VendingMachineCLI : VendingMachine
    {
        private VendingMachine _vm = null;

        public void Display()
        {
            _vm.ReadFile();
            bool timeToExit = false;
            while (!timeToExit)
            {
                Console.Clear();

                Console.WriteLine("Main Menu");
                Console.WriteLine();
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(Q) Quit");
                Console.WriteLine();

                Console.Write("What option do you want to select? ");
                char input = Console.ReadKey().KeyChar;

                if (input == '1')
                {
                    Console.Clear();

                    _vm.WriteDictionaryToConsole();
                    
                    Console.WriteLine();

                    Console.ReadKey();
                    Console.Clear();
                }

                else if (input == '2')
                {
                    PurchaseCLI submenu = new PurchaseCLI(_vm);
                    submenu.DisplaySub();
                }

                else if (input == 'Q' || input == 'q')
                {
                    if(CurrentBalance > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Not so fast! You need to complete your purchase first.");
                        Console.ReadKey();
                    }
                    else
                    {
                    timeToExit = true;
                    break;
                    }
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"{input} is not a valid option. Please select a valid option from the menu.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }

            }
        }

        public VendingMachineCLI(VendingMachine vm)
        {
            _vm = vm;
        }
    }
}