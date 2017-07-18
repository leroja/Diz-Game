using Lidgren.Network;
using System;
using DizGame.Source.Communication;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// NetworkSystem class is responsible for the server side application
    /// and contains the functions needed for the administration.
    /// </summary>
    public class NetworkSystem 
    {
        private NetPeerConfiguration config;
        private NetClient client;

        private static int PORTNUMBER = 1337;
        private string IP = "localhost";

        private int portNum;

        private TalkToServer talkToServer;

        private bool EndThread;

        /// <summary>
        /// Basic constructor for the NetworkSystem, default portnumber is 1337
        /// </summary>
        public NetworkSystem(): this(PORTNUMBER)
        {

        }

        /// <summary>
        /// Alternate constructor, if another portnumber is desired.
        /// </summary>
        /// <param name="portNumber"></param>
        public NetworkSystem(int portNumber)
        {
            portNum = portNumber;

            ConfigClient();

            client = new NetClient(config);

            talkToServer = new TalkToServer(client);

            EndThread = false;
        }


        /// <summary>
        /// This function give the thread a signal to gracefully 
        /// end execution.
        /// </summary>
        public void EndExecution()
        {
            EndThread = true;
        }


        private void ConfigClient()
        {
            config = new NetPeerConfiguration("gameOne")
            {
                AutoFlushSendQueue = false

            };

            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
        }

        /// <summary>
        /// Method for creating and running the network client
        /// </summary>
        public void ConnectToServer()
        {
            client.Start();

            client.Connect(IP, portNum);

            DiscoverLocalPeers();

            //while (client.ConnectionStatus != NetConnectionStatus.Connected)
            //    ;

            //talkToServer.SendRequestInitialGameState();

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

            bool sendRequest = true;

            while (EndThread == false)
                while ((message = client.ReadMessage()) != null)
                {
                //This should be moved to some better place.
                if (client.ConnectionStatus == NetConnectionStatus.Connected && sendRequest == true)
                {
                    talkToServer.SendRequestInitialGameState();
                    sendRequest = false;
                }

                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            Console.WriteLine(message.ReadString() + " connection status: " + client.ConnectionStatus);
                            break;

                        case NetIncomingMessageType.DiscoveryRequest:
                            //
                            // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                            //
                            client.SendDiscoveryResponse(null, message.SenderEndPoint);
                            break;

                        case NetIncomingMessageType.Data:
                            {
                                Console.WriteLine("Client got a message!");
                                //var data = message.ReadString();
                                //Console.WriteLine(data);

                                talkToServer.AnswerMessage(message);

                                break;
                            }
                        case NetIncomingMessageType.DebugMessage:
                            Console.WriteLine(message.ReadString());
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine(message.ReadString());
                            break;

                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine(message.ReadString());
                            break;

                        default:
                            Console.WriteLine("Unhandled message type: {message.MessageType}");
                            break;
                    }

                    client.Recycle(message);

            }

            client.Disconnect("Client ended communication");
        }
    }
}
