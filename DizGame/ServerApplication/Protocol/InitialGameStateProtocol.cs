using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ServerApplication.Protocol
{
    public class InitialGameStateProtocol
    {
        private static readonly InitialGameStateProtocol Instance = new InitialGameStateProtocol();

        private static readonly int START_OF_RANGE = 1000000;
        private static readonly int RANGE_SPAN = 999999;

        public static int currentHeightMapNumber { get; private set; }

        private static List<Vector3> staticModelPositions;

        private static List<int> entityIdReserved;
        private static List<int> reserveStart;
        private static List<int> reserveEnd;


        /// <summary>
        /// This protocol is used by the server to store a range of entityIds that is sent to the clients.
        /// </summary>
        private InitialGameStateProtocol()
        {
            int startOfRange = 1000000;
            int rangeSpan = 999999;

            currentHeightMapNumber = 1;

            staticModelPositions = new List<Vector3>();

            entityIdReserved = new List<int>();
            reserveStart = new List<int>();
            reserveEnd = new List<int>();

            reserveStart.Add(startOfRange);
            reserveEnd.Add(startOfRange + rangeSpan);

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
    }
}
