using BLL.Services;
using DAL.Data;
using DAL.Entities;

namespace BLL
{
    /// <summary>
    /// User registration
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// User who has just registered
        /// </summary>
        private RegUser _RegisteredUser;

        public readonly RegUserRepository users = new RegUserRepository();

        public Registration(string name, string surname, string location, string email, string password, string phone)
        {
            FillingData(name, surname, location, email, password, phone);
            users.Add(_RegisteredUser);
        }

        /// <summary>
        /// Get a registered user
        /// </summary>
        /// <returns>Registered user</returns>
        public RegUser GetUser() => _RegisteredUser;

        /// <summary>
        /// Filling in the data for the newly created user
        /// </summary>
        private void FillingData(string name, string surname, string location, string email, string password, string phone)
        {

            if (new CheckCorrectValue().CheckAllPositions(name, surname, location, email, password, phone))
            {
                _RegisteredUser = new RegUser(new System.Random().Next(11111, 99999),
                    name, surname, location, email, password, phone);
            }
        }
    }
}
