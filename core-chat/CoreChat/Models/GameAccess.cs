using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreChat.Models
{
	public class GameAccess : IGameAccess
	{
        string ID { get; set; }
        DateTime AccessTime { get; set; }
        int serverId { get; set; }
        int sx { get; set; }
        int sy { get; set; }
        int sz { get; set; }
        string UserEmail { get; set; }
        public static int AccessMembers;
        public GameAccess()
        {
            AccessMembers = 0;
        }
        public void AllocateSerer()
        {
            // after years ... I will make it ..
            serverId = 1;
        }

        public void Access()
        {
            GenerateLocation();
            AccessTime = DateTime.Now;
            AllocateSerer();
            AccessMembers++;
        }

        public void Logout()
        {
            AccessMembers--;
        }

        public void GenerateLocation()
        {
            Random random = new System.Random();
            sx = random.Next(15, 450);
            sy = random.Next(50, 120);
            sz = random.Next(15, 450);
        }

        public int Getsx()
        {
            return sx;
        }
        public int Getsy()
        {
            return sy;
        }
        public int Getsz()
        {
            return sz;
        }
        public int GetAccessMembers()
        {
            return AccessMembers;
        }
    }
}