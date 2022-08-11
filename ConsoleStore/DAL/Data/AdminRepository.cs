using System.Collections.Generic;
using DAL.Entities;

namespace DAL.Data
{
    public class AdminRepository : UserRepository<Admin>, IRepository<Admin>
    {
        public AdminRepository()
        {
            Initial();
        }
        private void Initial()
        {
            base._UserList = new List<Admin>()
            {
                new Admin(new System.Random().Next(11111,99999),"Oleh", "Mandra", "Kyiv,Ukraine", "mandra9012003@gmail.com"
                                            , "099Ok$$+", "095-937-3031"),
                new Admin(new System.Random().Next(11111,99999),"Katya", "Petrova", "Lviv,Ukraine", "petka@gmail.com"
                                            , "Shot69+#", "066-938-3131"),
                new Admin(new System.Random().Next(11111,99999),"John", "Wick", "Stockholm,Sweden", "wick1990j@gmail.com"
                                             , "JoW77!", "855-437-3045")
            };
        }

        public void Add(Admin entity)
        {
            if (new CheckCorrectValue().CheckAllPositions(entity.Name, entity.Surname, entity.Location,
                entity.Email, entity.Password, entity.PhoneNumber))
            {
                base._UserList.Add(new Admin(entity.Id, entity.Name, entity.Surname, entity.Location,
                                entity.Email, entity.Password, entity.PhoneNumber));
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

        public Admin GetById(int id)
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

        public string Update(Admin entity, string change, string NewParametr)
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

        /// <summary>
        /// Looks for the right admin by email and password
        /// </summary>
        /// <returns>A copy of the required admin</returns>
        public override Admin SearchUser(string email, string password)
        {
            foreach (var item in base._UserList)
            {
                if (item.Email == email && item.Password == password)
                {
                    return item;
                }
            }
            return default;
        }

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
