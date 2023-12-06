using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
     public class Vehicle 
     {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int? Power { get;  set; }
        public int? Speed { get;  set; }
        public int? Weight { get; set; }
        public int Manufacturer_id { get; set; }
        public int Supplier_id { get; set; }
       
        public Vehicle() 
        {

        }

         public class VehicleBuilder
         {
            private int Id { get; set; }
            private string Name { get; set; }
            private float Price { get; set; }
            private int? Power { get; set; }
            private int? Speed { get; set; }
            private int? Weight { get; set; }
            private int Manufacturer_id { get; set; }
            private int Supplier_id { get; set; }

            public VehicleBuilder()
            {
                Power = 0;
                Speed = 0;
                Weight = 0;
            }

            public VehicleBuilder SetSupplierId(int val)
            {
                Supplier_id = val;
                return this;
            }

            public VehicleBuilder SetManufacturerId(int val)
            {
                Manufacturer_id = val;
                return this;
            }

            public VehicleBuilder SetId(int val)
            {
                Id = val;
                return this;
            }

            public VehicleBuilder SetName(string val)
            {
                Name = val;
                return this;
            }

            public VehicleBuilder SetPrice(float val)
            {
                Price = val;
                return this;
            }

            public VehicleBuilder SetPower(int? val)
            {
                 Power = val;
                 return this;
            }

             public VehicleBuilder SetSpeed(int? val)
             {
                 Speed = val;
                 return this;
             }

             public VehicleBuilder SetWeight(int? val)
             {
                 Weight = val;
                 return this;
             }

             public Vehicle Build() 
             {
                    return new Vehicle()
                    {
                         Id = this.Id,
                         Name = this.Name,
                         Price = this.Price,
                         Power = this.Power ?? 0,
                         Speed = this.Speed ?? 0,
                         Weight = this.Weight ?? 0,
                         Manufacturer_id = this.Manufacturer_id,
                         Supplier_id = this.Supplier_id
                    };
             }

         }

        public Memento Save()
        {
            Memento memento = new Memento(Id, Name, Price, Power, Speed, Weight, Manufacturer_id, Supplier_id);
            Console.WriteLine($"Стан транспортного засобу збережено.");
            return memento;
        }

        public void Restore(Memento memento)
        {
            if (memento != null)
            {
                Id = memento.Id;
                Name = memento.Name;
                Price = memento.Price;
                Power = memento.Power;
                Speed = memento.Speed;
                Weight = memento.Weight;
                Manufacturer_id = memento.ManufacturerId;
                Supplier_id = memento.SupplierId;

                Console.WriteLine("Стан транспортного засобу відновлено.");
            }
            else
            {
                Console.WriteLine("Помилка: Немає збереженого стану транспортного засобу.");
            }
        }
        
    }
}
