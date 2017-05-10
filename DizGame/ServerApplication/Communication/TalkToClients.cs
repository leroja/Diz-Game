using Lidgren.Network;
using System;
using System.Collections.Generic;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using GameEngine.Source.Enums;

namespace ServerApplication.Communication
{
    public class TalkToClients
    {

        //private hmm keeps states of history for smoothing and prediction.

        private NetOutgoingMessage broadcastMessage;
        private NetIncomingMessage incommingMessage;

        private NetServer server;

        private int WAIT_MAX_MILLIS = 100;

        public TalkToClients(ref NetServer server)
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
        private void AnswerMessage(NetIncomingMessage message)
        {
            switch(message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    //read which message type (first byte) as defined by us and act accordingly.
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// This function shall send the initial game state when asked for by the clients.
        /// </summary>
        private void SendInitialGameState()
        {
            //Send boulders, houses, tree as a list of positions.
            //Send Players as entities.
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
           foreach(NetConnection netconn in server.Connections)
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

        ///// <summary>
        ///// This function writes all keyboardstates that has a key pressed to message.
        ///// </summary>
        //private void WriteKeyBoardStates()
        //{
        //    //TO DO: Build a byte array of message for each client. Seems easier than writing 
        //    //variables of different kind to message.
        //    List<int> entities;
        //    KeyBoardComponent kbdComponent;

        //    entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<KeyBoardComponent>();

        //    foreach (int entityID in entities)
        //    {
        //        kbdComponent = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(entityID);

        //        broadcastMessage.WriteVariableInt32(entityID);

        //        foreach (string move in kbdComponent.State.Keys)
        //        {
        //            if (kbdComponent.State[move] == ButtonStates.Pressed)
        //                broadcastMessage.WriteAllFields(move);
        //        }

        //    }
        //}

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
