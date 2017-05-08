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


        private NetServer server;

        public TalkToClients(ref NetServer server)
        {
            this.server = server;
        }



        private enum ClientStateEnum
        {
            SendingMessages = 1,
            NotAnsweredXTimes = 3,
            Predict = 4,

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

        }
        
        /// <summary>
        /// Write messages to all connected clients updating physics state (from KeyBoardComponent) and
        /// health, ammo, weapon.
        /// Client state contains:
        /// physics state:
        /// *time
        /// *entityID
        /// *position(Vector3)
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

            //foreach (NetPeer peer in clients)
            server.SendMessage(broadcastMessage, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        /// <summary>
        /// This function writes all keyboardstates that has a key pressed to message.
        /// </summary>
        private void WriteKeyBoardStates()
        {
            //TO DO: Build a byte array of message for each client. Seems easier than writing 
            //variables of different kind to message.
            List<int> entities;
            KeyBoardComponent kbdComponent;

            entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<KeyBoardComponent>();

            foreach (int entityID in entities)
            {
                kbdComponent = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(entityID);

                broadcastMessage.WriteVariableInt32(entityID);

                foreach (string move in kbdComponent.State.Keys)
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
