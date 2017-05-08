using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// NetworkSystem class is responsible for the server side application
    /// and contains the functions needed for the administration.
    /// </summary>
    public class NetworkSystem
    {
        NetPeerConfiguration config;
        NetClient client;
        int portnumber;

        /// <summary>
        /// Basic constructor for the NetworkSystem, default portnumber is 1337
        /// </summary>
        public NetworkSystem()
        {
            portnumber = 1337;
        }

        /// <summary>
        /// Alternate constructor, if another portnumber is desired.
        /// </summary>
        /// <param name="portnumber"></param>
        public NetworkSystem(int portnumber)
        {
            this.portnumber = portnumber;
        }

        /// <summary>
        /// Method for creating and running the network client
        /// </summary>
        public void RunClient()
        {
             config = new NetPeerConfiguration("gameOne")
            {
                AutoFlushSendQueue = false
            };
            client = new NetClient(config);
            client.Start();

            string ip = "localhost";
            int port = portnumber;
            client.Connect(ip, port);
        }
        /// <summary>
        /// This method is used to get the client to discover the server.
        /// </summary>
        public void DiscoverLocalPeers()
        {
            client.DiscoverLocalPeers(1337);
        }

        /// <summary>
        /// Can be used to send text messages to the server.
        /// </summary>
        /// <param name="text">a string that should be sent to the server</param>
        public void SendMessage(string text)
        {
            // Similar methods like this can be used to send other types of information to the server
            NetOutgoingMessage message = client.CreateMessage(text);

            // Might wanna use different delivery method
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
                        
        }

        /// <summary>
        /// Method for reading incoming messages from the server, if there is any.
        /// Unknown message types results in a printed message with the messagetype.
        /// </summary>
        public void ReadMessages()
        {
            NetIncomingMessage message;
            

            //TODO: add functionallity for the different messagetypes, some information might be needed to be broadcasted to other clients
            
            while ((message = client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        //
                        // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                        //
                        client.SendDiscoveryResponse(null, message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        {
                            Console.WriteLine("Client got a message!");
                            var data = message.ReadString();
                            Console.WriteLine(data);

                           
                            break;
                        }
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine(message.ReadString());
                        break;
                    default:
                        Console.WriteLine("Unhandled message type: {message.MessageType}");
                        break;
                }
                client.Recycle(message);
            }
            

            
        }
    }
}
