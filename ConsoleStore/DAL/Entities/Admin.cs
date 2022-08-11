﻿using DAL.Data;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Admin : UserEntity
    {
        public Admin(int id, string name, string surname, string location,
                                             string email, string password, string phone)
        : base(id, name, surname, location, email, password, phone)
        {
            base.OrderList = new List<Order>();
        }

        public override IAccessRole GetAccess()
            => new AdminAccess();
    }
}
