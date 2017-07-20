using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.NetworkStuff
{
    /// <summary>
    /// These are the supported component types that the server and client
    /// may send between them.
    /// </summary>
    public enum ComponentType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        TransformComponent,
        /// <summary>
        /// 
        /// </summary>
        PhysicsProjectileComponent,
        /// <summary>
        /// 
        /// </summary>
        BulletComponent,
    }
}
