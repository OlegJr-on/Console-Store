namespace DAL.Data
{
    /// <summary>
    /// Sets the level of access to different roles
    /// </summary>
    public interface IAccessRole
    {
        public bool ViewListOfGoods();
        public bool SearchForGoodsByName();
        public bool CreatingNewOrder();
        public bool ReviewHistoryOrders();
        public bool SetStatusOrder();
        public bool ChangePersonalInformation();
        public bool ViewUsersPersonalInformation();
        public bool ChangeUsersPersonalInformation();
        public bool AddNewProduct();
        public bool ChangeInformationAboutProduct();
        public bool ChangeStatusOrderByAdmin();
    }

    /// <summary>
    /// Access as admin
    /// </summary>
    public class AdminAccess : IAccessRole
    {
        //  Аllowed
        public bool AddNewProduct() => true;
        public bool ChangeInformationAboutProduct() => true;
        public bool ChangePersonalInformation() => true;
        public bool ChangeStatusOrderByAdmin() => true;
        public bool CreatingNewOrder() => true;
        public bool ReviewHistoryOrders() => true;
        public bool SearchForGoodsByName() => true;
        public bool SetStatusOrder() => true;
        public bool ViewUsersPersonalInformation() => true;
        public bool ChangeUsersPersonalInformation() => true;
        public bool ViewListOfGoods() => true;

        // Forbidden
    }

    /// <summary>
    /// Access as a registered user
    /// </summary>
    public class RegisteredUsersAccess : IAccessRole
    {
        //  Аllowed
        public bool ChangePersonalInformation() => true;
        public bool CreatingNewOrder() => true;
        public bool ReviewHistoryOrders() => true;
        public bool SearchForGoodsByName() => true;
        public bool SetStatusOrder() => true;
        public bool ViewListOfGoods() => true;

        // Forbidden
        public bool AddNewProduct() => false;
        public bool ChangeInformationAboutProduct() => false;
        public bool ChangeStatusOrderByAdmin() => false;
        public bool ViewUsersPersonalInformation() => false;
        public bool ChangeUsersPersonalInformation() => false;

    }

    /// <summary>
    /// Guest access
    /// </summary>
    public class GuestAccess : IAccessRole
    {
        //  Аllowed
        public bool SearchForGoodsByName() => true;

        // Forbidden
        public bool AddNewProduct() => false;
        public bool ChangeInformationAboutProduct() => false;
        public bool ChangePersonalInformation() => false;
        public bool ChangeStatusOrderByAdmin() => false;
        public bool CreatingNewOrder() => false;
        public bool ReviewHistoryOrders() => false;
        public bool SetStatusOrder() => false;
        public bool ViewUsersPersonalInformation() => false;
        public bool ChangeUsersPersonalInformation() => false;
        public bool ViewListOfGoods() => false;
    }
}
