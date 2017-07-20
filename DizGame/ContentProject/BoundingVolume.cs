using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace ContentProject
{
    /// <summary>
    /// 
    /// </summary>
    public class BoundingVolume
    {
        /// <summary>
        /// 
        /// </summary>
        [ContentSerializer]
        public List<BoundingVolume> Volume { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ContentSerializer]
        public IBounding3D Bounding { get; set; }

        //public BoundingVolume()
        //{
        //    Volume = new List<BoundingVolume>();
        //}
        
        private BoundingVolume()
        {
            //Volume = new List<BoundingVolume>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public BoundingVolume(object obj)
        {
            // TODO: object obj? Varför användes ej obj isf?
            Volume = new List<BoundingVolume>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounding"></param>
        public BoundingVolume(IBounding3D bounding)
        {
            Bounding = bounding;
            Volume = new List<BoundingVolume>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="bounding"></param>
        public BoundingVolume(List<BoundingVolume> volume, IBounding3D bounding)
        {
            Bounding = bounding;
            Volume = volume;
        }
    }
}
