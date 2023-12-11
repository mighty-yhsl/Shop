using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shop
{
    public class SupplierDAO : IDAO<Supplier>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;

        private const string GET_ALL_QUERY = "SELECT * FROM supplier";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM supplier WHERE FirstName = @FirstName";
        private const string GET_BY_ID_QUERY = "SELECT * FROM supplier WHERE Id = @Id";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM supplier WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE supplier SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO supplier (FirstName, LastName, Email, Phone) VALUES (@FirstName, @LastName, @Email, @Phone)";


        public SupplierDAO(string connectionString)
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

        public List<Supplier> GetAll()
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, _connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<Supplier> suppliers = new List<Supplier>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string firstName = reader.GetString(1);
                    string lastName = reader.GetString(2);
                    string email = reader.GetString(3);
                    string phone = reader.GetString(4);

                    Supplier supplier = new Supplier.SupplierBuilder()
                        .SetId(id)
                        .SetFirstName(firstName)
                        .SetLastName(lastName)
                        .SetEmail(email)
                        .SetPhone(phone)
                        .Build();

                    suppliers.Add(supplier);
                }
                reader.Close();
                Notify("\n Всі постачальники: \n");
                return suppliers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні постачальників: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання постачальників." +
                    $" Будь ласка, перевірте стан системи та спробуйте знову.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Supplier GetByName(string firstName)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, _connection);
                command.Parameters.AddWithValue("@FirstName", firstName);

                MySqlDataReader reader = command.ExecuteReader();
                Supplier supplier = null;

                if (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string lastName = reader.GetString(1);
                    string email = reader.GetString(2);
                    string phone = reader.GetString(3);

                    supplier = new Supplier.SupplierBuilder()
                        .SetId(id)
                        .SetFirstName(firstName)
                        .SetLastName(lastName)
                        .SetEmail(email)
                        .SetPhone(phone)
                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдений постачальник з ім'ям {firstName}: \n");
                return supplier;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні постачальника з ім'ям {firstName}: {ex.Message}");
                throw new InvalidOperationException($"\n Постачальник з ім'ям '{firstName}' не знайдений.");
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

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Постачальник з Id {id} був успішно видалений.\n");
                }
                else
                {
                    Notify($"\n Постачальник з Id {id} не був знайдений або не був видалений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при видаленні постачальника з Id {id}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Add(Supplier supplier)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_QUERY, _connection);
                command.Parameters.AddWithValue("@FirstName", supplier.FirstName);
                command.Parameters.AddWithValue("@LastName", supplier.LastName);
                command.Parameters.AddWithValue("@Email", supplier.Email);
                command.Parameters.AddWithValue("@Phone", supplier.Phone);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Постачальник з ім'ям {supplier.FirstName} був успішно доданий.\n");
                }
                else
                {
                    Notify($"\n Постачальник з ім'ям {supplier.FirstName} не був доданий.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при додаванні постачальника: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Supplier supplier)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", supplier.Id);
                command.Parameters.AddWithValue("@FirstName", supplier.FirstName);
                command.Parameters.AddWithValue("@LastName", supplier.LastName);
                command.Parameters.AddWithValue("@Email", supplier.Email);
                command.Parameters.AddWithValue("@Phone", supplier.Phone);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Постачальник з Id {supplier.Id} був успішно оновлений.\n");
                }
                else
                {
                    Notify($"\n Постачальник з Id {supplier.Id} не був знайдений або не був оновлений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при оновленні постачальника: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Supplier GetById(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                Supplier supplier = null;

                if (reader.Read())
                {
                    string firstName = reader.GetString(1);
                    string lastName = reader.GetString(2);
                    string email = reader.GetString(3);
                    string phone = reader.GetString(4);

                    supplier = new Supplier.SupplierBuilder()
                        .SetId(id)
                        .SetFirstName(firstName)
                        .SetLastName(lastName)
                        .SetEmail(email)
                        .SetPhone(phone)
                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдений постачальник з Id {id}: \n");
                return supplier;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні постачальника з Id {id}: {ex.Message}");
                throw new InvalidOperationException($"\n Постачальник з Id '{id}' не знайдений.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Supplier GetByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
