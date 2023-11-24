using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class DAOFactory
    {
        private static DAOFactory instance;
        private string connectionString;
        private MySqlConnection connection;

        private DAOFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private DAOFactory()
        {
            string connectionString = "server=localhost;port=3306;username=root;password=secret;database=shop";
            connection = new MySqlConnection(connectionString);
        }

        public static DAOFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new DAOFactory();
            }
            return instance;
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public IDAO<T> CreateDAO<T>()
        {
            if (typeof(T) == typeof(Vehicle))
            {
                return new VehicleDAO(connectionString) as IDAO<T>;
            }
            else if (typeof(T) == typeof(Supplier))
            {
                return new SupplierDAO(connectionString) as IDAO<T>;
            }
            else if (typeof(T) == typeof(Manufacturer))
            {
                return new ManufacturerDAO(connectionString) as IDAO<T>;
            }
            else
            {
                Console.WriteLine("Виникла помилка при створенні об'єкта CreateDAO !!!");
                return null;
            }
        }
    }
}
