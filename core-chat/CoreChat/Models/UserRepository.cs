using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreChat.Models
{
    public class UserRepository : IUserRepository
    {
        private int _id;
        private static List<User> _users = new List<User>();

        // Constructor
        public UserRepository()
        {
            // User num update
            UpdateUsernum();
            // log -> // Console.WriteLine("Constructor : "+_id);
            int t_id;
            String t_name, t_mail, t_password, t_token;
            for (int i=1; i<=_id; i++)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "wonseoksample.database.windows.net";
                builder.UserID = "jws786";
                builder.Password = "Wonseok786!";
                builder.InitialCatalog = "mySampleDatabase";
                // DB Load end;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder SB = new StringBuilder();
                    SB.Append("SELECT * FROM dbo.users WHERE ID = " + i);
                    String sql = SB.ToString();
                    SqlCommand sqlCommand = new SqlCommand(sql, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            t_id = reader.GetInt32(0);
                            t_name = reader.GetString(1);
                            t_mail = reader.GetString(2);
                            t_password = reader.GetString(3);
                            t_token = reader.GetString(4);
                            var t_user = new User();
                            t_user.ID = t_id;
                            t_user.Name = t_name;
                            t_user.Email = t_mail;
                            t_user.Password = t_password;
                            t_user.Token = t_token;
                            _users.Add(t_user);
                        }
                    }

                }
            }
        }
        public void UpdateUsernum()
        {
            // DB Load start;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            // DB Load end;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder SB = new StringBuilder();
                SB.Append("SELECT COUNT(*) FROM dbo.users");
                String sql = SB.ToString();
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        Console.WriteLine(reader);
                        _id = reader.GetInt32(0);
                        Console.WriteLine("{0}", _id);
                    }
                }

            }
        }
        public User Add(User user)
        {

            // User setting start
            user.ID = _id;
            user.Token = Guid.NewGuid().ToString();
            _users.Add(user);
            _id++;  // _id must be usernum when constructor is called;
                    // User Setting end

            // DB access start
            // * 1. Get a usernum.
            UpdateUsernum();
            // * 2. _Id + 1
            _id++;
            // * 3. Insert new user.
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            //     - DB Load end;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder SB = new StringBuilder();
                SB.Append("INSERT INTO dbo.users VALUES ( " + (_id) + ", '" + user.Name + "', '" + user.Email + "', '" + user.Password + "', '" + user.Token + "' )");
                String sql = SB.ToString();
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        Console.WriteLine(reader);
                        Console.WriteLine("{0}", reader.GetInt32(0));
                    }
                }

            }
            return user;
        }

        public User FindByID(int id)
        {
            return _users.Find(x => x.ID == id);
        }

        public User FindByToken(string token)
        {
            return _users.Find(x => x.Token == token);
        }

        public User FindByEmail(string email)
        {
            return _users.Find(x => x.Email == email);
        }

        public void Update(User user)
        {
            var storedUser = _users.Find(x => x.Email == user.Email);
            storedUser = user;
        }
        public void GetUsers(ref List<User> user, ref int num)
        {
            int i = 0;
            for( i=0; i<_users.Count; i++)
            {
                user.Add(_users[i]);
            }
            num = _users.Count;
            return;
        }
    }
}
