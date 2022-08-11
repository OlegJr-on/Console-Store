using DAL.Data;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public abstract class BaseService
    {
        public UserEntity User { get; set; }

        protected BaseService(UserEntity user)
        {
            this.User = user;
            StorageOfGoods = new ProductRepository();
        }

        /// <summary>
        /// Storage of registered users
        /// </summary>
        public RegUserRepository LoggedUsers { set; get; }


        /// <summary>
        /// List of products
        /// </summary>
        public ProductRepository StorageOfGoods { get; set; }

        /// <summary>
        /// Shows personal information about all registered users. Available for admin.
        /// </summary>
        public string GetInfoRegistredUsers()
        {
            if (User.GetAccess().ViewUsersPersonalInformation())
            {
                var builder = new StringBuilder();
                foreach (var item in LoggedUsers.GetUserList())
                {
                    builder.Append(item.GetPersonalInfo() + "\n" + new string('-', 120) + "\n");
                }
                return builder.ToString();
            }
            else
            {
                return " - Sorry, but you don't have access - ";
            }
        }

        /// <summary>
        /// Changes the personal information of all registered users. Available for admin
        /// </summary>
        public string ChangeUsersPersonalInformation(string email, string change, string NewParametr)
        {
            if (User.GetAccess().ChangeUsersPersonalInformation())
            {
                foreach (var user in LoggedUsers.GetUserList())
                {
                    if (user.Email == email)
                    {
                        LoggedUsers.Update(user, change, NewParametr);
                        return " -> The change was successful <-";
                    }
                }
                return $" - Sorry, but no user with this email: {email} was found - ";
            }
            return " - Sorry, but you don't have access - ";
        }

        /// <summary>
        /// Create a new order by the current user
        /// </summary>
        public string CreateNewOrder(Order order)
        {
            try
            {
                if (User.GetAccess().CreatingNewOrder())
                {
                    User.OrderList.Add(order);
                    return "\t - Order created successfully - ";
                }
                else
                {
                    return " - Sorry, you do not have access - ";
                }
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("order", message: $"Something went wrong, try again | Error: {e.GetType().Name}");
            }
        }

        /// <summary>
        /// Sets the status for the product for the current user. Possible settings: received, canceled
        /// </summary>
        public string SetStatusGoods(int art, string status)
        {
            if (User.GetAccess().SetStatusOrder())
            {
                foreach (Order order in User.OrderList)
                {
                    if (order.Article == art && (int)order.Status < 4)
                    {
                        switch (status)
                        {
                            case "1":
                                order.Status = (StatusOrder)6;
                                return " - Status set successfully - ";
                            case "2":
                                order.Status = (StatusOrder)4;
                                return " - Status set successfully - ";

                            default:
                                return " \t - Sorry, but you entered something wrong, try again - ";
                        }
                    }
                }
            }
            return "Sorry, but something went wrong, try again :(";
        }

        /// <summary>
        /// Sets the delivery status of goods to all users. Admin available
        /// </summary>
        /// <param name="email">The email of the user we want to change</param>
        /// <param name="article">User product article</param>
        /// <param name="status">The status we want to change</param>
        /// <returns></returns>
        public string SetStatusUsersByAdmin(string email, int article, char status)
        {
            if (User.GetAccess().ChangeStatusOrderByAdmin() && new CheckCorrectValue().CheckEmail(email))
            {
                RegUser user = null;
                foreach (RegUser reg_user in LoggedUsers.GetUserList())
                {
                    if (reg_user.Email == email)
                        user = reg_user;
                }
                if (user == null)
                {
                    return " - User don`t found - ";
                }

                foreach (Order order in user.OrderList)
                {
                    if (order.Article == article)
                    {
                        switch (status)
                        {
                            case '1':
                                order.Status = (StatusOrder)7;
                                return " - Status changed! - ";
                            case '2':
                                order.Status = (StatusOrder)2;
                                return " - Status changed! - ";
                            case '3':
                                order.Status = (StatusOrder)3;
                                return " - Status changed! - ";
                            case '4':
                                order.Status = (StatusOrder)5;
                                return " - Status changed! - ";

                            default:
                                return "Sorry, but you entered something wrong, try again";
                        }
                    }
                }
            }
            return " - Sorry something went wrong, try again - ";
        }


        /// <summary>
        /// Review the history of orders and the status of their delivery;
        /// </summary>
        public string ReviewOrders()
        {
            if (User.GetAccess().ReviewHistoryOrders())
            {
                double resultprice = 0;
                var builder = new StringBuilder(new string('-', 120));

                foreach (Order order in User.OrderList)
                {
                    builder.Append("\n" + order.ToString() + "\n" + new string('-', 120));

                    if (order.Status == StatusOrder.New)
                        resultprice += order.ToPay;
                }

                return builder.Append($"\n\nAmount to be paid: {resultprice} UAH\n").ToString();
            }
            return "Sorry, you are not registered :(";
        }

        /// <summary>
        /// Changes product information
        /// </summary>
        /// <param name="change">The criterion by which the product changes</param>
        /// <param name="newParamet">A new value that will change the old one</param>
        /// <param name="title">Title to search for a variable product</param>
        public string ChangeInformationOfProduct(string change, string newParamet, string title)
        {
            if (User.GetAccess().ChangeInformationAboutProduct())
            {
                try
                {
                    var product = this.StorageOfGoods.SearchProduct(title);

                    this.StorageOfGoods.Update(product, change, newParamet);
                    return " - The change was correct - ";

                }
                catch (Exception e)
                {
                    throw new ArgumentNullException("title", message: $"Something went wrong, try again | Error: {e.GetType().Name}");
                }
            }
            else
            {
                return $" - Sorry, but you don't have access - ";
            }
        }
    }
}
