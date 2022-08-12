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

}
