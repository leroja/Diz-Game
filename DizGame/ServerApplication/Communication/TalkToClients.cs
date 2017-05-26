using Lidgren.Network;
using System;
using System.Collections.Generic;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using GameEngine.Source.Enums;
//using GameEngine.Source.Communication;
using DizGame.Source.Enums;
using ServerApplication.Protocol;
using ServerSupportedCommunication.Enums;

namespace ServerApplication.Communication
{
    public class TalkToClients
    {

        //private hmm keeps states of history for smoothing and prediction.

        private NetOutgoingMessage broadcastMessage;
        private NetIncomingMessage incommingMessage;

        private NetServer server;

        private int WAIT_MAX_MILLIS = 100;
        private int MAX_MESSAGE_SIZE = 100;

        public TalkToClients(NetServer server)
        {
            this.server = server;

        }


        /// <summary>
        /// Used internally for setting a client's state as percieved by the server.
        /// </summary>
        private enum ClientStateEnum
        {
            SendingMessages = 1,
            NotAnsweredXTimes = 3,
            Predict = 4,

        }



        /// <summary>
        /// This function shall answer the message from the client. 
        /// The answer is depending of which type of message is recevied.
        /// </summary>
        /// <param name="message"></param>
        public void AnswerMessage(NetIncomingMessage message)
        {
            Byte messageType;

            switch (message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    //read which message type (first byte) as defined by us and act accordingly.
                    messageType = message.ReadByte();

                    switch (messageType)
                    {
                        case (byte)MessageType.GetInitialGameState:
                            SendInitialGameState(message, GameSettingsType.GameSettings0);
                            break;

                        case (byte)MessageType.WhoIsTheMaster:
                            SendWhoIsTheMaster(message);
                            break;





                        //Used for debugging purposes when communicating with the clients.
                        case (byte)MessageType.DebugThisFunction0:
                            DebugFunction0(message);
                            break;
                        case (byte)MessageType.DebugThisFunction1:
                            DebugFunction1(message);
                            break;
                        case (byte)MessageType.DebugThisFunction2:
                            DebugFunction2(message);
                            break;
                        case (byte)MessageType.DebugThisFunction3:
                            DebugFunction3(message);
                            break;
                        case (byte)MessageType.DebugThisFunction4:
                            DebugFunction4(message);
                            break;
                        case (byte)MessageType.DebugThisFunction5:
                            DebugFunction5(message);
                            break;

                    }
                    break;

                default:
                    break;
            }
        }


        private void SendWhoIsTheMaster(NetIncomingMessage message)
        {
            //int messageLen = 0;
            //Byte[] messageArray;
            NetOutgoingMessage outMessage;

            InitMessage(/*out messageArray,*/ out outMessage);

            //Building the message.
           GameStateProtocol.WhoIsTheMaster(ref outMessage);

            SendMessage(/*messageLen, ref messageArray,*/ message, outMessage);
        }


        /// <summary>
        /// This function shall send the initial game state when asked for by the clients.
        /// </summary>
        private void SendInitialGameState(NetIncomingMessage message, GameSettingsType gameSetting)
        {
            //    int messageLen = 0;
            //    Byte[] messageArray;
            NetOutgoingMessage outMessage;

            InitMessage(/*out messageArray, */out outMessage);

            //Building the message.
            GameStateProtocol.InitialGameState(ref outMessage, gameSetting);

            SendMessage(/*messageLen, ref messageArray, */message, outMessage);
        }


        private void InitMessage(/*out byte[] messageArray, */out NetOutgoingMessage outMessage)
        {
            //messageArray = new byte[MAX_MESSAGE_SIZE];

            outMessage = server.CreateMessage();
        }


        private void SendMessage(/*int messageLen, ref Byte[] messageArray, */NetIncomingMessage message, NetOutgoingMessage outMessage)
        {
            //Array.Resize(ref messageArray, messageLen);

            //outMessage.Write(messageArray);

            server.SendMessage(outMessage, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);

            server.FlushSendQueue();
        }





        /// <summary>
        /// This function shall try to read messages from client in
        /// a round the robin fashion.
        /// Clients that have not answered for a couple of reads
        /// will be set to a state that predicts the client's position
        /// and movements.
        /// </summary>
        private void ReadClientsRoundTheRobin()
        {
            //Maybe not using this function as it is in the server logic above.
            foreach (NetConnection netconn in server.Connections)
            {
                netconn.Peer.WaitMessage(WAIT_MAX_MILLIS);
            }
        }

        /// <summary>
        /// Write messages to all connected clients updating physics state
        /// health, ammo, weapon.
        /// Client state contains:
        /// physics state:
        /// *time
        /// *entityID
        /// *position(Vector3)
        /// *rotation
        /// *scale
        /// *Should be moved to client side though.
        /// 
        /// Rest of state:
        /// entityID
        /// health
        /// ammo
        /// weapon.
        /// </summary>
        private void WriteClientsState(int entityID)
        {

            broadcastMessage = server.CreateMessage();
            broadcastMessage.WriteTime(true);

            //WriteKeyBoardStates();

            //foreach (NetPeer peer in clients)
            server.SendMessage(broadcastMessage, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
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


        private void DebugFunction0(NetIncomingMessage message)
        {

        }

        private void DebugFunction1(NetIncomingMessage message)
        {

        }

        private void DebugFunction2(NetIncomingMessage message)
        {

        }

        private void DebugFunction3(NetIncomingMessage message)
        {

        }

        private void DebugFunction4(NetIncomingMessage message)
        {

        }

        private void DebugFunction5(NetIncomingMessage message)
        {
            broadcastMessage = server.CreateMessage();

            //Sendig a 0 because using Byte[] arr on the other side for now.
            broadcastMessage.Write((byte)MessageType.DebugFunction5);
            //broadcastMessage.Write((byte)0);

            broadcastMessage.Write(message);

            server.SendMessage(broadcastMessage, message.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
