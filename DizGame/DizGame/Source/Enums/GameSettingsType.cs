using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Enums
{
    /// <summary>
    /// These are the settings that the game has set on the various types of maps, count of rocks etc.
    /// </summary>
    public enum GameSettingsType : byte
    {
        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings0=10,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings1,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings2,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings3,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings4,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings5,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings6,

        /// <summary>
        /// The game setting no.
        /// </summary>
        GameSettings7,

        /// <summary>
        /// The name of the map.
        /// </summary>
        HeightMapName,

        /// <summary>
        /// The name of the texture to use for the map.
        /// </summary>
        HeightMapTexture,

        /// <summary>
        /// How many chunks that the map will be divided into.
        /// </summary>
        HeightMapChunksPerSide,

        /// <summary>
        /// The number of houses on the map.
        /// </summary>
        CountOfHouses,

        /// <summary>
        /// The count of other static objects in the scene - rocks, trees etc.
        /// </summary>
        CountOfStaticObjects,


        /// <summary>
        /// The players entity id on the client side.
        /// </summary>
        PlayerEntityId,
    }
}
