using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreChat.Models
{
    public interface IUserRepository
    {

        User Add(User user);
        User FindByID(int id);
        User FindByToken(string token);
        User FindByEmail(string email);
        void GetUsers(ref List<User> user, ref int num);
        void Update(User user);
    }
}
