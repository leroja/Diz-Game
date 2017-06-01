using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class BoundingVolume
    {
        [ContentSerializer]
        public List<BoundingVolume> Volume { get; set; }
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
        
        public BoundingVolume(object obj)
        {
            // TODO: object obj? Varför användes ej obj isf?
            Volume = new List<BoundingVolume>();
        }

        public BoundingVolume(IBounding3D bounding)
        {
            Bounding = bounding;
            Volume = new List<BoundingVolume>();
        }

        public BoundingVolume(List<BoundingVolume> volume, IBounding3D bounding)
        {
            Bounding = bounding;
            Volume = volume;
        }
    }

}
