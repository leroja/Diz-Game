using GameEngine.Source.RandomStuff;
using System.Collections.Generic;

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
        /// A Bool that says whether the model is vivible or not
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeightmapComponentTexture()
        {
            HeightMapChunks = new List<HeightMapChunk>();
            IsVisible = true;
        }
    }
}
