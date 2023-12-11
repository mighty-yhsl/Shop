using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Users
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public string Login { get; set; }
        public int RolesId { get; set; }

        public Users()
        {

        }

        public class UsersBuilder
        {
            private int Id { get; set; }
            private string Password { get; set; }
            private string Login { get; set; }
            private int RolesId { get; set; }

            public UsersBuilder SetId(int val)
            {
                Id = val;
                return this;
            }

            public UsersBuilder SetPassword(string val)
            {
                Password = val;
                return this;
            }
            public UsersBuilder SetLogin(string val)
            {
                Login = val;
                return this;
            }
            public UsersBuilder SetRolesId(int val)
            {
                RolesId = val;
                return this;
            }

            public Users Build()
            {
                return new Users()
                {
                    Id = this.Id,
                    Password = this.Password,
                    Login = this.Login,
                    RolesId = this.RolesId
                };
            }

        }
    }
}
