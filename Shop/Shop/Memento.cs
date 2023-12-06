using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Memento
    {
        public int Id { get; }
        public string Name { get; }
        public float Price { get; }
        public int? Power { get; }
        public int? Speed { get; }
        public int? Weight { get; }
        public int ManufacturerId { get; }
        public int SupplierId { get; }

        public Memento(int id, string name, float price, int? power, int? speed, int? weight, int manufacturerId, int supplierId)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Power = power;
            this.Speed = speed;
            this.Weight = weight;
            this.ManufacturerId = manufacturerId;
            this.SupplierId = supplierId;
        }

    }
}
