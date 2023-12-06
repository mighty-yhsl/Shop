using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shop
{
    public  class ManufacturerDAO : IDAO<Manufacturer>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;

        private const string GET_ALL_QUERY = "SELECT * FROM manufacturer";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM manufacturer WHERE Name = @Name";
        private const string GET_BY_ID_QUERY = "SELECT * FROM manufacturer WHERE Id = @Id";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM manufacturer WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE manufacturer SET Name = @Name WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO manufacturer (Name) VALUES (@Name)";

        public ManufacturerDAO(string connectionString)
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

        public List<Manufacturer> GetAll()
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, _connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<Manufacturer> manufacturers = new List<Manufacturer>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);

                    Manufacturer manufacturer = new Manufacturer.ManufacturerBuilder()
                        .SetId(id)
                        .SetName(name)
                        .Build();

                    manufacturers.Add(manufacturer);
                }
                reader.Close();
                Notify("\n Всі виробники: \n");
                return manufacturers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні виробників: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання виробників. Будь ласка," +
                    $" перевірте стан системи та спробуйте знову.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Manufacturer GetByName(string Name)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, _connection);
                command.Parameters.AddWithValue("@Name", Name);

                MySqlDataReader reader = command.ExecuteReader();
                Manufacturer manufacturer = null;

                if (reader.Read())
                {
                    int id = reader.GetInt32(0);

                    manufacturer = new Manufacturer.ManufacturerBuilder()
                        .SetId(id)
                        .SetName(Name)
                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдений виробник з ім'ям {Name}: \n");
                return manufacturer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні виробника за ім'ям {Name}: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання виробника за ім'ям. " +
                    $"Будь ласка, перевірте стан системи та спробуйте знову.");
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
                    Notify($"\n Виробник з Id {id} був успішно видалений.\n");
                }
                else
                {
                    Console.WriteLine($"\n Виробник з Id {id} не був знайдений або не був видалений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при видаленні виробника з Id {id}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Add(Manufacturer manufacturer)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", manufacturer.Id);
                command.Parameters.AddWithValue("@Name", manufacturer.Name);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Виробник з ім'ям {manufacturer.Name} був успішно доданий.\n");
                }
                else
                {
                    Console.WriteLine($"\n Виробник з ім'ям {manufacturer.Name} не був доданий.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при додаванні виробника з ім'ям {manufacturer.Name}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Manufacturer manufacturer)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", manufacturer.Id);
                command.Parameters.AddWithValue("@Name", manufacturer.Name);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Notify($"\n Виробник з Id {manufacturer.Id} був успішно оновлений.\n");
                }
                else
                {
                    Console.WriteLine($"\n Виробник з Id {manufacturer.Id} не був знайдений або не був оновлений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при оновленні виробника: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Manufacturer GetById(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                Manufacturer manufacturer = null;

                if (reader.Read())
                {
                    string name = reader.GetString(1);

                    manufacturer = new Manufacturer.ManufacturerBuilder()
                    .SetId(id)
                    .SetName(name)
                    .Build();
                }
                reader.Close();
                Notify($"\n Знайдений виробник з Id {id}: \n");
                return manufacturer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні виробника за Id {id}: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання виробника за Id. " +
                    $"Будь ласка, перевірте стан системи та спробуйте знову.");
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
