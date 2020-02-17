using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server {
    class Player {
        public string Username;
        public int id = 0;
        public float[] Position;
        public int AnimNum = 0;
        public NetConnection conn;

        public Player(string username, int id) {
            Username = username;
            Position = new float[2];
            this.id = id;
        }
    }
}
