using DAL.Entities;
using DAL.Data;

namespace BLL.Services
{
    public class AdminService : BaseService
    {

        public AdminService(Admin user) : base(user)
        {
            base.LoggedUsers = new RegUserRepository();
        }
    }
}
