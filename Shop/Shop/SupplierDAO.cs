using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class SupplierDAO : IDAO<Supplier>
    {
        private readonly string connectionString;
        private MySqlConnection connection;

        private const string GET_ALL_QUERY = "SELECT * FROM supplier";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM supplier WHERE FirstName = @FirstName";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM supplier WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE supplier SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO supplier (FirstName, LastName, Email, Phone) VALUES (@FirstName, @LastName, @Email, @Phone)";


        public SupplierDAO(string connectionString)
        {
            connection = DAOFactory.GetInstance().GetConnection();
        }

        public List<Supplier> GetAll()
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, connection);
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
            connection.Close();
            return suppliers;
        }

        public Supplier GetByName(string firstName)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, connection);
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
            connection.Close();
            return supplier;
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
                Console.WriteLine($"\n Постачальник з Id {id} був успішно видалений.\n");
            }
            else
            {
                Console.WriteLine($"\n Постачальник з Id {id} не був знайдений або не був видалений.\n");
            }
        }

        public void Add(Supplier supplier)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(INSERT_QUERY, connection);
            command.Parameters.AddWithValue("@FirstName", supplier.FirstName);
            command.Parameters.AddWithValue("@LastName", supplier.LastName);
            command.Parameters.AddWithValue("@Email", supplier.Email);
            command.Parameters.AddWithValue("@Phone", supplier.Phone);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine("\n Постачальник був успішно доданий.\n");
            }
            else
            {
                Console.WriteLine("\n Постачальник не був доданий.\n");
            }
        }

        public void Update(Supplier supplier)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(UPDATE_QUERY, connection);
            command.Parameters.AddWithValue("@Id", supplier.Id);
            command.Parameters.AddWithValue("@FirstName", supplier.FirstName);
            command.Parameters.AddWithValue("@LastName", supplier.LastName);
            command.Parameters.AddWithValue("@Email", supplier.Email);
            command.Parameters.AddWithValue("@Phone", supplier.Phone);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"\n Постачальник з Id {supplier.Id} був успішно оновлений.\n");
            }
            else
            {
                Console.WriteLine($"\n Постачальник з Id {supplier.Id} не був знайдений або не був оновлений.\n");
            }
        }
    }
}
