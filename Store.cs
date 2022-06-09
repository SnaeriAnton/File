using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);
  
            warehouse.ShowInfo(); //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.ShowInfo(); //Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().PayLink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары

            shop.Close();
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;
        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            if (warehouse != null)
            {
                _warehouse = warehouse;
            }

            _cart = new Cart();
            _cart.AddedGood += OnCheakGoodOnWarehouse;
        }

        public Cart Cart()
        {
            return _cart;
        }

        public void Close()
        {
            _cart.AddedGood -= OnCheakGoodOnWarehouse;
        }

        private int OnCheakGoodOnWarehouse(Good good, int count)
        {
            int countGoods = _warehouse.Goods.FirstOrDefault(goods => goods.Key == good).Value;

            if (countGoods <= count)
            {
                count = countGoods;
            }

            _warehouse.RemoveGood(good, count);

            return count;
        }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public void Delive(Good good, int count)
        {
            if (good != null)
            {
                if (count < 0)
                {
                    count = 0;
                }
                _goods.Add(good, count);
            }
        }

        public void ShowInfo()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"Name - {good.Key.Name}, Count: {good.Value}");
            }
        }

        public void RemoveGood(Good good, int count)
        {
            _goods[good] -= count;
        }
    }

    public class Good
    {
        private string _name;

        public string Name => _name;

        public Good(string name)
        {
            _name = name;
        }
    }

    public class Cart
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();
        private Order _order;

        public delegate int Count(Good goos, int count);

        public Count AddedGood;

        public Cart()
        {
            _order = new Order(this);
        }

        public void Add(Good good, int count)
        {
            if (good != null)
            {
                if (count < 0)
                {
                    count = 0;
                }

                count = AddedGood(good, count);

                _goods.Add(good, count);
            }
        }

        public void ShowInfo()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"Name - {good.Key.Name}, Count: {good.Value}");
            }
        }

        public Order Order() 
        {
            _order.PayGoods();
            return _order;
        } 

        public void Clear()
        {
            _goods.Clear();
        }
    }

    public class Order
    {
        private Cart _cart;

        public string PayLink { get; }

        public Order(Cart cart)
        {
            _cart = cart;
            PayLink = "Заказ оплчен";
        }

        public void PayGoods()
        {
            _cart.Clear();
        }
    }
}
