using System;
using System.Collections.Generic;

namespace DAL.Data
{

    interface IRepository<T>
    {
        void Add(T entity);
        string Update(T entity, string change, string NewParametr);
        T GetById(int id);
        void DeleteById(int id);
    }

}
