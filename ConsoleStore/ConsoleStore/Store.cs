using System;
using BLL;
using BLL.Services;
using DAL.Entities;


namespace PL
{
    static class Store
    {
        static readonly private Action<Authorization> ActionGuest = GuestMode;
        static readonly private Action<Authorization> Authorizing = AuthorizationMode;

        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Choose how you want to visit the store, Guest mode(1) | Authorized user(2) | Registration(3): ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                char c = char.Parse(Console.ReadLine());
                Console.ResetColor();

                switch (c)
                {
                    case '1':
                        ActionGuest?.Invoke(new Authorization());
                        break;

                    case '2':
                        Console.Write("\n|\tInput email: ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        string AuthoEmail = Console.ReadLine();

                        Console.ResetColor();
                        Console.Write("|\tInput password: ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        string AuthoPassword = Console.ReadLine();
                        Console.ResetColor();

                        Authorizing?.Invoke(new Authorization(AuthoEmail, AuthoPassword));
                        break;

                    case '3':
                        Console.WriteLine("\nFill in the data:");
                        Console.Write("Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Surname: ");
                        string surname = Console.ReadLine();

                        Console.Write("Location: ");
                        string location = Console.ReadLine();

                        Console.Write("Email: ");
                        string RegEmail = Console.ReadLine();

                        Console.Write("Password: ");
                        string RegPassword = Console.ReadLine();

                        Console.Write("Phone number(like this 111-111-1111): ");
                        string phone = Console.ReadLine();

                        Authorizing?.Invoke(new Authorization(new Registration(name, surname, location, RegEmail,
                             RegPassword, phone)));
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" - Sorry, you entered wrong value,try again -");
                        Console.ResetColor();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" - Sorry, something went wrong, try again | Error: {e.GetType().Name} - ");
                Console.ResetColor();
            }
        }

        static void GuestMode(Authorization authorization)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t-> You have visited the store in guest mode <-");
            Console.ResetColor();
            do
            {
                Console.WriteLine(new string('-', 120) + "\n-> For a list of available features, select the one you need:(1-4)");
                Console.Write("1) Search for goods by name;\n" +
                                  "2) Autorization;\n" +
                                  "3) Registartion; \n" +
                                  "4) Leave the store;\nEnter: ");
                string c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        Console.Write("\tInput title of product: ");
                        string title = Console.ReadLine();
                        Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.SearchProductByName(title) + "\n");
                        break;

