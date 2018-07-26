using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Text;

namespace CoreChat
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            try
            {
                /*
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "wonseoksample.database.windows.net";
                builder.UserID = "jws786";
                builder.Password = "Wonseok786!";
                builder.InitialCatalog = "mySampleDatabase";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\n Query data Example : ");
                    Console.WriteLine("===================================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("CREATE TABLE test ( testID int )");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}, {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }*/
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }/*
            Console.ReadLine();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();*/
        }
    }

}
