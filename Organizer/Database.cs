using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//jkdfgkjdfg
namespace Organizer
{
    class Database
    {

        private string _connectionString;


        public Database(string connectionString)
        {
            _connectionString = connectionString;

        }

        public List<TaskViewModel> GetTasks()
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
                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        temp.Name = reader.GetValue(1).ToString();

                        temp.Description = reader.GetValue(2).ToString();

                        var rawDate = reader.GetValue(3);
                        if (DBNull.Value.Equals(rawDate))
                        {
                            temp.Date = null;
                        } else
                        {
                            temp.Date = Convert.ToDateTime(rawDate);
                        }

                        temp.IsComplete = Convert.ToBoolean(reader.GetValue(4));

                        temp.Id = Convert.ToInt32(reader.GetValue(0));

                        result.Add(temp);
                    }
                }

                reader.Close();
            }

            return result;
        }


        public void Save(TaskViewModel task)
        {
            if (task.Id == null)
            {
                Insert(task);
            } else
            {
                Update(task);
            }
        }

        public void Update(TaskViewModel task)
        {
           
            string sqlExpression = "UPDATE Tasks SET Name=@name, Description=@description, Date=@date, IsComplete=@iscomplete Where id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // создаем параметр для имени

                command.Parameters.Add(new SqlParameter("@id", task.Id));

                command.Parameters.Add(new SqlParameter("@name", (object)task.Name ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@description", (object)task.Description ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@date", (object)task.Date ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@iscomplete", task.IsComplete));

                command.ExecuteNonQuery();
            }
        }

        public void Delete(TaskViewModel task)
        {

            string sqlExpression = "DELETE FROM Tasks Where id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // создаем параметр для имени

                command.Parameters.Add(new SqlParameter("@id", task.Id));

                command.ExecuteNonQuery();
            }
        }

        public void Insert(TaskViewModel task)
        {
            string sqlExpression = "INSERT INTO Tasks (Name, Description, Date, IsComplete) VALUES (@name, @description, @date, @iscomplete); SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // создаем параметр для имени

                command.Parameters.Add(new SqlParameter("@name", (object)task.Name ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@description", (object)task.Description ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@date", (object)task.Date ?? DBNull.Value));

                command.Parameters.Add(new SqlParameter("@iscomplete", task.IsComplete));

                int insertedID = Convert.ToInt32(command.ExecuteScalar());

                task.Id = insertedID;
            }
        }
    }

}
