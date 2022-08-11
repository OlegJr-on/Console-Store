
namespace DAL.Entities
{
    public class Product : BaseEntities
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public string Comment { get; set; }

        public Product(int id, string title, string category, string manufacture, double price, string comment)
        => (base.Id, Title, Category, Manufacturer, Price, Comment) = (id, title, category, manufacture, price, comment);

        public override string ToString()
                => $"\n\t\t*{Title}*\n\t+ Category product: {Category}\n\t" +
                        $"+ Production: {Manufacturer}\n\t+ Description: {Comment}" +
                        $"\n\t+ Price: {Price} UAH";
    }
}
