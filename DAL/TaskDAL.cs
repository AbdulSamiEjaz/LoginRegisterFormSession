using Login_RegisterFormSession.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Login_RegisterFormSession.DAL
{
    public class TaskDAL
    {
        private IConfiguration Configuration { get; set; }
        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<TaskModel> FetchAllTasks(string email)
        {

            List<TaskModel> tasks = new List<TaskModel>();

            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("[dbo].[sp_Get_All_Tasks]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@email", email);

            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        TaskModel task = new TaskModel();
                        task.id = Convert.ToInt32(dr["id"].ToString());
                        task.title = dr["title"].ToString();
                        task.desctiption = dr["desctiption"].ToString();
                        task.userId = Convert.ToInt32(dr["userId"].ToString());
                        tasks.Add(task);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return tasks;
        }

        public bool CreateTask(TaskModel task)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("[dbo].[sp_Create_Task]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@title", task.title);
            command.Parameters.AddWithValue("@desctiption", task.desctiption);
            command.Parameters.AddWithValue("@userId", task.userId);

            try
            {
                using (connection)
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public bool UpdateTask(TaskModel task)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("[dbo].[sp_Update_Task]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@id", task.id);
            command.Parameters.AddWithValue("@title", task.title);
            command.Parameters.AddWithValue("@desctiption", task.desctiption);
            command.Parameters.AddWithValue("@userId", task.userId);

            try
            {
                using (connection)
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public TaskModel FindTaskById(int taskId)
        {
            TaskModel task = new TaskModel();

            SqlConnection connection = new SqlConnection(GetConnectionString());

            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("select * from Tasks where id = " + taskId, connection);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        task.id = Convert.ToInt32(dr["id"].ToString());
                        task.title = dr["title"].ToString();
                        task.desctiption = dr["desctiption"].ToString();
                        task.userId = Convert.ToInt32(dr["userId"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return task;
        }

        public bool DeleteTask(int taskId, int userId)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(GetConnectionString());

            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from Tasks where id = {taskId} and  userId = {userId}", connection);

                    rowsAffected = command.ExecuteNonQuery();
                   
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0; 
        }
    }
}
