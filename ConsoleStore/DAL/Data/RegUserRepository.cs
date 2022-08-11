using System.Collections.Generic;
using DAL.Entities;

namespace DAL.Data
{
    public class RegUserRepository : UserRepository<RegUser>, IRepository<RegUser>
    {
        public RegUserRepository()
        {
            InitialList();
        }
        private void InitialList()
        {
            base._UserList = new List<RegUser>()
            {
                new RegUser(new System.Random().Next(11111,99999),"Valeria", "Ignatenko", "Ivano-Frankivsk,Ukraine",
                                     "ingava@gmail.com", "Va88+ska", "099-333-1414"),
                new RegUser(new System.Random().Next(11111,99999),"Inga", "Fedko", "Warsaw,Poland",
                                     "fegor77@gmail.com", "Inga$55", "210-733-4563"),
                new RegUser(new System.Random().Next(11111,99999),"Jim", "Karry", "Paris,France",
                                     "jimka11@gmail.com", "kAr14**", "777-411-7898"),
                new RegUser(new System.Random().Next(11111,99999),"Nazar", "Tkach", "Dnipro,Ukraine",
                                     "naz999@gmail.com", "xxxFl99@", "095-910-0012"),
                new RegUser(new System.Random().Next(11111,99999),"Dan", "Sergov", "Odessa,Ukraine",
                                     "danse1999@gmail.com", "daN45$$off", "067-123-1001")
            };
        }

        /// <summary>
        /// Add a new user to the list of users by input parameters
        /// </summary>
        public void Add(RegUser entity)
        {
            base._UserList.Add(entity);
        }
        public void Add(string name, string surname, string location,
                                                     string email, string password, string phone)
        {
            if (new CheckCorrectValue().CheckAllPositions(name, surname, location, email, password, phone))
            {
                base._UserList.Add(new RegUser(new System.Random().Next(11111, 99999), name, surname, location, email, password, phone));
            }
        }

        public void DeleteById(int id)
        {
            foreach (var user in _UserList)
            {
                if (id == user.Id)
                {
                    _UserList.Remove(user);
                }
            }
        }

        public RegUser GetById(int id)
        {
            foreach (var user in _UserList)
            {
                if (id == user.Id)
                {
                    return user;
                }
            }
            return default;
        }

        public string Update(RegUser entity, string change, string NewParametr)
        {
            switch (change.ToLower())
            {
                case "name":
                    if (new CheckCorrectValue().CheckNameOrSurname(NewParametr))
                    {
                        entity.Name = NewParametr;
                    }
                    return "Value was changed correctly";

                case "surname":
                    if (new CheckCorrectValue().CheckNameOrSurname(NewParametr))
                    {
                        entity.Surname = NewParametr;
                    }
                    return "Value was changed correctly";

                case "location":
                    if (new CheckCorrectValue().CheckLocation(NewParametr))
                    {
                        entity.Location = NewParametr;
                    }
                    return "Value was changed correctly";

                case "email":
                    if (new CheckCorrectValue().CheckEmail(NewParametr))
                    {
                        entity.Email = NewParametr;
                    }
                    return "Value was changed correctly";

                case "password":
                    if (new CheckCorrectValue().GetPasswordStrength(NewParametr))
                    {
                        entity.Password = NewParametr;
                    }
                    return "Value was changed correctly";

                case "phone":
                    if (new CheckCorrectValue().CheckPhoneNumber(NewParametr))
                    {
                        entity.PhoneNumber = NewParametr;
                    }
                    return "Value was changed correctly";

                default:
                    return $"\t- Error! You have entered an incorrect value: {change} ";
            }
        }

        public override RegUser SearchUser(string email, string password)
        {
            foreach (var item in _UserList)
            {
                if (item.Email == email && item.Password == password)
                {
                    return item;
                }
            }
            return default;
        }

        /// <summary>
        /// Get a list of users
        /// </summary>
        public List<RegUser> GetUserList() => base._UserList;

        /// <summary>
        /// Determines whether the user exists in the base
        /// </summary>
        public override bool UserIsExist(string email, string password)
        {
            foreach (var user in base._UserList)
            {
                if (user.Email == email && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
