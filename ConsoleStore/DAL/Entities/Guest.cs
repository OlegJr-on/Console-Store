using DAL.Data;

namespace DAL.Entities
{
    public class Guest : UserEntity
    {
        public Guest(int id) : base(id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        { }

        public override IAccessRole GetAccess()
            => new GuestAccess();
    }
}
