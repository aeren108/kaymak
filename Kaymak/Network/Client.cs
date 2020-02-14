using Kaymak.Entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaymak.Network {
    class Client {
        private NetClient client;
        private NetPeerConfiguration config;
        private World world;

        public Client(World world, string host, int port) {
            this.world = world;

            config = new NetPeerConfiguration("kaymak");
            client = new NetClient(config);

            NetOutgoingMessage approval = client.CreateMessage();
            approval.Write("Username");

            client.Start();
            client.Connect(host, port, approval);
        }

        public void Update(GameTime gameTime) {
            NetIncomingMessage inMsg = null;

            if ((inMsg = client.ReadMessage()) != null) {

                switch(inMsg.MessageType) {
                    case NetIncomingMessageType.Data:
                        string dataType = inMsg.ReadString();
                        if (dataType.Equals("newplayer")) {
                            Player p = new Player(world);
                            
                            float x = inMsg.ReadFloat();
                            float y = inMsg.ReadFloat();
                            p.Position = new Vector2(x, y);

                            world.entities.Add(p);
                        }
                        break;
                }

            }
        }

    }
}
