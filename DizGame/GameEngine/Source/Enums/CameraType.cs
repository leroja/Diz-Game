using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    /// <summary>
    /// Enum to define the different CameraTypes
    /// </summary>
    public enum CameraType : int
    {
        /// <summary>
        /// Camera that chases the object (Third person view)
        /// </summary>
        Chase,
        /// <summary>
        /// Head like camera, used as "eyes" (Personal view)
        /// </summary>
        Pov,
        /// <summary>
        /// Static camera that is imovable (can change position manualy)
        /// </summary>
        StaticCam
    };
}
