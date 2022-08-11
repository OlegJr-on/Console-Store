using DAL.Data;
using System.Collections.Generic;

namespace DAL.Entities
{
    public abstract class UserEntity : BaseEntities
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public abstract IAccessRole GetAccess();

        /// <summary>
        /// List of user orders
        /// </summary>
        public List<Order> OrderList { get; set; }

        protected UserEntity(int id, string name, string surname, string location, string email, string password, string phone)
            => (base.Id, Name, Surname, Location, Email, Password, PhoneNumber) = (id, name, surname, location, email, password, phone);


        /// <summary>
        /// Shows personal information about the current user
        /// </summary>
        public string GetPersonalInfo()
        {
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
            {
                return "Sorry, you cannot view this information because you are not registered :(";
            }
            else
            {
                return $"\n\t\tInfo about user({Email}):" +
                    $"\n\tFull name: {Name} {Surname}\n\tLocation: {Location}\n\t" +
                        $"Personal info:    phone:{PhoneNumber} | password: {Password}";
            }
        }

        /// <summary>
        /// Change the personal information of the current user
        /// </summary>
        public string ChangePersonalInfo(string change, string newParametr)
        {
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
            {
                return "Sorry, but you cannot edit the information because you are not registered :(";
            }
            else
            {
                switch (change.ToLower())
                {
                    case "name":
                        if (new CheckCorrectValue().CheckNameOrSurname(newParametr))
                        {
                            this.Name = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    case "surname":
                        if (new CheckCorrectValue().CheckNameOrSurname(newParametr))
                        {
                            this.Surname = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    case "location":
                        if (new CheckCorrectValue().CheckLocation(newParametr))
                        {
                            this.Location = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    case "email":
                        if (new CheckCorrectValue().CheckEmail(newParametr))
                        {
                            this.Email = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    case "password":
                        if (new CheckCorrectValue().GetPasswordStrength(newParametr))
                        {
                            this.Password = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    case "phone":
                        if (new CheckCorrectValue().CheckPhoneNumber(newParametr))
                        {
                            this.PhoneNumber = newParametr;
                            return " - Value was changed correctly - ";
                        }
                        else
                        {
                            return " - Value wasn't changed - ";
                        }

                    default:
                        return $"\t- Error! You have entered an incorrect value: {change} ";
                }
            }
        }
    }
}
