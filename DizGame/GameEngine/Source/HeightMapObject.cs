using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;

namespace GameEngine.Source.Objects
{
    /// <summary>
    /// HeightmapObject is used by the HeightMapSystem for creating hightmaps
    /// in the game engine. This object can be used from the game side.
    /// </summary>
    public class HeightMapObject
    {
        public int TerrainWidth { get; set; }
        public int TerrainHeight { get; set; }
        public string TerrainMapName { get; set; }

        private float ScaleFactor { get; set; }

        public TransformComponent Transform { get; set; }

        /// <summary>
        /// heightData a 2D-dimesional array is given by the user to build a heightmap from.
        /// </summary>
        public float[,] HeightData { get; set; }

        /// <summary>
        /// entityId will contain the entity id after the system has built a heightmap. 
        /// </summary>
        public int EntityId { get; set; }

    }
}
