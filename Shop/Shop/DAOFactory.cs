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
        private static DAOFactory _instance;
        private string _connectionString;
        private MySqlConnection _connection;

        private DAOFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DAOFactory()
        {
            string connectionString = "server=localhost;port=3306;username=root;password=secret;database=shop";
            _connection = new MySqlConnection(connectionString);
        }

        public static DAOFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DAOFactory();
            }
            return _instance;
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        public IDAO<T> CreateDAO<T>()
        {
            if (typeof(T) == typeof(Vehicle))
            {
                return new VehicleDAO(_connectionString) as IDAO<T>;
            }
            else if (typeof(T) == typeof(Supplier))
            {
                return new ProxySupplierDAO(_connectionString, SupplierType.SECONDARY) as IDAO<T>;
            }
            else if (typeof(T) == typeof(Manufacturer))
            {
                return new ManufacturerDAO(_connectionString) as IDAO<T>;
            }
            else
            {
                Console.WriteLine("Виникла помилка при створенні об'єкта CreateDAO !!!");
                return null;
            }
        }
    }
}
