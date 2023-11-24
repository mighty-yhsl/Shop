using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Supplier
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public Supplier()
        {

        }

        public class SupplierBuilder
        {
            private int Id { get; set; }
            private string FirstName { get; set; }
            private string LastName { get; set; }
            private string? Email { get; set; }
            private string? Phone { get; set; }

            public SupplierBuilder()
            {
                Email = null;
                Phone = null;
            }

            public SupplierBuilder SetId(int val)
            {
                Id = val;
                return this;
            }

            public SupplierBuilder SetFirstName(string name)
            {
                FirstName = name;
                return this;
            }

            public SupplierBuilder SetLastName(string surname)
            {
                LastName = surname;
                return this;
            }

            public SupplierBuilder SetEmail (string email)
            {
                Email = email;
                return this;
            }

            public SupplierBuilder SetPhone(string phone)
            {
                Phone = phone;
                return this;
            }
            
            public Supplier Build()
            {
                return new Supplier()
                {
                    Id = this.Id,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email ?? "",
                    Phone = this.Phone ?? ""
 
                };
            }

        }
    }
}
