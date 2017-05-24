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
using Microsoft.Xna.Framework;

namespace DizGame.Source.Communication
{
    class TalkToServer
    {

        private NetOutgoingMessage broadcastMessage;
        private NetIncomingMessage incommingMessage;

        private static NetClient client;

        private static NetOutgoingMessage message;

        private static Byte[] messageArray;

        private int WAIT_MAX_MILLIS = 100;
        private static int MAX_MESSAGE_SIZE = 10000;

        //Variables that are received from server when asking for InitialGameState
        public static int playerEntityId { get; private set; }
        public static byte gameSetting { get; private set; }
        public static int rangeStart { get; private set; }
        public static int rangeEnd { get; private set; }
        public static int seed { get; private set; }

        //Variables that are received from server when asking for WhoIsTheMaster
        public static bool MeMaster  { get; private set; }

        public TalkToServer(NetClient client)
        {
            TalkToServer.client = client;
            MeMaster = false;
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
                    ConvertFromByteArray.ConvertValue(message.ReadBytes(2), 0, out messageType);

                    switch (messageType)
                    {
                        case (byte)MessageType.CreateInitialGameState:
                            ReceiveInitialGameState(message);
                            Console.WriteLine("Initial game state length: " + message.LengthBytes);
                            break;

                        case (byte)MessageType.YouAreTheMaster:
                            IAmTheMaster(message);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        private static void IAmTheMaster(NetIncomingMessage message)
        {
            Byte[] convertArray = message.ReadBytes(message.LengthBytes);
            bool valueBool = false;

            //Part2 of message
            //ConvertFromByteArray.ConvertValue(convertArray, 0, out valueBool);
            //MeMaster = valueBool;

        }


        /// <summary>
        /// This function shall receive the initial game state when asked for by the clients.
        /// </summary>
        private void ReceiveInitialGameState(NetIncomingMessage message)
        {
            ////string what = ConvertFromByteArray.ConvertValue(message.ReadBytes(message.LengthBytes));

            //Byte[] convertArray = message.ReadBytes(message.LengthBytes);
            //int valueInt = 0;
            //Byte valueByte = 0;
            //int currentPos = 0;

            ////    //Part2 of message
            //currentPos += ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //playerEntityId = valueInt;


            ////    //Part3 of message
            //currentPos += ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueByte);
            //gameSetting = valueByte;


            ////    //Part4 of message
            //currentPos += ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //rangeStart = valueInt;

            //currentPos +=ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //rangeEnd = valueInt;

            ////    //Part5 of message will be the seed to derive positions from that will be needed by client to
            //currentPos += ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //seed = valueInt;
        }


        /// <summary>
        /// This function is temporary - dont use it for now other than for testing purposes.
        /// </summary>
        public void SendRequestInitialGameState()
        {
            InitMessage();

            int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.GetInitialGameState);

            SendMessage(arrLength);
        }


        /// <summary>
        /// This function is used for sending a newly created entity as a list of components.
        /// </summary>
        /// <param name="entityIds">The entities' component lists to send to server.</param>
        public static void SendCreatedNewEntities(List<int> entityIds)
        {
            IComponent component = null;

            foreach(int entityId in entityIds)
            {
                foreach (ComponentType type in Enum.GetValues(typeof(ComponentType)))
                {
                    component = null;

                    switch(type)
                    {
                        case ComponentType.TransformComponent:
                            component = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);
                            if(component != null) //Not sure if this always will be null when componentManager fails...
                                SendCreatedNewTransformComponent(entityId, (TransformComponent)component);
                            break;

                        default:
                            break;
                    }
                }
            }
        }


        private static void SendCreatedNewBullet(int entityId, string modelName, Vector3 position, Vector3 scale, float maxRange, float initialVelocity,
                                                 Vector3 rotation, float damage)
        {
            InitMessage();

            int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.CreatedNewBulletComponent);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, entityId);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, modelName);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, position);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, scale);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, maxRange);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, initialVelocity);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, rotation);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, damage);

            SendMessage(arrLength);
        }

        private static void SendCreatedNewTransformComponent(int entityId, TransformComponent component)
        {
            InitMessage();

            int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.CreatedNewTransformComponent);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, 0, entityId);
            arrLength += ConvertToByteArray.ConvertValue(ref messageArray, arrLength, component);

            SendMessage(arrLength);
        }


        private static void SendRequestWhoIsTheMaster()
        {
            InitMessage();

            int arrLength = ConvertToByteArray.ConvertValue(ref messageArray, 0, (byte)MessageType.WhoIsTheMaster);

            SendMessage(arrLength);
        }


        private static void InitMessage()
        {
            message = client.CreateMessage();

            messageArray = new Byte[MAX_MESSAGE_SIZE];
        }


        private static void SendMessage(int arrLength)
        {
            Array.Resize(ref messageArray, arrLength);

            message.Write(messageArray);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }

    }
}
