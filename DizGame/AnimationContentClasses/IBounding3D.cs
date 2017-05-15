using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public interface IBounding3D
    {
        bool Intersects(IBounding3D bounding);

        IBounding3D CreateMerged(IBounding3D bound);
    }
}
