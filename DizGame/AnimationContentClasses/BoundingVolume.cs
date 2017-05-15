using AnimationContentClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class BoundingVolume
    {
        public int BoundingID { get; private set; }
        public List<BoundingVolume> Volume { get; set; }
        public IBounding3D Bounding { get; set; }

        public BoundingVolume(int id, IBounding3D bounding)
        {
            BoundingID = id;
            Bounding = bounding;
            Volume = new List<BoundingVolume>();
        }

        public BoundingVolume(int id, List<BoundingVolume> volume, IBounding3D bounding)
        {
            BoundingID = id;
            Bounding = bounding;
            Volume = volume;
        }
    }

}
