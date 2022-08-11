using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
    public abstract class UserRepository<T> : IUserIsExist
    {
        public List<T> _UserList { get; set; }

        public abstract T SearchUser(string email, string password);

        public abstract bool UserIsExist(string email, string password);
    }
}
