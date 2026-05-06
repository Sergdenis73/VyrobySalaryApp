using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace VyrobySalaryApp
{
    public class DataAccess
    {
        private string connStr = "Server=localhost;Database=vyroby_salary_db;Uid=root;Pwd=Sergdenis@73;";

        public ObservableCollection<WorkerProduct> LoadWorkersProducts()
        {
            ObservableCollection<WorkerProduct> list = new ObservableCollection<WorkerProduct>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = "SELECT id, surname, workshop_number, product_a, product_b, product_c FROM workers_products;";

                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    WorkerProduct worker = new WorkerProduct();

                    worker.id = reader.GetInt32("id");
                    worker.surname = reader.GetString("surname");
                    worker.workshop_number = reader.GetInt32("workshop_number");
                    worker.product_a = reader.GetInt32("product_a");
                    worker.product_b = reader.GetInt32("product_b");
                    worker.product_c = reader.GetInt32("product_c");

                    list.Add(worker);
                }

                reader.Close();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Не вдалося підключитися до бази даних MySQL.");
            }

            return list;
        }
    }
}