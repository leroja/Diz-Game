using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.ServerLogic
{
    public class Server
    {
        private NetServer server;
        private List<NetPeer> clients;
        private int portnumber;
        public Server(int portnumber)
        {
            this.portnumber = portnumber;
        }
        public Server()
        {
            portnumber = 1337;
        }

        /// <summary>
        /// Method for setting up and starting the server, includes port configuration.
        /// portnumber is the value set in the constructor or set default to 1337
        /// </summary>
        public void StartServer()
        {
            var config = new NetPeerConfiguration("GameOne")
            { Port = portnumber };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);

            server = new NetServer(config);
            server.Start();

            if (server.Status == NetPeerStatus.Running)
            {
                Console.WriteLine("Server is running on port " + config.Port);
            }
            else
                Console.WriteLine("Server is not started");
            clients = new List<NetPeer>();
        }

        public void ReadMessages()
        {
            NetIncomingMessage message;
            while (true)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                //TODO: add code for handling different types of messages

                                var data = message.Data;
                                break;
                            }

                        default:
                            {
                                Console.WriteLine("Unhandled message type: {message.MessageType}");
                                break;
                            }
                    }
                }
            }
            
        }

    }
}
