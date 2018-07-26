using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreChat.Models;
using CoreChat.Filters;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text;

// http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreChat.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        public IUserRepository Users { get; set; }

        public UsersController(IUserRepository users)
        {
            Users = users;
        }

        [HttpGet("GetMembers")]
        public IActionResult GetUsers()
        {
            List<User> tmpUser = new List<User>();
            int num = 0;
            Users.GetUsers(ref tmpUser, ref num);
            var json = JsonConvert.SerializeObject(tmpUser);
            return Json(json);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUser register)
        {
            if (register == null || register.Password != register.Confirm)
            {
                return BadRequest();
            }


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "wonseoksample.database.windows.net";
            builder.UserID = "jws786";
            builder.Password = "Wonseok786!";
            builder.InitialCatalog = "mySampleDatabase";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("I will add a new member!");
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM dbo.users WHERE Name = '" +register.Name +"'");
                String sql = sb.ToString();
                Console.WriteLine("SQL IS : " + sql);

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Already exhists!");
                            return BadRequest();
                        }
                    }
                }
            }

            Console.WriteLine(register);
            var user = new User
            {
                Name = register.Name,
                Email = register.Email,
                Password = register.Password
            };

            // The repository adds an ID and Token, so we need a response from it.
            user = Users.Add(user);

            return Json(BuildUserResponse(user));
        }

        private object BuildUserResponse(string v)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] RegisterUser login)
        {
            var user = Users.FindByEmail(login.Email);

            if (user == null || login.Password != user.Password)
            {
                return new UnauthorizedResult();
            }

            return Json(BuildUserResponse(user));
        }

        [HttpGet("Me")]
        [BearerToken]
        public IActionResult ViewMe()
        {
            string token = User.FindFirst("token").Value;
            User user = Users.FindByToken(token);

            if (user == null)
            {
                return Unauthorized();
            }

            return Json(BuildUserResponse(user));
        }

        [HttpPut("Me")]
        [BearerToken]
        public IActionResult EditMe([FromBody] RegisterUser edit)
        {
            string token = User.FindFirst("token").Value;
            User user = Users.FindByToken(token);

            if (user == null)
            {
                return Unauthorized();
            }

            user.Name = edit.Name;
            user.Email = edit.Email;

            Users.Update(user);

            return Json(BuildUserResponse(user));
        }

        private object BuildUserResponse(User user)
        {
            return new
            {
                success = true,
                data = new
                {
                    id = user.ID,
                    token = user.Token,
                    email = user.Email,
                    name = user.Name
                }
            };
        }
    }
}
