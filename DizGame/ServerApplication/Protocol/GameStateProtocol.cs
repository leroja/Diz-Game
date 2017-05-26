using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Enums;
using DizGame.Source.Settings;
using GameEngine.Source.Communication;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using ServerSupportedCommunication.Enums;

namespace ServerApplication.Protocol
{
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

        public static int WhoIsTheMaster(Byte[] messageArray)
        {

            int messageLen = 0;

            //Part1 of message
            messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, (Byte)MessageType.YouAreTheMaster);

            //Part2 of message this row seems not needed...
            if (!IsMasterSent)
            {
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, true);
                IsMasterSent = true;
            }
            else
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, false);

            return messageLen;
        }


        /// <summary>
        /// This function builds a message for the client and contains the initial settings that 
        /// the client will load at gamew start.
        /// </summary>
        /// <param name="messageArray">The array that will contain the message to be sent to the client.</param>
        /// <returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int InitialGameState(Byte[] messageArray, GameSettingsType gameSetting)
        {
            int rangeStart = 0;
            int rangeEnd = 0;
            int messageLen = 0;
            int numOfObjectsTotal = 0;
            int partOfTotal = 0;
            int seed = 122;

            //Part1 of message
            messageLen = ConvertToByteArray.ConvertValue(ref messageArray, 0, (Byte)MessageType.CreateInitialGameState);

            //Part2 of message this row seems not needed...
            //messageLen += ConvertToByteArray.ConvertValue(ref messageArray, messageLen, (Byte)GameSettingsType.PlayerEntityId);

            //Lock for playerEntityId
            lock (_lockObject)
            {
                //Part2 of message
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, playerEntityId);

                //Part3 of message
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, (Byte)gameSetting);



                //Part4 of message
                ReserveRangeEntityIds(playerEntityId++, ref rangeStart, ref rangeEnd);
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, rangeStart);
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, rangeEnd);

                //Part5 of message will be the seed to derive positions from that will be needed by client to
                //build gamesettings0 gameMap.
                messageLen = ConvertToByteArray.ConvertValue(ref messageArray, messageLen, seed);
            }

            return messageLen;
        }


        //private static List<Vector3> GetStaticObjectPositions(GameSettingsType gameSetting)
        //{
        //    int numOfObjectsTotal = 0;
        //    int partOfTotal = 0;

        //    if (Int32.TryParse(GameSettings.GetGameSettings(gameSetting, GameSettingsType.CountOfHouses), out partOfTotal))
        //    {
        //        numOfObjectsTotal += partOfTotal;
        //        if (Int32.TryParse(GameSettings.GetGameSettings(gameSetting, GameSettingsType.CountOfHouses), out partOfTotal))
        //        {
        //            numOfObjectsTotal += partOfTotal;
        //            return DizGame.EntityFactory.GetModelPositions(numOfObjectsTotal);
        //        }
        //    }

        //    return new List<Vector3>();
        //}
    }
}
