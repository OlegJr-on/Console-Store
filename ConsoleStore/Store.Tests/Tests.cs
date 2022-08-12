using NUnit.Framework;
using System;
using System.Linq;
using DAL.Entities;
using DAL.Data;
using BLL.Services;
using BLL;
using System.Text;

namespace Store.Tests
{
    [TestFixture]
    public class Class_Testing_Registration
    {
        [TestCase("Frodo", "Begenns", "Shire,Middle-earth", "frod99@gmail.com", "PasS099&*", "047-633-2222")]
        [TestCase(null, "06", "Kyiv,Ukraine", "lolo9@gmail.com", "nghjkl*", "666-633-2222")]
        [TestCase("Nill", "Johnson", "London,UK", "nil@gmail.com", "N66ghjkl*", "6549865")]
        public void CheckUserRegistration_NewUserData_ShouldReturnTrueOrFalse(string name, string surname, string location, string email,
                                                                            string password, string phone)
        {
            //arrange
            var actual = new Registration(name, surname, location, email, password, phone);
            bool result = false;

            //act
            if (actual.users.GetUserList().Contains(actual.GetUser()))
                result = true;

            //assert
            Assert.That(result, Is.True, message: "Registration was not successful");
        }
    }


    [TestFixture]
    public class Class_Testing_Authorization
    {
        [TestCase("kpxo", "lxpkco", false)]
        [TestCase("fegor77@gmail.com", "Inga$55", true)]
        [TestCase("petka@gmail.com", "Shot69+#", true)]
        [TestCase(null, "Pswrd077--*", false)]
        public void CheckCtorAuthorization_EmailPassword_ShouldReturnTrueOrFalse(string email, string password, bool expected)
        {
            //arrange
            var authorization = new Authorization(email, password);
            bool actual = authorization.IsEntranceCorrect;

            //assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("Frodo", "Begenns", "Shire,Middle-earth", "frod99@gmail.com", "PasS099&*", "047-633-2222", true)]
        [TestCase(null, "null", "_", "gmail.com", "nghjkl*", "6669933-2222", false)]
        [TestCase("Bill", "Marlin", "London,UK", "bil@gmail.com", "N66ghjkl*", "6549865", false)]
        public void CheckCtorAuthorization_Registration_ShouldReturnTrueOrFalse(string name, string surname, string location, string email,
                                                                            string password, string phone, bool expected)
        {
            //arrange
            var authorization = new Authorization(new Registration(name, surname, location, email, password, phone));
            var actual = authorization.IsEntranceCorrect;

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckEmptyCtorAuthorization_ShouldReturnGuestMode()
        {
            //arrange
            var authorization = new Authorization();
            bool result = false;

            //act
            if (authorization.AuthorizedUser is GuestService)
            {
                result = true;
            }

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckSignOutAccount_ShouldReturnNewAuthorizationMode()
        {
            //arrange
            var authorization = new Authorization("fegor77@gmail.com", "Inga$55");
            bool result = false;

            //act
            if (authorization.IsEntranceCorrect && authorization.AuthorizedUser is RegUserService)
                authorization = authorization.SignOutAccount();


            if (authorization.AuthorizedUser is GuestService)
                result = true;

            //assert
            Assert.IsTrue(result);
        }
    }

    [TestFixture]
    abstract public class Class_Testing_User
    {
        public abstract BaseService CreateService();

        protected BaseService Service;

        [SetUp]
        public void SetUp()
        {
            Service = CreateService();
        }

        [Test]
        public void CheckGetPersonalInfo_User_ShouldReturnInfoUserInStr()
        {
            //arrange
            string actual = Service.User.GetPersonalInfo();
            string expected = "\n\t\tInfo about user(wick1990j@gmail.com):" +
                    "\n\tFull name: John Wick\n\tLocation: Stockholm,Sweden\n\t" +
                        $"Personal info:    phone:855-437-3045 | password: JoW77!";
            //act
            if (Service is GuestService)
            {
                expected = "Sorry, you cannot view this information because you are not registered :(";
            }

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckChangePersonalInfo_User_ChangedUserInfo()
        {
            //arrange
            var ChangedName = "John";

            string actual = Service.User.ChangePersonalInfo("Name", ChangedName);
            string expected = " - Value was changed correctly - ";

            //act
            if (Service is GuestService)
            {
                expected = "Sorry, but you cannot edit the information because you are not registered :(";
            }

            //assert
            Assert.AreEqual(expected, actual);

            if (!(Service is GuestService))
                Assert.AreEqual(ChangedName, Service.User.Name, message: "The value hasn`t changed");
        }

        [TestCase("Iphone", 5)]
        public void CheckCreateNewOrder_User_AddedOrder(string title, int quantity)
        {
            if (this.Service.User.GetAccess().CreatingNewOrder())
            {
                //arrange
                var order = new Order(new ProductRepository().SearchProduct(title), quantity);

                //act
                Service.CreateNewOrder(order);

                //assert
                Assert.That(Service.User.OrderList.Select(x => x.Article == order.Article).AsEnumerable().First(), Is.True);
            }
        }

        [Test]
        public void CheckSetStatusGoods_User_SettedStatusOnOrder()
        {
            if (Service.User.GetAccess().SetStatusOrder())
            {
                //arrange
                var order = new Order(product: new ProductRepository().SearchProduct("playstation"),
                                      quantity: 4);

                //act
                Service.CreateNewOrder(order);

                Service.SetStatusGoods(order.Article, "2");
                //assert
                Assert.That(Service.User.OrderList[0].Status, Is.EqualTo(StatusOrder.Received));
            }
        }

        [Test]
        public void CheckReviewOrders_User_OrderListInStr()
        {
            //arrange
            CheckSetStatusGoods_User_SettedStatusOnOrder();
            var actual = Service.ReviewOrders();
            var expected = new StringBuilder(new string('-', 120));
            double resultprice = 0;

            if (Service.User.GetAccess().ReviewHistoryOrders())
            {
                //act
                Service.User.OrderList.AsParallel().ForAll(x =>
                {
                    if (x.Status == StatusOrder.New)
                        resultprice += x.ToPay;
                    expected.Append("\n" + x.ToString() + "\n" + new string('-', 120));
                }
                );

                expected.Append($"\n\nAmount to be paid: {resultprice} UAH\n");

                //assert
                Assert.AreEqual(expected.ToString(), actual);
            }
            else
            {
                expected.Clear().Append("Sorry, you are not registered :(");
                Assert.AreEqual(expected.ToString(), actual);
            }

        }
    }



}
