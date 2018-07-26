using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreChat.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreChat.Controllers
{
    [Route("[controller]")]
    public class GameAccessController : Controller
    {
        // GET: api/<controller>
        public GameAccess GameAccesse = new GameAccess();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {
                "Game access server is online."
            };
        }

        [HttpGet("GetVector/{mail}")]
        public IActionResult GetVector(string mail)
        {
            mail = $"{mail}";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            StringBuilder sb = new StringBuilder();
            int _x=-1, _y=-1, _z=-1;
            sb.Append("SELECT STARTX, STARTY, STARTZ FROM dbo.GameStatus WHERE Email = '" + mail + "'");
            Console.WriteLine(sb);
            Console.WriteLine(mail);
            Console.WriteLine("--------------------------------");
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                String sql = sb.ToString();
                Console.WriteLine("SQL IS : " + sql);
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if( reader.HasRows)
                {
                    if (reader.Read())
                    {
                        _x = reader.GetInt32(0);
                        _y = reader.GetInt32(1);
                        _z = reader.GetInt32(2);

                    }
                }
            }
            return Json(new
            {
                x = _x,
                y = _y,
                z = _z
            });
        }


        // GET api/<controller>/5
        [HttpGet("GetAccessMembers")]
        public IActionResult GetAccessMembers()
        {
            var json = JsonConvert.SerializeObject(GameAccess.AccessMembers);
            return Json(json);
        }
        
        public Boolean CheckExist(string mail)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("I'll check that mail is in DB");
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM dbo.GameStatus WHERE EMAIL = '" + mail +"'");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Already exhists!");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // POST api/<controller>
        [HttpPost("AccessServer")]
        public IActionResult AccessServer([FromBody]RegisterUser value)
        {
            string userMail = value.Email;
            // Server process
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            StringBuilder sb = new StringBuilder();
            GameAccesse.GenerateLocation();
            int sx, sy, sz;
            sx = GameAccesse.Getsx();
            sy = GameAccesse.Getsy();
            sz = GameAccesse.Getsz();
            string InsertCommand = "( 1, '"+ userMail + "' ,"  + sx.ToString() + ", "  + sy.ToString() + ", " + sz.ToString() + ")";
            if (CheckExist(value.Email))
            {
                sb.Append("UPDATE dbo.GameStatus SET STARTX=" + sx.ToString() + ", STARTY=" + sy.ToString() + ", STARTZ=" + sz.ToString() + " WHERE EMAIL='" + value.Email +"'");
            }
            else
            {
                sb.Append("INSERT INTO dbo.GameStatus VALUES " + InsertCommand);
            }
            String sql = sb.ToString();
            Console.WriteLine("SQL : " + sql);

            using( SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("Game acceess starts ....");
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("It has problem!");
                            return BadRequest();
                        }
                    }
                }
            }
            return Json("OK!");
        }
        

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
