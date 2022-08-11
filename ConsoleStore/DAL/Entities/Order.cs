using System;

namespace DAL.Entities
{
    public enum StatusOrder
    {
        New = 1,
        Payment_received, Sent,
        Received, Completed, Canceled_by_user, Canceled_by_admin
    }

    public class Order : BaseEntities
    {
        public Product Product { get; set; }
        public StatusOrder Status { get; set; }
        public double ToPay { get; set; }
        public int Quantity { get; private set; }
        public int Article { get; private set; }

        public Order(Product product, int quantity)
        {
            this.Product = product;
            this.Status = StatusOrder.New;
            this.Quantity = quantity;
            this.Article = new Random().Next(1111111, int.MaxValue - 1);
            this.ToPay = product.Price * quantity;
        }

        public override string ToString()
        => $"\n Title: {Product.Title} |Quentity: {Quantity} |Price: {Product.Price}UAH " +
                     $"|Status:{Status} |Article: {Article}\n";

    }
}
