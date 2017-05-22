using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    /// <summary>
    /// Abstract class IBounding3D. Used to handle boundings in the same way
    /// </summary>
    public abstract class IBounding3D
    {
        /// <summary>
        /// ID for the component this IBounding3D belongs to
        /// </summary>
        //public int CompID { get; set; }
        
        /// <summary>
        /// Check if this IBounding3D intersects with another IBounding3D
        /// </summary>
        /// <param name="bounding"></param>
        /// <returns></returns>
        public abstract bool Intersects(IBounding3D bounding);

        /// <summary>
        /// Merges this IBounding with another IBounding3D
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public abstract IBounding3D CreateMerged(IBounding3D bound);
    }
}
