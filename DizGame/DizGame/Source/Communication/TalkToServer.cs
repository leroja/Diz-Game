using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GameEngine.Source.Communication;
using GameEngine.Source.Components;
//using ServerApplication.Enums;
using GameEngine.Source.Managers;
using Lidgren.Network;
using ServerSupportedCommunication.Enums;
using Microsoft.Xna.Framework;
using ServerSupportedCommunication.XNAExtensions.Lidgren.Network.Xna;
using ServerSupportedCommunication.XNAExtensions;

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
        public static int PlayerEntityId { get; private set; }
        public static byte GameSetting { get; private set; }
        public static int RangeStart { get; private set; }
        public static int RangeEnd { get; private set; }
        public static int Seed { get; private set; }

        //Variables that are received from server when asking for WhoIsTheMaster
        public static bool IsMaster  { get; private set; }

        public TalkToServer(NetClient client)
        {
            TalkToServer.client = client;
            IsMaster = false;
        }


        /// <summary>
        /// This function shall answer the message from the server - if deemed necessary - or just take action on message.
        /// The answer or action is depending of which type of message is received.
        /// </summary>
        /// <param name="message"></param>
        public void AnswerMessage(NetIncomingMessage message)
        {
            byte messageType;

            switch (message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    //read which message type (first byte) as defined by us and act accordingly.
                    //ConvertFromByteArray.ConvertValue(message.ReadBytes(2), 0, out messageType);
                    messageType = message.ReadByte();

                    switch (messageType)
                    {
                        case (byte)MessageType.CreateInitialGameState:
                            ReceiveInitialGameState(message);

                            //Functions that need debugging:
                            //This uses DebugThisFunction5();
                            SendCreatedNewBullet(1, "Bullet", new Vector3(1, 1, 1), new Vector3(.1f, .1f, .1f), 100, 1000, new Vector3(1, 1, 1), 10);

                            break;

                        case (byte)MessageType.YouAreTheMaster:
                            IAmTheMaster(message);
                            break;


                            //Use these cases when debugging messages:
                        case (byte)MessageType.DebugFunction0:
                            DebugThisFunction0(message);
                            break;
                        case (byte)MessageType.DebugFunction1:
                            DebugThisFunction1(message);
                            break;
                        case (byte)MessageType.DebugFunction2:
                            DebugThisFunction2(message);
                            break;
                        case (byte)MessageType.DebugFunction3:
                            DebugThisFunction3(message);
                            break;
                        case (byte)MessageType.DebugFunction4:
                            DebugThisFunction4(message);
                            break;
                        case (byte)MessageType.DebugFunction5:
                            DebugThisFunction5(message);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }


        private void IAmTheMaster(NetIncomingMessage message)
        {
            //Byte[] convertArray = message.ReadBytes(message.LengthBytes);
            //bool valueBool = false;

            //Part2 of message
            //ConvertFromByteArray.ConvertValue(convertArray, 0, out valueBool);
            //IsMaster = valueBool;
            IsMaster = message.ReadBoolean();

        }


        /// <summary>
        /// This function shall receive the initial game state when asked for by the clients.
        /// </summary>
        private void ReceiveInitialGameState(NetIncomingMessage message)
        {
            //Byte[] convertArray = message.ReadBytes(message.LengthBytes);
            //int valueInt = 0;
            //Byte valueByte = 0;
            //int currentPos = 0;

            //    //Part2 of message
            //currentPos = ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //PlayerEntityId = valueInt;
            PlayerEntityId = message.ReadInt32();


            //    //Part3 of message
            //currentPos = ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueByte);
            //GameSetting = valueByte;
            GameSetting = message.ReadByte();


            //    //Part4 of message
            //currentPos = ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //RangeStart = valueInt;
            RangeStart = message.ReadInt32();

            //currentPos = ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //RangeEnd = valueInt;
            RangeEnd = message.ReadInt32();

            //    //Part5 of message will be the seed to derive positions from that will be needed by client to
            //currentPos = ConvertFromByteArray.ConvertValue(convertArray, currentPos, out valueInt);
            //Seed = valueInt;
            Seed = message.ReadInt32();
        }


        /// <summary>
        /// This function is temporary - don't use it for now other than for testing purposes.
        /// </summary>
        public void SendRequestInitialGameState()
        {
            InitMessage();

            int arrLength = 0;

            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, (byte)MessageType.GetInitialGameState);
            message.Write((byte)MessageType.GetInitialGameState);

            SendMessage(/*arrLength*/);
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

            //int arrLength = 0;

            /////////////This line just used for debugging - remove in final version.
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, (byte)MessageType.DebugThisFunction5);
            message.Write((byte)MessageType.DebugThisFunction5);

            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, (byte)MessageType.CreatedNewBulletComponent);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, entityId);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, modelName);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, modelName);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, position);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, scale);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, maxRange);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, initialVelocity);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, rotation);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, damage);

            message.Write((byte)MessageType.CreatedNewBulletComponent);
            message.Write(entityId);
            message.Write(modelName);
            message.Write(position);
            message.Write(scale);
            message.Write(maxRange);
            message.Write(initialVelocity);
            message.Write(rotation);
            message.Write(damage);

            SendMessage(/*arrLength*/);
        }

        private static void SendCreatedNewTransformComponent(int entityId, TransformComponent component)
        {
            InitMessage();

            //int arrLength = 0;

            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, (byte)MessageType.CreatedNewTransformComponent);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, entityId);
            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, component);

            message.Write((byte)MessageType.CreatedNewTransformComponent);
            message.Write(entityId);
            message.WriteTransform(component);

            SendMessage(/*arrLength*/);
        }


        private static void SendRequestWhoIsTheMaster()
        {
            InitMessage();

            //int arrLength = 0;

            //arrLength = ConvertToByteArray.ConvertValue(ref messageArray, arrLength, (byte)MessageType.WhoIsTheMaster);
            message.Write((byte)MessageType.WhoIsTheMaster);

            SendMessage(/*arrLength*/);
        }


        private static void InitMessage()
        {
            message = client.CreateMessage();

            //messageArray = new Byte[MAX_MESSAGE_SIZE];
        }


        private static void SendMessage(/*int arrLength*/)
        {
            //Array.Resize(ref messageArray, arrLength);

            //message.Write(messageArray);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }


        /// <summary>
        /// Used for internal debugging purposes by developers that need testing functions before making them public.
        /// </summary>
        private void DebugThisFunction0(NetIncomingMessage message)
        {
            //string what = "";
            //int pos = ConvertFromByteArray.ConvertValue(message.ReadBytes(message.LengthBytes), 0, out what);
        }

    
        private void DebugThisFunction1(NetIncomingMessage message)
        {

        }

        private void DebugThisFunction2(NetIncomingMessage message)
        {

        }

        private void DebugThisFunction3(NetIncomingMessage message)
        {

        }

        private void DebugThisFunction4(NetIncomingMessage message)
        {

        }

        private void DebugThisFunction5(NetIncomingMessage message)
        {
            ;
        }
    }
}
