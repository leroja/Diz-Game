using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSupportedCommunication.Enums
{
    /// <summary>
    /// These are the supported component types that the server and client
    /// may send between them.
    /// </summary>
    public enum ComponentType : byte
    {
        TransformComponent,
        PhysicsProjectileComponent,
        BulletComponent,
    }
}
