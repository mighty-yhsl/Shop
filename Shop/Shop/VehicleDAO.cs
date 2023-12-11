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
using System.Numerics;

namespace Shop
{
    public class VehicleDAO : IDAO<Vehicle>, IDAOObservable
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;

        private const string GET_ALL_QUERY = "SELECT * FROM vehicles";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM vehicles WHERE Name = @Name";
        private const string GET_BY_ID_QUERY = "SELECT * FROM vehicles WHERE Id = @Id";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM vehicles WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE vehicles SET Name = @Name, Price = @Price, Power = @Power, Speed = @Speed, Weight = @Weight, Manufacturer_id = @Manufacturer_id, Supplier_id = @Supplier_id WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO vehicles (Name, Price, Power, Speed, Weight, Manufacturer_id, Supplier_id) VALUES (@Name, @Price, @Power, @Speed, @Weight, @Manufacturer_id, @Supplier_id)";

        public VehicleDAO(string connectionString)
        {
            _connection = DAOFactory.GetInstance().GetConnection();
            _observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public List<Vehicle> GetAll()
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, _connection);
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
                Notify("\n Всі транспортні засоби: \n");
                return vehicles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні транспортних засобів: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання транспортних засобів." +
                    $" Будь ласка, перевірте стан системи та спробуйте знову.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Vehicle GetByName(string name)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, _connection);
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
                Notify($"\n Знайдений транспортний засіб з назвою {name}: \n");
                return vehicle;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні транспортного засобу за назвою {name}: {ex.Message}");
                throw new InvalidOperationException($"\n Транспортний засіб з назвою '{name}' не знайдений.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(DELETE_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Notify($"\n Транспорт з Id {id} був видалений \n");
                }
                else
                {
                    Notify($"\n Транспорт з Id {id} не був знайдений або не був видалений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при видаленні транспорту з Id {id}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }
        
        public void Add(Vehicle vehicle)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_QUERY, _connection);
                command.Parameters.AddWithValue("@Name", vehicle.Name);
                command.Parameters.AddWithValue("@Price", vehicle.Price);
                command.Parameters.AddWithValue("@Power", vehicle.Power);
                command.Parameters.AddWithValue("@Speed", vehicle.Speed);
                command.Parameters.AddWithValue("@Weight", vehicle.Weight);
                command.Parameters.AddWithValue("@Manufacturer_id", vehicle.Manufacturer_id);
                command.Parameters.AddWithValue("@Supplier_id", vehicle.Supplier_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Notify($"\n Транспорт з назвою {vehicle.Name} був доданий \n");
                }
                else
                {
                    Notify("\n Транспорт не був доданий.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при додаванні транспорту: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Vehicle vehicle)
        {
            try
            {
               
                _connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", vehicle.Id);
                command.Parameters.AddWithValue("@Name", vehicle.Name);
                command.Parameters.AddWithValue("@Price", vehicle.Price);
                command.Parameters.AddWithValue("@Power", vehicle.Power);
                command.Parameters.AddWithValue("@Speed", vehicle.Speed);
                command.Parameters.AddWithValue("@Weight", vehicle.Weight);
                command.Parameters.AddWithValue("@Manufacturer_id", vehicle.Manufacturer_id);
                command.Parameters.AddWithValue("@Supplier_id", vehicle.Supplier_id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Notify($"\n Транспорт з Id {vehicle.Id} був оновлений");
                }
                else
                {
                    Notify($"\n Транспорт з Id {vehicle.Id} не був знайдений або не був оновлений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при оновленні транспорту: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Vehicle GetById(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                Vehicle vehicle = null;

                if (reader.Read())
                {
                    string name = reader.GetString(1);
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
                Notify($"\n Знайдений транспортний засіб з Id {id}: \n");
                return vehicle;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні транспортного засобу за Id  {id}: {ex.Message}");
                throw new InvalidOperationException($"\n Транспортний засіб за Id '{id}' не знайдений.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Vehicle GetByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}

