using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Enums;
using DizGame.Source.Settings;
//using GameEngine.Source.Communication;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using ServerSupportedCommunication.Enums;
using Lidgren.Network;

namespace ServerApplication.Protocol
{
    /// <summary>
    /// This class is used when communicating with clients from the server.
    /// </summary>
    public class GameStateProtocol
    {
        private static readonly GameStateProtocol Instance = new GameStateProtocol();

        private static readonly int START_OF_RANGE = 1000000;
        private static readonly int RANGE_SPAN = 999999;

        private static List<Vector3> staticModelPositions;

        private static List<int> entityIdReserved;
        private static List<int> reserveStart;
        private static List<int> reserveEnd;

        private static int playerEntityId;
        private static Object _lockObject;

        private static bool IsMasterSent;

        /// <summary>
        /// This protocol is used by the server to store a range of entityIds that is sent to the clients.
        /// It also contains functions to build messages for the clients.
        /// </summary>
        private GameStateProtocol()
        {
            int startOfRange = 1000000;
            int rangeSpan = 999999;

            staticModelPositions = new List<Vector3>();

            entityIdReserved = new List<int>();
            reserveStart = new List<int>();
            reserveEnd = new List<int>();

            reserveStart.Add(startOfRange);
            reserveEnd.Add(startOfRange + rangeSpan);

            playerEntityId = 1;

            _lockObject = new object();

            IsMasterSent = false;
        }


        /// <summary>
        /// This function will reserve a range of entityIds for a specific player.
        /// </summary>
        /// <param name="playerEntityId">The entityId that is reserving a range of entityIds for
        /// the player to use as wished.</param>
        /// <param name="rangeStart">The start of the range that is reserved by the player. The number is included in the range.</param>
        /// <param name="rangeEnd">The end of the range that is reserved by the player. The number is 
        /// included in the range.</param>
        public static void ReserveRangeEntityIds(int playerEntityId, ref int rangeStart, ref int rangeEnd)
        {
            entityIdReserved.Add(playerEntityId);
            rangeStart = reserveStart.Last();
            rangeEnd = reserveEnd.Last();

            reserveStart.Add(reserveStart.Last() + RANGE_SPAN + 1);
            reserveEnd.Add(reserveEnd.Last() + 1 + RANGE_SPAN);

        }


        /// <summary>
        /// This function finds the range start and end of entityIds for a specific player, that
        /// has reserved the range earlier.
        /// </summary>
        /// <param name="playerEntityId">The entityId that has reserved a range of entityIds for
        /// the player.</param>
        /// <param name="rangeStart">The start of the range that is reserved by the player. The number is included in the range.</param>
        /// <param name="rangeEnd">The end of the range that is reserved by the player. The number is 
        /// included in the range.</param>
        public static void GetReservedRangeEntityIds(int playerEntityId, ref int rangeStart, ref int rangeEnd)
        {
            int index = entityIdReserved.IndexOf(playerEntityId);

            rangeStart = reserveStart.ElementAt(index);
            rangeEnd = reserveEnd.ElementAt(index);
        }



        /// <summary>
        /// This function sends a message to one client that it is the master and the others will receive 
        /// that they are not the master in deciding game map etc.
        /// </summary>
        /// <param name="outMessage">The built message ready to send.</param>
        public static void WhoIsTheMaster(ref NetOutgoingMessage outMessage)
        {
            //Part1 of message
            outMessage.Write((Byte)MessageType.YouAreTheMaster);



            if (!IsMasterSent)
            {
                outMessage.Write(true);
                IsMasterSent = true;
            }
            else
                outMessage.Write(false);

        }


        /// <summary>
        /// This function builds a message for the client and contains the initial settings that 
        /// the client will load at game start.
        /// </summary>
        /// <param name="outMessage">The built message ready to send.</param>
        /// <param name="gameSetting">The gameSetting the clients will use for loading map, place static objects on locations and so on.</param>
        public static void InitialGameState(ref NetOutgoingMessage outMessage, GameSettingsType gameSetting)
        {
            int rangeStart = 0;
            int rangeEnd = 0;
            int seed = 122;

            //Part1 of message
            outMessage.Write((Byte)MessageType.CreateInitialGameState);

            //Lock for playerEntityId
            lock (_lockObject)
            {
                //Part2 of message
                outMessage.Write(playerEntityId);

                //Part3 of message
                outMessage.Write((Byte)gameSetting);


                //Part4 of message
                ReserveRangeEntityIds(playerEntityId++, ref rangeStart, ref rangeEnd);
                outMessage.Write(rangeStart);
                outMessage.Write(rangeEnd);

                //Part5 of message will be the seed to derive positions from that will be needed by client to
                //build gamesettings0 gameMap.
                outMessage.Write(seed);
            }
        }
    }
}
