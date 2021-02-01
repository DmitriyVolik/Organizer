using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{
    class Database
    {

        private string _connectionString;


        public Database(string connectionString)
        {
            _connectionString = connectionString;

        }

        public List<TaskViewModel> GetNews(string header = null)
        {
            List<TaskViewModel> result = new List<TaskViewModel>();

            string sqlExpression;

            sqlExpression = "SELECT * FROM Tasks";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        TaskViewModel temp = new TaskViewModel();

                        temp.Name = reader.GetValue(1).ToString();

                        temp.Description = reader.GetValue(2).ToString();
                        
                        temp.Date = Convert.ToDateTime(reader.GetValue(3));
                        
                        temp.IsComplete = Convert.ToBoolean(reader.GetValue(4));
                       
                        

                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;

            // InsertToDb(new News() { Header = "Новый заголовок", Text = "Длинный текстттт", Url = "Адрес", Date = "13-05-1997" });
        }
    }

}
