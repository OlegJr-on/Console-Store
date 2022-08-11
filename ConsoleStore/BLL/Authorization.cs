using BLL.Services;
using DAL.Data;
using DAL.Entities;

namespace BLL
{
    public class Authorization
    {
        /// <summary>
        /// User who has been logged in
        /// </summary>
        public BaseService AuthorizedUser { get; set; }

        /// <summary>
        /// Verification that the authorization was successful
        /// </summary>
        public bool IsEntranceCorrect { get; set; } = false;

        /// <summary>
        /// Email and password authorization
        /// </summary>
        /// <param name="email">Еmail of the user who wants to log in</param>
        /// <param name="password">Рassword of the user who wants to log in</param>
        public Authorization(string email, string password)
        {
            if (new AdminRepository().UserIsExist(email, password))
            {
                IsEntranceCorrect = true;
                AuthorizedUser = new AdminService(new AdminRepository().SearchUser(email, password));
            }
            else if (new RegUserRepository().UserIsExist(email, password))
            {
                IsEntranceCorrect = true;
                AuthorizedUser = new RegUserService(new RegUserRepository().SearchUser(email, password));
            }
        }

        /// <summary>
        /// Authorization through registration
        /// </summary>
        /// <param name="registration">The registration form of the user who is logging in has arrived</param>
        public Authorization(Registration registration)
        {
            if (registration.GetUser() != null)
            {
                this.AuthorizedUser = new RegUserService(registration.GetUser());
                IsEntranceCorrect = true;
            }
        }

        /// <summary>
        /// Authorization in guest mode
        /// </summary>
        public Authorization()
        {
            AuthorizedUser = new GuestService(new Guest(new System.Random().Next(11111, 99999)));
            IsEntranceCorrect = true;
        }

        /// <summary>
        /// Sign out of the account
        /// </summary>
        /// <returns>Authorization in guest mode</returns>
        public Authorization SignOutAccount()
        {
            AuthorizedUser = null;

            return new Authorization();
        }
    }
}
