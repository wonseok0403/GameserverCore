using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreChat.Models
{
    public interface IGameAccess
    {
        void GenerateLocation();
        void Access();
        void AllocateSerer();
        void Logout();
        int GetAccessMembers();
    }
}