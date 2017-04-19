using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Systems
{
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
    }
}
