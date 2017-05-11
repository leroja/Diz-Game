using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Communication;
using GameEngine.Source.Enums;
using Lidgren.Network;

namespace DizGame.Source.Communication
{
    class TalkToServer
    {

        private NetOutgoingMessage broadcastMessage;
        private NetIncomingMessage incommingMessage;

        private NetClient client;

        private int WAIT_MAX_MILLIS = 100;
        private int MAX_MESSAGE_SIZE = 100;


        public TalkToServer(NetClient client)
        {
            this.client = client;

        }


        /// <summary>
        /// This function shall answer the message from the server - if deemed necessary - or just take action on message.
        /// The answer or action is depending of which type of message is recevied.
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
                        case (byte)MessageType.CreateInitialGameState:
                            ReceiveInitialGameState(message);
                            Console.WriteLine("Initial game state length: " + message.LengthBytes);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// This function shall receive the initial game state when asked for by the clients.
        /// </summary>
        private void ReceiveInitialGameState(NetIncomingMessage message)
        {
            ;
            //Byte[] messageArray = new byte[MAX_MESSAGE_SIZE];

            //NetOutgoingMessage outMessage = server.CreateMessage();

            //ConvertToByteArray.ConvertValue(ref messageArray, 0, (Byte)MessageType.CreateMap);

            //outMessage.Write(messageArray);

            //server.SendMessage(outMessage, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);

            ////Send boulders, houses, tree as a list of positions.
            ////Send Players as entities.
        }

        /// <summary>
        /// This function is temporary - dont use it for now other than for testing purposes.
        /// </summary>
        public void SendRequestInitialGameState()
        {
            NetOutgoingMessage message = client.CreateMessage();

            Byte[] messageArray = new Byte[MAX_MESSAGE_SIZE];

            int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.GetInitialGameState);

            Array.Resize(ref messageArray, arrLength);

            message.Write(messageArray);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }
    }
}
