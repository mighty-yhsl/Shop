using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class ProxySupplierDAO : IDAO<Supplier>
    {
        private MySqlConnection _connection;
        private List<IObserver> _observers;
        private SupplierType _supplierType;
        private SupplierDAO _supplierDAO;

        public ProxySupplierDAO(string connectionString, SupplierType supplierType)
        {
            _connection = DAOFactory.GetInstance().GetConnection();
            _observers = new List<IObserver>();
            _supplierDAO = new SupplierDAO(connectionString);
            _supplierType = supplierType;
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
           return _supplierDAO.GetAll();
        }

        public Supplier GetByName(string firstName)
        {
           return _supplierDAO.GetByName(firstName);
        }

        public void Delete(int id)
        {
            if (_supplierType == SupplierType.PRIMARY)
            {
                _supplierDAO.Delete(id);
            }
            else
            {
                Console.WriteLine("Your type is not Primary. You can't delete suppliers");
            }
        }

        public void Add(Supplier supplier)
        {
            if (_supplierType == SupplierType.PRIMARY)
            {
                _supplierDAO.Add(supplier);
            }
            else 
            {
                Console.WriteLine("Your type is not Primary. You can't add suppliers");
            }
        }

        public void Update(Supplier supplier)
        {
            if (_supplierType == SupplierType.PRIMARY)
            {
                _supplierDAO.Update(supplier);
            }
            else
            {
                Console.WriteLine("Your type is not Primary. You can't update suppliers");
            }
        }

        public Supplier GetById(int id)
        {
            return _supplierDAO.GetById(id);
        }
    }
}
