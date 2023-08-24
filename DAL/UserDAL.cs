using Login_RegisterFormSession.Models;
using System.Data.SqlClient;

namespace Login_RegisterFormSession.DAL
{
    public class UserDAL
    {
        private IConfiguration Configuration { get; set; }
        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("[dbo].[sp_Select_User]", connection);
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
                        user.id = Convert.ToInt32(dr["id"].ToString());
                        user.email = dr["email"].ToString();
                        user.password = dr["password"].ToString();
                        user.username = dr["username"].ToString();
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
            return user;
        }

        public bool CreateUser(User user)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("[dbo].[sp_Create_User]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@email", user.email);
            command.Parameters.AddWithValue("@password", user.password);
            command.Parameters.AddWithValue("@username", user.username);

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
    }
}
