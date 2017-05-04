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

namespace ServerApplication.ServerLogic
{
    public class Server
    {
        private NetServer server;
        private List<NetPeer> clients;
        private int portnumber;

        //private hmm keeps states of history for smoothing and prediction.
        
        private NetOutgoingMessage broadcastMessage;

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
                                var data = message.ReadString();
                                Console.WriteLine(data);

                                NetOutgoingMessage somemsg = server.CreateMessage("Damn!");

                                // Might wanna use different delivery method
                                server.SendMessage(somemsg, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                                server.FlushSendQueue();

                                if (data == "exit")
                                {
                                    stop = true;
                                }

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


        /// <summary>
        /// Write messages to all connected clients updating physics state (from KeyBoardComponent) and
        /// health, ammo, weapon.
        /// Client state contains:
        /// physics state:
        /// *time
        /// *entityID
        /// How many keyboardstates that will be sent.
        /// *keyBoardStates for every movable controlled entity.
        /// *to be cont.
        /// *Should be moved to client side though.
        /// 
        /// Rest of state:
        /// entityID
        /// health
        /// ammo
        /// weapon.
        /// </summary>
        private void WriteClientsState()
        {
            
            broadcastMessage = server.CreateMessage();
            broadcastMessage.WriteTime(true);

            WriteKeyBoardStates();


            foreach (NetPeer peer in clients)
                server.SendMessage(broadcastMessage, peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        /// <summary>
        /// This function writes all keyboardstates that has a key pressed to message.
        /// </summary>
        private void WriteKeyBoardStates()
        {
            List<int> entities;
            KeyBoardComponent kbdComponent;

            entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<KeyBoardComponent>();

            foreach (int entityID in entities)
            {
                kbdComponent = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(entityID);

                broadcastMessage.WriteVariableInt32(entityID);

                foreach(string move in kbdComponent.State.Keys)
                {
                    if (kbdComponent.State[move] == ButtonStates.Pressed)
                        broadcastMessage.WriteAllFields(move);
                }
                    
            }
        }

        /// <summary>
        /// This function shall see how long a client hasnt sent a message.
        /// Maybe the client disconnected or the network is lagging.
        /// </summary>
        private void CheckClientsIfConnected()
        {
            //server.Statistics.ReceivedBytes
        }

        /// <summary>
        /// This function shall predict the clients state - if it has not been sending a message for some
        /// amount of time.
        /// </summary>
        private void PredictClientsState()
        {
        }

        /// <summary>
        /// This function shall smoothe the movements of the client until the client is back
        /// and have sent a new state.
        /// </summary>
        private void SmootheClientState()
        {

        }

    }
}
