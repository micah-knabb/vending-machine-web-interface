using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Exceptions;
using VendingService;
using VendingService.Models;

namespace VendingMachineCLI
{
    public class VendingMachineCLI
    {
        private const string Option_LoginVendingMachine = "1";
        private const string Option_RegisterVendingMachine = "2";
        private const string Option_LogoutVendingMachine = "3";
        private const string Option_DisplayVendingMachine = "1";
        private const string Option_DisplayPurchaseMenu = "2";
        private const string Option_InsertMoney = "1";
        private const string Option_MakeSelection = "2";
        private const string Option_ReturnChange = "3";
        private const string Option_ReturnToPreviousMenu = "r";
        private const string Option_Quit = "q";

        private VendingMachine _vm;

        public VendingMachineCLI(VendingMachine vm)
        {
            _vm = vm;
        }

        public void Run()
        {
            MainMenu();
        }

        private void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintTitle();

                Console.WriteLine(" (1) Login");
                Console.WriteLine(" (2) Register");
                Console.WriteLine(" (Q) Quit");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_LoginVendingMachine)
                {
                    DisplayLogin();
                }
                else if (choice == Option_RegisterVendingMachine)
                {
                    DisplayRegister();
                }
                else if (choice == Option_Quit)
                {
                    exit = true;
                }
                else
                {
                    DisplayInvalidOption();
                    Console.ReadKey();
                }
            }
        }

        private void DisplayLogin()
        {
            Console.Clear();
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();
            try
            {
                _vm.LoginUser(username, password);
                Console.WriteLine($"Welcome {_vm.CurrentUser.FirstName} {_vm.CurrentUser.LastName}");
                Console.ReadKey();
                VendingMenu();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }            
        }

        private void DisplayRegister()
        {
            Console.Clear();

            User user = new User();
            Console.WriteLine("Enter username: ");
            user.Username = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            user.Password = Console.ReadLine();
            Console.WriteLine("Enter password again: ");
            user.ConfirmPassword = Console.ReadLine();
            Console.WriteLine("Enter email: ");
            user.Email = Console.ReadLine();
            Console.WriteLine("Enter first name: ");
            user.FirstName = Console.ReadLine();
            Console.WriteLine("Enter last name: ");
            user.LastName = Console.ReadLine();

            try
            {
                _vm.RegisterUser(user);
                Console.WriteLine($"Welcome {_vm.CurrentUser.FirstName} {_vm.CurrentUser.LastName}");
                Console.ReadKey();
                VendingMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private void VendingMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                PrintTitle();

                Console.WriteLine(" (1) Display vending machine items");
                Console.WriteLine(" (2) Purchase");
                Console.WriteLine(" (3) Logout");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_DisplayVendingMachine)
                {
                    DisplayInventory();
                    Console.ReadKey();
                }
                else if (choice == Option_DisplayPurchaseMenu)
                {
                    DisplayPurchaseMenu();
                }
                else if (choice == Option_LogoutVendingMachine)
                {
                    _vm.LogoutUser();
                    exit = true;
                }
                else
                {
                    DisplayInvalidOption();
                    Console.ReadKey();
                }
            }
        }

        

        private void PrintTitle()
        {
            Console.Clear();

            Console.WriteLine("****************************************************************");
            Console.WriteLine("*                       VENDO-MATIC 5000                       *");
            Console.WriteLine("****************************************************************");

            Console.WriteLine();
        }

        private void DisplayPurchaseMenu()
        {

            while (true)
            {
                PrintTitle();

                Console.WriteLine(" (1) Insert money");
                Console.WriteLine(" (2) Make a selection");
                Console.WriteLine(" (3) Finish Transaction");
                Console.WriteLine(" (R) Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine($" Current balance: {_vm.RunningTotal.ToString("C")}");
                Console.Write(" Please make a choice: "); ;

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_InsertMoney)
                {
                    Console.Write(" How much money do you want to enter? ($1, $2, $5, $10): ");
                    int moneyIn = int.Parse(Console.ReadLine());

                    _vm.FeedMoney(moneyIn);
                }
                else if (choice == Option_MakeSelection)
                {
                    PrintTitle();

                    Console.WriteLine($" Current balance: {_vm.RunningTotal.ToString("C")}");
                    DisplayInventory();

                    Console.WriteLine();

                    Console.Write(" Please select a slot id: ");
                    string slot = Console.ReadLine().ToUpper();

                    Console.WriteLine();

                    try
                    {
                        int col = int.Parse(slot[0].ToString());
                        int row = int.Parse(slot[1].ToString());
                        var purchasedItem = _vm.PurchaseItem(row,col);
                        Console.WriteLine(" Here are your " + purchasedItem.Product.Name);
                        Console.WriteLine(" " + purchasedItem.Category.Noise);
                    }
                    catch (InvalidProductSelection)
                    {
                        Console.WriteLine("Invalid slot id");
                    }
                    catch (InsufficientFundsException)
                    {
                        Console.WriteLine("Insufficient funds");
                    }
                    catch (SoldOutException)
                    {
                        Console.WriteLine("Product is sold out");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine();
                        Console.WriteLine(" Thank you for using Vendo-Matic!");
                        Console.ReadKey();
                    }
                }
                else if (choice == Option_ReturnChange)
                {
                    DisplayReturnedChange();
                    Console.ReadKey();
                }
                else if (choice == Option_ReturnToPreviousMenu)
                {
                    Console.WriteLine(" Returning to previous menu. ");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    DisplayInvalidOption();
                }
            }
        }

        private void DisplayInvalidOption()
        {
            Console.WriteLine(" The option you selected is not a valid option.");
            Console.WriteLine();
        }

        private void DisplayReturnedChange()
        {
            double changeAmt = _vm.RunningTotal;
            Change change = _vm.CompleteTransaction();

            Console.WriteLine($" Your change is: {changeAmt.ToString("C")}");
            Console.WriteLine($"  {change.Dollars.ToString().PadLeft(3)} dollars");
            Console.WriteLine($"  {change.Quarters.ToString().PadLeft(3)} quarters");
            Console.WriteLine($"  {change.Dimes.ToString().PadLeft(3)} dimes");
            Console.WriteLine($"  {change.Nickels.ToString().PadLeft(3)} nickels");
            Console.WriteLine($"  {change.Pennies.ToString().PadLeft(3)} pennies");
            Console.WriteLine();
        }

        private void DisplayInventory()
        {
            Console.Write("Location".PadRight(11));
            Console.Write("Product Name".PadRight(40));
            Console.WriteLine("Purchase Price".PadRight(18));

            // Get All Known Slots
            var existingSlots = _vm.VendingItems;

            // Loop Through Each Slot and Display Its Item Data
            foreach (var slot in existingSlots)
            {
                Console.Write($"{slot.Inventory.Column.ToString()}{slot.Inventory.Row.ToString()}".PadRight(11));

                if (slot.Inventory.Qty == 0)
                {
                    Console.WriteLine("* SOLD OUT *".PadRight(40));
                }
                else
                {
                    Console.Write(slot.Product.Name.PadRight(40));
                    Console.Write((slot.Product.Price.ToString("C").PadRight(18)));
                    Console.WriteLine($"In Stock ({slot.Inventory.Qty})".PadRight(20));
                }
            }
        }
    }
}
