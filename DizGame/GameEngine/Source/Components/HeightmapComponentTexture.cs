using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component that contain information for a heightmap
    /// </summary>
    public class HeightmapComponentTexture : IComponent
    {
        /// <summary>
        /// HeightData of th eheightmap
        /// </summary>
        public float[,] HeightMapData { get; set; }
        /// <summary>
        /// A list of the chunks that belong to the Heightmap
        /// </summary>
        public List<HeightMapChunk> HeightMapChunks { get; set; }

        /// <summary>
        /// The width of the Heightmap
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of the Heightmap
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeightmapComponentTexture()
        {
            HeightMapChunks = new List<HeightMapChunk>();
        }
    }
}
