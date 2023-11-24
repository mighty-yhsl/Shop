using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public  class ManufacturerDAO : IDAO<Manufacturer>
    {
        private readonly string connectionString;
        private MySqlConnection connection;

        private const string GET_ALL_QUERY = "SELECT * FROM manufacturer";
        private const string GET_BY_NAME_QUERY = "SELECT * FROM manufacturer WHERE Name = @Name";
        private const string DELETE_BY_ID_QUERY = "DELETE FROM manufacturer WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE manufacturer SET Name = @Name WHERE Id = @Id";
        private const string INSERT_QUERY = "INSERT INTO manufacturer (Name) VALUES (@Name)";


        public ManufacturerDAO(string connectionString)
        {
            connection = DAOFactory.GetInstance().GetConnection();
        }

        public List<Manufacturer> GetAll()
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_ALL_QUERY, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string Name = reader.GetString(1);

                Manufacturer manufacturer = new Manufacturer.ManufacturerBuilder()
                    .SetId(id)
                    .SetName(Name)
                    .Build();

                manufacturers.Add(manufacturer);
            }
            reader.Close();
            connection.Close();
            return manufacturers;
        }

        public Manufacturer GetByName(string Name)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(GET_BY_NAME_QUERY, connection);
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
            connection.Close();
            return manufacturer;
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
                Console.WriteLine($"\n Виробник з Id {id} був успішно видалений.\n");
            }
            else
            {
                Console.WriteLine($"\n Виробник з Id {id} не був знайдений або не був видалений.\n");
            }
        }

        public void Add(Manufacturer manufacturer)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(INSERT_QUERY, connection);
            command.Parameters.AddWithValue("@Id", manufacturer.Id);
            command.Parameters.AddWithValue("@Name", manufacturer.Name);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine("\n Виробник був успішно доданий.\n");
            }
            else
            {
                Console.WriteLine("\n Виробник не був доданий.\n");
            }
        }

        public void Update(Manufacturer manufacturer)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(UPDATE_QUERY, connection);
            command.Parameters.AddWithValue("@Id", manufacturer.Id);
            command.Parameters.AddWithValue("@Name", manufacturer.Name);

            int rowsAffected = command.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected > 0)
            {
                Console.WriteLine($"\n Виробник з Id {manufacturer.Id} був успішно оновлений.\n");
            }
            else
            {
                Console.WriteLine($"\n Виробник з Id {manufacturer.Id} не був знайдений або не був оновлений.\n");
            }
        }
    }
}
