using System;
using System.Collections.Generic;

namespace DAL.Data
{
    /// <summary>
    /// Designed to determine if there is a desired user in the current group
    /// </summary>
    public interface IUserIsExist
    {
        public bool UserIsExist(string email, string password);
    }

    interface IRepository<T>
    {
        void Add(T entity);
        string Update(T entity, string change, string NewParametr);
        T GetById(int id);
        void DeleteById(int id);
    }

}
