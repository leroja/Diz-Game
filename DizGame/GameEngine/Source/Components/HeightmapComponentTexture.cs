using GameEngine.Source.RandomStuff;
using System.Collections.Generic;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component that contains information for a textured heightmap
    /// </summary>
    public class HeightmapComponent : IComponent
    {
        /// <summary>
        /// HeightData of the heightmap
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
        /// A Bool that says whether the model is visible or not
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeightmapComponent()
        {
            HeightMapChunks = new List<HeightMapChunk>();
            IsVisible = true;
        }
    }
}