                    case "2":
                        Console.Write("\n|\tInput email: ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        string email = Console.ReadLine();

                        Console.ResetColor();
                        Console.Write("|\tInput password: ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        string password = Console.ReadLine();
                        Console.ResetColor();

                        Authorizing?.Invoke(new Authorization(email, password));
                        return;

                    case "3":
                        Console.WriteLine("\nFill in the data:");
                        Console.Write("Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Surname: ");
                        string surname = Console.ReadLine();

                        Console.Write("Location: ");
                        string location = Console.ReadLine();

                        Console.Write("Email: ");
                        string RegEmail = Console.ReadLine();

                        Console.Write("Password: ");
                        string RegPassword = Console.ReadLine();

                        Console.Write("Phone number(like this 111-111-1111): ");
                        string phone = Console.ReadLine();

                        Authorizing?.Invoke(new Authorization(new Registration(name, surname, location, RegEmail,
                             RegPassword, phone)));
                        return;

                    case "4":
                        authorization.AuthorizedUser = null;
                        Console.WriteLine("\t\t * BYE!*");
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" - You entered wrong value, try again - ");
                        Console.ResetColor();
                        break;
                }

            } while (true);
        }

        static void AuthorizationMode(Authorization authorization)
        {
            if (authorization.IsEntranceCorrect)
            {
                if (authorization.AuthorizedUser is RegUserService)
                {
                    OrdinaryUser(authorization);
                }
                else if (authorization.AuthorizedUser is AdminService)
                {
                    Admin(authorization);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n - Sorry, but you didn`t log in, please try again or register :(");
                Console.ResetColor();
            }


        }

        static void OrdinaryUser(Authorization authorization)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t-> Authorization was successful. You are in the system <-");
            Console.ResetColor();

            do
            {
                Console.ResetColor();
                Console.WriteLine(new string('-', 120) + "\n-> For a list of available features, select the one you need:(1-8)");
                Console.Write("1) Search for goods by name;\n" +
                                  "2) View the list of goods;\n" +
                                  "3) Creating a new order; \n" +
                                  "4) Review the history of orders and the status of their delivery;\n" +
                                  "5) Setting the status of the order;\n" +
                                  "6) Change of personal information;\n" +
                                  "7) View of personal information;\n" +
                                  "8) Sign out of the account;\n" +
                                  "9) Leave the store;\n Enter: ");
                string c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        Console.Write("\tInput title of product: ");
                        string title = Console.ReadLine();
                        Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.SearchProductByName(title) + "\n");
                        break;

                    case "2":
                        Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.ViewListGoods());
                        break;

                    case "3":
                        Console.Write("\n> Enter a product title: ");
                        string ProductTitle = Console.ReadLine();
                        var product = authorization.AuthorizedUser.StorageOfGoods.SearchProduct(ProductTitle);
                        if (product != null)
                        {

                            Console.WriteLine(product.ToString());

                            Console.Write("\n> Enter the quantity of this product: ");
                            int quantity = int.Parse(Console.ReadLine());

                            Console.WriteLine(authorization.AuthorizedUser.CreateNewOrder(new Order(product, quantity)));
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" - Product wasn't found - ");
                            Console.ResetColor();
                        }
                        break;

                    case "4":
                        Console.WriteLine(authorization.AuthorizedUser.ReviewOrders());
                        break;

                    case "5":
                        Console.Write("\n> Enter the product article: ");
                        int art = int.Parse(Console.ReadLine());

                        Console.Write("> Set status Canceled(1) or Received(2): ");
                        string st = Console.ReadLine();

                        Console.WriteLine(authorization.AuthorizedUser.SetStatusGoods(art, st));
                        break;

                    case "6":
                        Console.Write("\n> Enter the parameter you want to change: ");
                        string change = Console.ReadLine();
                        change = change.ToLower();

                        Console.Write("> Enter a new value for this parameter: ");
                        string newParametr = Console.ReadLine();

                        Console.WriteLine(authorization.AuthorizedUser.User.ChangePersonalInfo(change, newParametr));
                        break;

                    case "7":
                        Console.WriteLine(authorization.AuthorizedUser.User.GetPersonalInfo());
                        break;

                    case "8":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t-> You have been logged out <-");
                        Console.ResetColor();

                        ActionGuest?.Invoke(authorization.SignOutAccount());
                        return;

                    case "9":
                        Console.WriteLine("\t\t * BYE!*");
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" - You entered wrong value, try again - ");
                        Console.ResetColor();
                        break;
                }

            } while (true);
        }

        static void Admin(Authorization authorization)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t-> Authorization was successful. You are Administrator <-");
            Console.ResetColor();

            do
            {
                Console.ResetColor();
                Console.WriteLine(new string('-', 120) + "\n-> For a list of available features, select the one you need:(1-12)");
                Console.Write("1) Search for goods by name;\n" +
                                  "2) View the list of goods;\n" +
                                  "3) Create a new order; \n" +
                                  "4) Review the history of orders and the status of their delivery;\n" +
                                  "5) Set the status of your own order;\n" +
                                  "6) Change personal information of users;\n" +
                                  "7) View personal information of users;\n" +
                                  "8) Add a new product;\n" +
                                  "9) Change of information about the product;\n" +
                                  "10) Сhange the status of the user's order;\n" +
                                  "11) Sign out of the account;\n" +
                                  "12) Leave the store;\n Enter: ");
                string c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        Console.Write("\tInput title of product: ");
                        string title = Console.ReadLine();
                        Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.SearchProductByName(title) + "\n");
                        break;

                    case "2":
                        Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.ViewListGoods());
                        break;

                    case "3":
                        Console.Write("\n> Enter a product title: ");
                        string ProductTitle = Console.ReadLine();
                        var product = authorization.AuthorizedUser.StorageOfGoods.SearchProduct(ProductTitle);
                        if (product != null)
                        {

                            Console.WriteLine(product.ToString());

                            Console.Write("\n> Enter the quantity of this product: ");
                            int quantity = int.Parse(Console.ReadLine());

                            Console.WriteLine(authorization.AuthorizedUser.CreateNewOrder(new Order(product, quantity)));
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" - Product wasn't found - ");
                            Console.ResetColor();
                        }
                        break;

                    case "4":
                        Console.WriteLine(authorization.AuthorizedUser.ReviewOrders());
                        break;

                    case "5":
                        Console.Write("\n> Enter the product article: ");
                        int art = int.Parse(Console.ReadLine());

                        Console.Write("> Set status Canceled(1) or Received(2): ");
                        string st = Console.ReadLine();

                        Console.WriteLine(authorization.AuthorizedUser.SetStatusGoods(art, st));
                        break;

                    case "6":
                        Console.Write("\n> Enter the email of the user you want to edit: ");
                        string UserEmail = Console.ReadLine();

                        Console.Write("\n> Enter the parameter you want to change: ");
                        string change = Console.ReadLine();

                        Console.Write("> Enter a new value for this parameter: ");
                        string newParametr = Console.ReadLine();

                        Console.WriteLine(authorization.AuthorizedUser
                            .ChangeUsersPersonalInformation(UserEmail, change, newParametr));
                        break;

                    case "7":
                        Console.WriteLine(authorization.AuthorizedUser.GetInfoRegistredUsers());
                        break;

                    case "8":
                        try
                        {
                            Console.WriteLine("\nEnter the following information:");
                            Console.Write("\t> Title: ");
                            string Newtitle = Console.ReadLine();
                            Console.Write("\t> Category: ");
                            string category = Console.ReadLine();
                            Console.Write("\t> Manufacture: ");
                            string manufacture = Console.ReadLine();
                            Console.Write("\t> Price: ");
                            double price = double.Parse(Console.ReadLine());
                            Console.Write("\t> Description: ");
                            string comment = Console.ReadLine();

                            var NewProduct = new Product(new Random().Next(111111, 9999999), Newtitle, category, manufacture, price, comment);

                            if (!authorization.AuthorizedUser.StorageOfGoods.GetList().Contains(NewProduct))
                            {
                                authorization.AuthorizedUser.StorageOfGoods.Add(NewProduct);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" - New product added successfully - ");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" - Sorry, but such a product exists - ");
                                Console.ResetColor();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Something went wrong, try again | Error: {e.GetType().Name}");
                            Console.ResetColor();
                        }
                        break;

                    case "9":
                        try
                        {
                            Console.Write("\n> Enter a product title: ");
                            string prod_title = Console.ReadLine();
                            var SearchProduct = authorization.AuthorizedUser.StorageOfGoods.SearchProduct(prod_title);
                            if (SearchProduct != null)
                            {
                                Console.WriteLine(SearchProduct.ToString());

                                Console.Write("\nEnter the parameter you want to change: ");
                                string Change = Console.ReadLine();

                                Console.Write("Enter a new value for this parameter: ");
                                string NewParametr = Console.ReadLine();

                                Console.WriteLine(authorization.AuthorizedUser.StorageOfGoods.Update(SearchProduct, Change, NewParametr));

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" - Product wasn't found - ");
                                Console.ResetColor();
                            }

                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Something went wrong, try again | Error: {e.GetType().Name}");
                            Console.ResetColor();
                        }
                        break;

                    case "10":
                        Console.Write("\n> Enter the EMAIL of the user whose status you want to change: ");
                        string email = Console.ReadLine();

                        Console.Write("Enter the article of the product: ");
                        int article = int.Parse(Console.ReadLine());

                        Console.Write("Set the desired status:\n\t1)Canceled;\n\t2)Payment received;" +
                                "\n\t3)Sent;\n\t4)Completed;\n   Input: ");
                        char status = char.Parse(Console.ReadLine());

                        Console.WriteLine(authorization.AuthorizedUser.SetStatusUsersByAdmin(email, article, status));
                        break;

                    case "11":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t-> You have been logged out <-");
                        Console.ResetColor();

                        ActionGuest?.Invoke(authorization.SignOutAccount());
                        return;

                    case "12":
                        Console.WriteLine("\t\t * BYE!*");
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" - You entered wrong value, try again - ");
                        Console.ResetColor();
                        break;
                }

            } while (true);
        }

    }
}
