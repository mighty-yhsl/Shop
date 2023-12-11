using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class UsersDAO : IDAO<Users>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;

        private const string GET_ALL_QUERY = "SELECT * FROM users";
        private const string GET_BY_ID_QUERY = "SELECT * FROM users WHERE Id = @Id";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM users WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE users SET Password = @Password, Login = @Login, Roles_Id = @Roles_Id WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO users (Password, Login, Roles_Id) VALUES (@Password, @Login, @Roles_Id)";
        private const string GET_BY_LOGIN_QUERY = "SELECT * FROM users WHERE Login = @Login";


        public UsersDAO(string connectionString)
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

        public List<Users> GetAll()
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, _connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<Users> users = new List<Users>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string password = reader.GetString(1);
                    string login = reader.GetString(2);
                    int rolesId = reader.GetInt32(3);

                    Users user = new Users.UsersBuilder()
                        .SetId(id)
                        .SetPassword(password)
                        .SetLogin(login)
                        .SetRolesId(rolesId)
                        .Build();

                    users.Add(user);
                }
                reader.Close();
                Notify("\n Всі користувачі: \n");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні користувачів: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання користувачів." +
                    $" Будь ласка, перевірте стан системи та спробуйте знову.");
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
                    Notify($"\n Користувач з Id {id} був успішно видалений.\n");
                }
                else
                {
                    Notify($"\n Користувач з Id {id} не був знайдений або не був видалений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при видаленні користувача з Id {id}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Add(Users users)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_QUERY, _connection);
                command.Parameters.AddWithValue("@Password", users.Password);
                command.Parameters.AddWithValue("@Login", users.Login);
                command.Parameters.AddWithValue("@Roles_Id", users.RolesId);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Користувач з Id {users.Id} був успішно доданий.\n");
                }
                else
                {
                    Notify($"\n Користувач з Id {users.Id} не був доданий.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при додаванні користувача: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Users users)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", users.Id);
                command.Parameters.AddWithValue("@Password", users.Password);
                command.Parameters.AddWithValue("@Login", users.Login);
                command.Parameters.AddWithValue("@Roles_Id", users.RolesId);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Користувач з Id {users.Id} був успішно оновлений.\n");
                }
                else
                {
                    Notify($"\n Користувач з Id {users.Id} не був знайдений або не був оновлений.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при оновленні користувача: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Users GetById(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                Users user = null;

                if (reader.Read())
                {
                    string password = reader.GetString(1);
                    string login = reader.GetString(2);
                    int rolesId = reader.GetInt32(3);

                    user = new Users.UsersBuilder()
                        .SetId(id)
                        .SetPassword(password)
                        .SetLogin(login)
                        .SetRolesId(rolesId)
                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдений користувач з Id {id}: \n");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні користувача з Id {id}: {ex.Message}");
                throw new InvalidOperationException($"\n Користувач з Id '{id}' не знайдений.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Users GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Users GetByLogin(string login)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_LOGIN_QUERY, _connection);
                command.Parameters.AddWithValue("@Login", login);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Users user = null;

                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string password = reader.GetString(1);
                        int rolesId = reader.GetInt32(3);

                        user = new Users.UsersBuilder()
                            .SetId(id)
                            .SetPassword(password)
                            .SetLogin(login)
                            .SetRolesId(rolesId)
                            .Build();
                    }

                    Notify($"\n Знайдений користувач з логіном {login}: \n");
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні користувача з логіном {login}: {ex.Message}");
                throw new InvalidOperationException($"\n Користувач з логіном '{login}' не знайдений.");
            }
            finally
            {
                _connection.Close();
            }
        }
       
    }
}
