using System.Collections.Generic;
using DAL.Entities;
using System;
using System.Text;
using System.Linq;

namespace DAL.Data
{
    public class ProductRepository : IRepository<Product>
    {
        /// <summary>
        /// List of goods
        /// </summary>
        private List<Product> ListProduct;

        public ProductRepository()
        {
            Initialization();
        }

        public ProductRepository(List<Product> list)
        {
            this.ListProduct = list;
        }

        /// <summary>
        /// Initialization the list of goods
        /// </summary>
        private void Initialization()
        {
            this.ListProduct = new List<Product>()
            {
                new Product(new Random().Next(11111,9999999),"Notebook Lenovo V17 G2 ITL (82NX00DCRA) Iron Grey",
                                "Machinery", "Lenovo CH",14999, "Экран 17.3 TN + film(1600x900) HD +," +
                                " матовый / Intel Pentium Gold 7505(2.0 - 3.5 ГГц) / RAM 4 ГБ"),
                new Product(new Random().Next(11111,9999999),"Notebook HP Pavilion Gaming 15-ec2024ua (5A0U9EA) Shadow Black",
                                "Machinery", "HP Core.Am",42999, "Экран 15.6 IPS(1920x1080) Full HD 144 Гц/ RAM 16 ГБ/nVidia GeForce RTX 3050 Ti, 4 ГБ"),
                new Product(new Random().Next(11111,9999999),"Notebook Honor MagicBook X 15 (BBR-WAH9) Space Grey",
                                "Machinery", "Honore Inc.",21994, "Экран 15.6, IPS(1920х1080) Full HD/ Intel Core i5 - 10210U(1.6 - 4.2 ГГц)"),
                new Product(new Random().Next(11111,9999999),"Apple IPhone 12 128gb","Machinery",
                            "Apple USA", 25999, "Экран (6.1, OLED(Super Retina XDR), 2532x1170)/ Apple A15 Bionic /двойная основная камера:12 Мп"),
                new Product(new Random().Next(11111,9999999),"Apple IPhone 13 256gb","Machinery", "Apple USA", 31299,
                        "Экран (6.1, OLED(Super Retina XDR), 2532x1170) / Apple A15 Bionic"),
                new Product(new Random().Next(11111,9999999),"Refrigerator Bosch KGN4875/UA", "Household machinery",
                        "Bosch Inc.Prod", 22999, "Тип холодильника: Двухкамерный | 350 л | Класс энергопотребления:A+ | No Frost(сухая)"),
                new Product(new Random().Next(11111,9999999),"Refrigerator Samsung RBN6565/UA", "Household machinery", "Samsung KR", 23399,
                        "Тип холодильника: Двухкамерный | 450 л | Класс энергопотребления:B+ |166x59.5x46.5 см"),
                new Product(89898989,"PlayStation 4 1ТБ Black", "Products for gamers", "Sony Int.",10999,
                        "8-ядерный процессор x86-64 AMD Jaguar, 288 x 265 х 39 мм,Вес: 2.1 кг,Bluetooth"),
                new Product(new Random().Next(11111,9999999),"Microsoft Xbox Series S 512GB", "Products for gamers", "Microsoft Corporation USA", 11899,
                        "White,390 x 92 x 260 мм,8-ядерный x86-64 AMD Ryzen Zen 2 (Частота: до 3.5 ГГц)")
            };
        }

        /// <summary>
        /// Get list of goods
        /// </summary>
        /// <returns>List of goods</returns>
        public List<Product> GetList() => this.ListProduct;

        public void Add(Product entity)
        {
            try
            {

                if (!ListProduct.Contains(entity))
                {
                    ListProduct.Add(entity);
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("entity", message: $"\tSomething went wrong, try again | Error: {e.GetType().Name}");
            }
        }

        public void DeleteById(int id)
        {
            foreach (var good in ListProduct)
            {
                if (id == good.Id)
                {
                    ListProduct.Remove(good);
                }
            }
        }

        public Product GetById(int id)
        {
            foreach (var product in ListProduct)
            {
                if (id == product.Id)
                {
                    return product;
                }
            }
            return default;
        }

        public string Update(Product entity, string change, string NewParametr)
        {
            switch (change.ToLower())
            {
                case "title":
                    entity.Title = NewParametr;
                    return " - Product changed successfully - ";

                case "category":
                    entity.Category = NewParametr;
                    return " - Product changed successfully - ";

                case "manufacturer":
                    entity.Manufacturer = NewParametr;
                    return " - Product changed successfully - ";

                case "comment":
                    entity.Comment = NewParametr;
                    return " - Product changed successfully - ";

                case "price":
                    entity.Price = Convert.ToDouble(NewParametr);
                    return " - Product changed successfully - ";

                default:
                    return $" - Error! You have entered an incorrect value: {change} - ";
            }

        }

        /// <summary>
        /// View the list of goods
        /// </summary>
        /// <returns>List of goods in string format</returns>
        public string ViewListGoods()
        {
            var builder = new StringBuilder(new string('-', 120));
            foreach (var item in ListProduct)
            {
                builder.Append(item.ToString() + "\n" + new string('-', 120));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Search for products by title
        /// </summary>
        /// <param name="name">Title of product</param>
        public string SearchProductByName(string name)
        {
            try
            {
                return ListProduct.Where(x => x.Title.ToLower().Contains(name.ToLower())).AsEnumerable().First().ToString();
            }
            catch (Exception)
            {
                return $" - Sorry, but product with this name: |{name}| wasn`t found :(";
            }

        }

        public Product SearchProduct(string title)
        {
            try
            {
                return ListProduct.Where(x => x.Title.ToLower().Contains(title.ToLower())).AsEnumerable().First();
            }
            catch
            {
                return null;
            }
        }
    }
}
