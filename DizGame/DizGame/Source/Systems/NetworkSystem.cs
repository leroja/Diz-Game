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

        public NetworkSystem()
        {
            portnumber = 1337;
        }

        public NetworkSystem(int portnumber)
        {
            this.portnumber = portnumber;
        }

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

        public void DiscoverLocalPeers()
        {
            client.DiscoverLocalPeers(1337);
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage message = client.CreateMessage(text);

            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();

            
        }
    }
}
