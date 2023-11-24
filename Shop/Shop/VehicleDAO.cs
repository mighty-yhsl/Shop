using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using static Shop.Vehicle;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace Shop
{

    public class VehicleDAO : IDAO<Vehicle>
    {
        private readonly string connectionString;
        private MySqlConnection connection;

        private const string GET_ALL_QUERY = "SELECT * FROM vehicles";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM vehicles WHERE Name = @Name";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM vehicles WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE vehicles SET Name = @Name, Price = @Price, Power = @Power, Speed = @Speed, Weight = @Weight, Manufacturer_id = @Manufacturer_id, Supplier_id = @Supplier_id WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO vehicles (Name, Price, Power, Speed, Weight, Manufacturer_id, Supplier_id) VALUES (@Name, @Price, @Power, @Speed, @Weight, @Manufacturer_id, @Supplier_id)";


        public VehicleDAO(string connectionString)
        {
            connection = DAOFactory.GetInstance().GetConnection();
        }

        public List<Vehicle> GetAll()
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Vehicle> vehicles = new List<Vehicle>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                float price = reader.GetFloat(2);
                int power = reader.GetInt32(3);
                int speed = reader.GetInt32(4);
                int weight = reader.GetInt32(5);
                int manufacturer_id = reader.GetInt32(6);
                int supplier_id = reader.GetInt32(7);

                Shop.Vehicle vehicle = new Shop.Vehicle.VehicleBuilder()
                    .SetId(id)
                    .SetName(name)
                    .SetPrice(price)
                    .SetPower(power)
                    .SetSpeed(speed)
                    .SetWeight(weight)
                    .SetManufacturerId(manufacturer_id) 
                    .SetSupplierId(supplier_id)
                    .Build();

                vehicles.Add(vehicle);
            }
            reader.Close();
            connection.Close();
            return vehicles;
        }

        public Vehicle GetByName(string name)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, connection);
            command.Parameters.AddWithValue("@Name", name);

            MySqlDataReader reader = command.ExecuteReader();
            Vehicle vehicle = null;

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                float price = reader.GetFloat(2);
                int power = reader.GetInt32(3);
                int speed = reader.GetInt32(4);
                int weight = reader.GetInt32(5);
                int manufacturer_id = reader.GetInt32(6);
                int supplier_id = reader.GetInt32(7);

                vehicle = new Vehicle.VehicleBuilder()
                    .SetId(id)
                    .SetName(name)
                    .SetPrice(price)
                    .SetPower(power)
                    .SetSpeed(speed)
                    .SetWeight(weight)
                    .SetManufacturerId(manufacturer_id)
                    .SetSupplierId(supplier_id)
                    .Build();
            }
            reader.Close();
            connection.Close();
            return vehicle;
        }

        public void Delete(int id)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(DELETE_BY_ID_QUERY, connection);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"\n Транспорт з Id {id} був успішно видалений.\n");
            }
            else
            {
                Console.WriteLine($"\n Транспорт з Id {id} не був знайдений або не був видалений.\n");
            }
        }

        public void Add(Vehicle vehicle)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(INSERT_QUERY, connection);
            command.Parameters.AddWithValue("@Name", vehicle.Name);
            command.Parameters.AddWithValue("@Price", vehicle.Price);
            command.Parameters.AddWithValue("@Power", vehicle.Power);
            command.Parameters.AddWithValue("@Speed", vehicle.Speed);
            command.Parameters.AddWithValue("@Weight", vehicle.Weight);
            command.Parameters.AddWithValue("@Manufacturer_id", vehicle.Manufacturer_id);
            command.Parameters.AddWithValue("@Supplier_id", vehicle.Supplier_id);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine("\n Транспорт был успешно добавлен.\n");
            }
            else
            {
                Console.WriteLine("\n Транспорт не был добавлен.\n");
            }
        }

        public void Update(Vehicle vehicle)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(UPDATE_QUERY, connection);
            command.Parameters.AddWithValue("@Id", vehicle.Id);
            command.Parameters.AddWithValue("@Name", vehicle.Name);
            command.Parameters.AddWithValue("@Price", vehicle.Price);
            command.Parameters.AddWithValue("@Power", vehicle.Power);
            command.Parameters.AddWithValue("@Speed", vehicle.Speed);
            command.Parameters.AddWithValue("@Weight", vehicle.Weight);
            command.Parameters.AddWithValue("@Manufacturer_id", vehicle.Manufacturer_id);
            command.Parameters.AddWithValue("@Supplier_id", vehicle.Supplier_id);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"\n Транспорт з Id {vehicle.Id} був успішно оновлений.\n");
            }
            else
            {
                Console.WriteLine($"\n Транспорт з Id {vehicle.Id} не був знайдений або не був оновлений.\n");
            }
        }
    }
}

