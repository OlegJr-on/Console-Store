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

    [TestFixture]
    public class Class_Testing_LoggedUsers
    {
        private RegUserRepository Users;

        [SetUp]
        public void SetUp()
        {
            Users = new RegUserRepository();
        }

        [TestCase("gmail.co", null, false)]
        [TestCase("petka@gmail.com", "Shot69+#", false)]
        [TestCase("danse1999@gmail.com", "daN45$$off", true)]
        public void CheckUserIsExist_EmailAndPassword_ReturnedTrueOrFalse(string email, string password,
                                                                    bool expected)
        {
            //arrange
            var actual = Users.UserIsExist(email, password);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckAddNewUser_NewUser_AddedNewUserInList()
        {
            //arrange
            var user = new RegUser(121212, "Name", "Surname", "Loc,ation", "email@gmail.com", "Pass78*(*", "966-333-2022");
            Users.Add(user);

            //assert
            Assert.IsTrue(Users.GetUserList().Contains(user));
        }

        [TestCase("naz999@gmail.com", "xxxFl99@")]
        [TestCase("fegor77@gmail.com", "Inga$55")]
        public void CheckSearchUser_WantedUser_ShouldReturnUser(string email, string password)
        {
            //arrange
            var actual = Users.SearchUser(email, password);
            var expected = Users.GetUserList().Where(x => x.Email == email && x.Password == password)
                .AsEnumerable().First();


            //assert
            Assert.AreSame(expected, actual, message: $"Message: {expected.GetType()}");
        }
    }

    [TestFixture]
    public class Class_Testing_Administrators
    {
        private AdminRepository Admins;

        [SetUp]
        public void SetUp()
        {
            Admins = new AdminRepository();
        }

        [TestCase("gmail.co", null, false)]
        [TestCase("petka@gmail.com", "Shot69+#", true)]
        [TestCase("danse1999@gmail.com", "daN45$$off", false)]
        public void CheckAdminIsExist_EmailAndPassword_ReturnedTrueOrFalse(string email, string password,
                                                                    bool expected)
        {
            //arrange
            var actual = Admins.UserIsExist(email, password);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }

    public class AdministratorTests : Class_Testing_User
    {
        private static readonly Admin _Admin = new AdminRepository().SearchUser("wick1990j@gmail.com", "JoW77!");

        public override BaseService CreateService()
            => new AdminService(_Admin);


        [Test]
        public void CheckGetInfoRegistredUsers_Admin_UsersInfoInStr()
        {
            //arrange
            string actual = Service.GetInfoRegistredUsers();
            var expected = new StringBuilder();

            //act
            foreach (var item in new RegUserRepository().GetUserList())
            {
                expected.Append(item.GetPersonalInfo() + "\n" + new string('-', 120) + "\n");
            }
            //assert
            Assert.AreEqual(expected.ToString(), actual);
        }

        [Test]
        public void CheckChangeUsersPersonalInformation_Admin_ChangedValues()
        {
            //arrange
            string expected = "095-666-2222";

            //act
            Service.ChangeUsersPersonalInformation("naz999@gmail.com", "PhOne", expected);

            var actual = Service.LoggedUsers.GetUserList().Where(x => x.Email == "naz999@gmail.com")
                                .Select(x => x.PhoneNumber).AsEnumerable().First();

            //assert
            Assert.AreEqual(expected, actual);
        }
    }

    public class RegisteredUserTests : Class_Testing_User
    {
        private readonly RegUser user1 = new RegUser(89998, "John", "Wick", "Stockholm,Sweden", "wick1990j@gmail.com"
            , "JoW77!", "855-437-3045");

        public override BaseService CreateService()
            => new RegUserService(this.user1);
    }

    public class GuestTests : Class_Testing_User
    {
        public override BaseService CreateService()
            => new GuestService(new Guest(new Random().Next(11111, 99999)));

    }

}
