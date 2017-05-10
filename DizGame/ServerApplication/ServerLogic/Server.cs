using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using System.Collections;
using GameEngine.Source.Enums;
using ServerApplication.Communication;

namespace ServerApplication.ServerLogic
{
    /// <summary>
    /// Server class, contains methods to run server application and also to find and recieve messages
    /// from some (if any) hosts.
    /// </summary>
    public class Server
    {
        private NetServer server;
        private List<NetPeer> clients;
        private int portnumber;

        private TalkToClients talkToClients;

        /// <summary>
        /// Default constructor for the server, default portnumber is set to 1337.
        /// </summary>
        public Server()
        {
            portnumber = 1337;
        }

        /// <summary>
        /// Alternate constructor for the server if another portnumber is desired
        /// </summary>
        /// <param name="portnumber">The desired portnumber for which the server needs to listen to</param>
        public Server(int portnumber)
        {
            this.portnumber = portnumber;
        }

        /// <summary>
        /// Method to create a server and to start it
        /// </summary>
        public void StartServer()
        {
            var config = new NetPeerConfiguration("gameOne") { Port = portnumber };
            server = new NetServer(config);
            server.Start();

            if (server.Status == NetPeerStatus.Running)
            {
                Console.WriteLine("Server is running on port " + config.Port);
            }
            else
            {
                Console.WriteLine("Server not started...");
            }
            clients = new List<NetPeer>();

            talkToClients = new TalkToClients(server);

        }

        /// <summary>
        /// Method for instructing the server to listen for incoming messages from clients
        /// If the message type is not recognised an error message with the message type is 
        /// written.
        /// </summary>
        public void ReadMessages()
        {
            NetIncomingMessage message;
            var stop = false;

            //TODO: add functionallity for the different messagetypes, some information might be needed to be broadcasted to other clients
            while (!stop)
            {
                while ((message = server.ReadMessage()) != null)
                {

                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            //
                            // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                            //
                            server.SendDiscoveryResponse(null, message.SenderEndPoint);
                            break;

                        case NetIncomingMessageType.Data:
                            {
                                Console.WriteLine("Server got message!");
                                //var data = message.ReadString();
                                //Console.WriteLine(data);

                                //NetOutgoingMessage somemsg = server.CreateMessage("Damn!");

                                //// Might wanna use different delivery method
                                //server.SendMessage(somemsg, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                                talkToClients.AnswerMessage(message);

                                server.FlushSendQueue();

                                //if (data == "exit")
                                //{
                                //    stop = true;
                                //}

                                break;
                            }

                        case NetIncomingMessageType.DebugMessage:
                            Console.WriteLine(message.ReadString());
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine(message.SenderConnection.Status);
                            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                clients.Add(message.SenderConnection.Peer);
                                Console.WriteLine("{0} has connected.", message.SenderConnection.Peer.Configuration.LocalAddress);

                            }
                            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                clients.Remove(message.SenderConnection.Peer);
                                Console.WriteLine("{0} has disconnected.", message.SenderConnection.Peer.Configuration.LocalAddress);
                            }
                            break;

                        default:
                            Console.WriteLine("Unhandled message type: {message.MessageType}");
                            break;
                    }
                    server.Recycle(message);
                }
            }

            Console.WriteLine("Shutdown package \"exit\" received. Press any key to finish shutdown");
            Console.ReadKey();
        }

    }
}