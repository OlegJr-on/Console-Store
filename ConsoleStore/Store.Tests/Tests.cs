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



}
