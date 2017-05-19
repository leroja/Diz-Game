using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Communication;
using GameEngine.Source.Components;
//using ServerApplication.Enums;
using GameEngine.Source.Managers;
using Lidgren.Network;
using ServerSupportedCommunication.Enums;

namespace DizGame.Source.Communication
{
    class TalkToServer
    {

        private NetOutgoingMessage broadcastMessage;
        private NetIncomingMessage incommingMessage;

        private static NetClient client;

        private int WAIT_MAX_MILLIS = 100;
        private static int MAX_MESSAGE_SIZE = 100;


        public TalkToServer(NetClient client)
        {
            TalkToServer.client = client;

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


        /// <summary>
        /// This function is used for sending a newly created entity as a list of components.
        /// </summary>
        /// <param name="entityIds">The entities' component lists to send to server.</param>
        public static void SendCreatedNewEntities(List<int> entityIds)
        {
            NetOutgoingMessage message = client.CreateMessage();
            IComponent component = null;

            foreach(int entityId in entityIds)
            {
                //message.Write(ComponentManager.Instance.GetAllEntityComponents(entityId));
                //List<IComponent> components = ComponentManager.Instance.GetAllEntityComponents(entityId);

                foreach (ComponentType type in Enum.GetValues(typeof(ComponentType)))
                {
                    switch(type)
                    {
                        case ComponentType.TransformComponent:
                            component = ComponentManager.Instance.GetEntityComponent<TextComponent>(entityId);
                            
                            break;

                        default:
                            break;
                    }
                }

                //This does not work as IComponent is not known to Lidgrens network. Vector3 does
                //not work too to send...
                //message.WriteAllFields(components);

                client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
                client.FlushSendQueue();
            }

            //Should send with Reflection if possible instead of building and converting.
            //Byte[] messageArray = new Byte[MAX_MESSAGE_SIZE];

            //int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.GetInitialGameState);

            //Array.Resize(ref messageArray, arrLength);

            
            //message.Write(messageArray);

        }
    }
}
