using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Roles
    {
        public int Id { get; set; }
        public string RolesName { get; set; }

        public Roles()
        {

        }

        public class RolesBuilder
        {
            private int Id { get; set; }
            private string RolesName { get; set; }


            public RolesBuilder SetId(int val)
            {
                Id = val;
                return this;
            }

            public RolesBuilder SetRolesName(string val)
            {
                RolesName = val;
                return this;
            }

            public Roles Build()
            {
                return new Roles()
                {
                    Id = this.Id,
                    RolesName = this.RolesName
                };
            }

        }
    }
}
