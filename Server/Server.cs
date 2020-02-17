using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Server {
   
    class Server {
        private NetServer server;
        private NetPeerConfiguration config;

        private Player[] players;
        private System.Timers.Timer laserTimer;
        private System.Timers.Timer fireballTimer;

        private Random random = new Random();
        private Thread thread;
        private NetIncomingMessage inMsg;
        public event EventHandler Status;

        int nextId = 0;
        private bool running;

        public Server() {
            config = new NetPeerConfiguration("kaymak");
            config.Port = 2020;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);

            players = new Player[16];

            laserTimer = new System.Timers.Timer(1500);
            laserTimer.AutoReset = true;
            fireballTimer = new System.Timers.Timer(200);
            fireballTimer.AutoReset = true;

            laserTimer.Elapsed += TimerEvent;
            fireballTimer.Elapsed += TimerEvent;

            thread = new Thread(new ThreadStart(ListenMessages));
        }

        public void ListenMessages() {
            while (running) {
                if ((inMsg = server.ReadMessage()) != null) {
                    switch (inMsg.MessageType) {
                        case NetIncomingMessageType.ConnectionApproval:
                            string username = inMsg.ReadString();
                            Player p = FindPlayerByName(username);
                            if (p == null) {
                                float x = inMsg.ReadFloat();
                                float y = inMsg.ReadFloat();

                                p = new Player(username, nextId);
                                p.Position[0] = x; p.Position[1] = y;

                                inMsg.SenderConnection.Approve();
                                p.conn = inMsg.SenderConnection;

                                players[nextId] = p;
                                p.id = nextId;

                                Status?.Invoke(this, new StatusEventArgs(string.Format("{0} has joined with ID: {1}, IP: {2}", username, nextId, inMsg.SenderEndPoint)));

                                SendPlayer(p);

                                foreach (Player pl in players) {
                                    if (pl != null && pl != p) {
                                        SendPlayer(pl, p);
                                    }
                                }

                                Console.WriteLine("New Player: " + p.Username);

                                nextId++;
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            string dataType = inMsg.ReadString();

                            if (dataType.Equals("move")) {
                                string usrname = inMsg.ReadString();
                                Player pl = FindPlayerByName(usrname);

                                if (pl != null) {
                                    float x = inMsg.ReadFloat();
                                    float y = inMsg.ReadFloat();
                                    pl.Position[0] = x;
                                    pl.Position[1] = y;

                                    SendMove(pl);
                                } else {
                                    string infoText = string.Format("Player doesn't exist: [username: {0}]", usrname);
                                    Status?.Invoke(this, new StatusEventArgs(infoText));
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void Run() {
            server.Start();
            
            laserTimer.Enabled = true;
            fireballTimer.Enabled = true;

            running = true;
            thread.Start();

            Status?.Invoke(this, new StatusEventArgs("Server has started..."));
        }

        private void TimerEvent(object source, ElapsedEventArgs args) {
            if (source == fireballTimer) {
                for (int i = 0; i < 2; i++) {
                    int dir = random.Next(0, 2);
                    AddFireball(dir);
                }
            } else if (source == laserTimer) {
                for (int i = 0; i < 2; i++) {
                    int dir = random.Next(0, 2);
                    AddLaser(dir);
                }
            }
        }

        private void SendPlayer(Player p1, Player p2) {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("newplayer");
            msg.Write(p1.Username);
            msg.Write(p1.id);
            msg.Write(p1.Position[0]);
            msg.Write(p1.Position[1]);

            Console.WriteLine("krakentrougly");
            server.SendMessage(msg, p2.conn, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendPlayer(Player p) {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("newplayer");
            msg.Write(p.Username);
            msg.Write(p.id);
            msg.Write(p.Position[0]);
            msg.Write(p.Position[1]);

            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendMove(Player p) {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("move");
            msg.Write(p.Username);
            msg.Write(p.id);
            msg.Write(p.Position[0]);
            msg.Write(p.Position[1]);
            msg.Write(p.AnimNum);

            server.SendToAll(msg, NetDeliveryMethod.Unreliable);
        }

        private void AddFireball(int direction) {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("fireball");
            msg.Write(direction);

            if (direction == 0) {
                //horizontal
                float x = 0;
                float y = random.Next(4, 64) * 32;

                msg.Write(x);
                msg.Write(y);

            } else if (direction == 1) {
                //vertical

                float x = random.Next(4, 80) * 32;
                float y = 0;

                msg.Write(x);
                msg.Write(y);
            }

            server.SendToAll(msg, NetDeliveryMethod.Unreliable);
        }

        private void AddLaser(int direction) {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write("laser");
            msg.Write(direction);

            if (direction == 0) {
                //horizontal
                float x = 0;
                float y = random.Next(0, 64) * 32;

                msg.Write(x);
                msg.Write(y);

            }
            else if (direction == 1) {
                //vertical

                float x = 0;
                float y = random.Next(0, 80) * 32;

                msg.Write(x);
                msg.Write(y);
            }

            server.SendToAll(msg, NetDeliveryMethod.Unreliable);
        }

        private Player FindPlayerByName(string username) {
            foreach (Player p in players) {
                if (p != null && p.Username.Equals(username))
                    return p;
            }

            return null;
        }

        public void Stop() {
            running = false;
            if (thread.IsAlive)
                thread.Join();
        }
    }

    class StatusEventArgs : EventArgs {
        public string Status { get; set; }

        public StatusEventArgs(string status) {
            Status = status;
        }
    }
}
