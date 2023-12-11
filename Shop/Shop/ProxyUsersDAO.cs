using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class ProxyUsersDAO : IDAO<Users>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;
        private UsersDAO _userDAO;
        private RolesDAO _roleDAO;

        public ProxyUsersDAO(string connectionString)
        {
            _connection = DAOFactory.GetInstance().GetConnection();
            _observers = new List<IObserver>();
            _userDAO = new UsersDAO(connectionString);
            _roleDAO = new RolesDAO(connectionString);
        }


        public void AddObserver(IObserver observer)
        {
            _userDAO.AddObserver(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _userDAO.RemoveObserver(observer);
        }

        public void Notify(string message)
        {
            _userDAO.Notify(message);
        }

        public List<Users> GetAll()
        {
           return _userDAO.GetAll();
        }
        
        public void Delete(int id)
        {
            Users userFromDB = _userDAO.GetById(id);

            if (userFromDB != null)
            {
                Roles userRole = _roleDAO.GetById(userFromDB.RolesId);

                if (userRole != null && userRole.RolesName == "Admin")
                {
                    _userDAO.Delete(id);
                }
                else
                {
                    Console.WriteLine("\n Ви не можете оновити користувача, оскільки ваша роль не Admin.\n");
                }
            }
            else
            {
                Console.WriteLine("\n Користувач не знайдений.\n");
            }
        }

        public void Add(Users users)
        {
            if (users.RolesId == 1 || users.RolesId == 2)
            {
                _userDAO.Add(users);
            }
            else
            {
                Console.WriteLine("\n Ви не можете додати користувача, оскільки ваша роль не Admin.\n");
            }
        }

        public void Update(Users users)
        {
            Users userFromDB = _userDAO.GetById(users.Id);

            if (userFromDB != null)
            {
                Roles userRole = _roleDAO.GetById(userFromDB.RolesId);

                if (userRole != null && userRole.RolesName == "Admin")
                {
                    _userDAO.Update(users);
                }
                else
                {
                    Console.WriteLine("\n Ви не можете оновити користувача, оскільки ваша роль не Admin.\n");
                }
            }
            else
            {
                Console.WriteLine("\n Користувач не знайдений.\n");
            }
        }

        public Users GetById(int id)
        {
            return _userDAO.GetById(id);
        }

        public Users GetByName(string name)
        {
            return _userDAO.GetByName(name);
        }

        public Users GetByLogin(string login)
        {
           return _userDAO.GetByLogin(login);
        }
    }
    public class UserContext
    {
        public static Users CurrentUser { get; set; }
    }
}
