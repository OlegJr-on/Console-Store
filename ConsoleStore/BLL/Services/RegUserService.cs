using DAL.Entities;
using DAL.Data;

namespace BLL.Services
{
    public class RegUserService : BaseService
    {

        public RegUserService(RegUser user) : base(user)
        {
            base.LoggedUsers = new RegUserRepository();
        }
    }
}
