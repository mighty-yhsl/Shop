using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Manufacturer()
        {

        }

        public class ManufacturerBuilder
        {
            private int Id { get; set; }
            private string Name { get; set; }

            public ManufacturerBuilder SetId(int val)
            {
                Id = val;
                return this;
            }

            public ManufacturerBuilder SetName(string name)
            {
                Name = name;
                return this;
            }

            public Manufacturer Build()
            {
                return new Manufacturer()
                {
                    Id = this.Id,
                    Name = this.Name
                };
            }

        }
    }

}
