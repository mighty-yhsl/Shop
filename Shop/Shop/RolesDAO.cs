using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class RolesDAO : IDAO<Roles>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;

        private const string GET_ALL_QUERY = "SELECT * FROM roles";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM roles WHERE RolesName = @Roles_Name";
        private const string GET_BY_ID_QUERY = "SELECT * FROM roles WHERE Id = @Id";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM roles WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE roles SET RolesName = @Roles_Name WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO roles (RolesName) VALUES (@Roles_Name)";


        public RolesDAO(string connectionString)
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

        public List<Roles> GetAll()
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, _connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<Roles> roles = new List<Roles>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string rolesName = reader.GetString(1);

                    Roles role = new Roles.RolesBuilder()
                        .SetId(id)
                        .SetRolesName(rolesName)
                        .Build();

                    roles.Add(role);
                }
                reader.Close();
                Notify("\n Всі ролі користувача: \n");
                return roles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні ролей користувача: {ex.Message}");
                throw new InvalidOperationException($"\n Неможливо виконати операцію отримання ролей користувачів." +
                    $" Будь ласка, перевірте стан системи та спробуйте знову.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Roles GetByName(string RolesName)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, _connection);
                command.Parameters.AddWithValue("@Roles_Name", RolesName);

                MySqlDataReader reader = command.ExecuteReader();
                Roles role = null;

                if (reader.Read())
                {
                    int id = reader.GetInt32(0);

                    role = new Roles.RolesBuilder()
                        .SetId(id)
                        .SetRolesName(RolesName)

                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдена роль з назвою {RolesName}: \n");
                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні ролі з назвою {RolesName}: {ex.Message}");
                throw new InvalidOperationException($"\n Роль з назвою '{RolesName}' не знайдена.");
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
                    Notify($"\n Роль з Id {id} була успішно видалена.\n");
                }
                else
                {
                    Notify($"\n Роль з Id {id} не була знайдена або не була видалена.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при видаленні ролі з Id {id}: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Add(Roles roles)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_QUERY, _connection);
                command.Parameters.AddWithValue("@Roles_Name", roles.RolesName);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Роль з назвою {roles.RolesName} була успішно додана.\n");
                }
                else
                {
                    Notify($"\n Роль з назвою {roles.RolesName} не була додана.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при додаванні ролі: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Roles roles)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", roles.Id);
                command.Parameters.AddWithValue("@Roles_Name", roles.RolesName);

                int rowsAffected = command.ExecuteNonQuery();

                _connection.Close();

                if (rowsAffected > 0)
                {
                    Notify($"\n Роль з Id {roles.Id} була успішно оновлена.\n");
                }
                else
                {
                    Notify($"\n Роль з Id {roles.Id} не була знайдена або не була оновлена.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при оновленні ролі: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Roles GetById(int id)
        {
            try
            {
                _connection.Open();
                MySqlCommand command = new MySqlCommand(GET_BY_ID_QUERY, _connection);
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                Roles role = null;

                if (reader.Read())
                {
                    string RolesName = reader.GetString(1);

                    role = new Roles.RolesBuilder()
                        .SetId(id)
                        .SetRolesName(RolesName)
                        .Build();
                }
                reader.Close();
                Notify($"\n Знайдена роль з Id {id}: \n");
                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify($"\n Помилка при отриманні ролі з Id {id}: {ex.Message}");
                throw new InvalidOperationException($"\n Роль з Id '{id}' не знайдена.");
            }
            finally
            {
                _connection.Close();
            }
        }

        public Roles GetByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
