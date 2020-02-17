using kaymak.Kaymak.Entities;
using Kaymak.Entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kaymak.Network {
    class Client {
        private NetClient client;
        private NetPeerConfiguration config;
        private World world;

        //private Dictionary<string, Player> players;
        private List<Player> players;
        public string username;

        public Client(World world, string username, string host, int port) {
            this.world = world;
            this.username = username;

            config = new NetPeerConfiguration("kaymak");
            client = new NetClient(config);
            players = new List<Player>();

            for (int i = 0; i < 16; i++) {
                players.Add(null);
            }

            NetOutgoingMessage approval = client.CreateMessage();
            approval.Write(username);
            approval.Write(100f);
            approval.Write(100f);

            client.Start();
            client.Connect(host, port, approval);
        }

        public void Update() {
            NetIncomingMessage inMsg;
            while ((inMsg = client.ReadMessage()) != null) {

                switch (inMsg.MessageType) {
                    case NetIncomingMessageType.Data:
                        string dataType = inMsg.ReadString();
                        if (dataType.Equals("newplayer")) {
                            Player p = new Player(world);

                            string username = inMsg.ReadString();
                            int id = inMsg.ReadInt32();

                            if (players[id] != null) {
                                Console.WriteLine("Aboooov   {0}     {1}", id, username);
                                break;
                            }

                            float x = inMsg.ReadFloat();
                            float y = inMsg.ReadFloat();

                            p.Position = new Vector2(x, y);
                            p.Username = username;
                            p.IsPrimary = false;
                            p.LoadContent();

                            world.entities.Add(p);
                            players[id] = p;
                            Console.WriteLine(players[id] + "  Abidin gubidin");

                        } else if (dataType.Equals("move")) {
                            string username = inMsg.ReadString();
                            int id = inMsg.ReadInt32();

                            if (players[id] != null) {
                                Player p = players[id];

                                float x = inMsg.ReadFloat();
                                float y = inMsg.ReadFloat();
                                int animNum = inMsg.ReadInt32();
                                p.CurAnim = p.GetAnimByAnimNum(animNum);
                                Vector2 newPos = new Vector2(x, y);

                                p.Position = newPos;

                            } else {
                                Console.WriteLine("Yok artık daha neler  :::::: {0} ,,,,,, {1}", username, id);
                            }
                        } else if (dataType.Equals("fireball")) {
                            int dir = inMsg.ReadInt32();
                            FireballDirection fbdir = dir == 0 ? FireballDirection.HORIZONTAL : FireballDirection.VERTICAL;
                            Fireball fb = new Fireball(world, fbdir);
                            fb.Position.X = inMsg.ReadInt32();
                            fb.Position.Y = inMsg.ReadInt32();

                            fb.LoadContent();
                            world.entities.Add(fb);

                        } else if (dataType.Equals("laser")) {
                            int dir = inMsg.ReadInt32();
                            LaserDirection ldir = dir == 0 ? LaserDirection.HORIZONTAL : LaserDirection.VERTICAL;
                            Laser l = new Laser(world, ldir);
                            l.Position.X = inMsg.ReadInt32();
                            l.Position.Y = inMsg.ReadInt32();

                            l.LoadContent();
                            world.entities.Add(l);
                        }
                        break;
                }
            }
            client.Recycle(inMsg);
        }

        public void SendMove(Player p) {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write("move");
            msg.Write(p.Username);
            msg.Write((float)p.Position.X);
            msg.Write((float)p.Position.Y);
            msg.Write((int)p.AnimNum);
            Console.WriteLine("Usrname : " + username);

            client.SendMessage(msg, NetDeliveryMethod.Unreliable);
        }
    }
}
