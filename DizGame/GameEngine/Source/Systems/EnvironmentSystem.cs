using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class EnvironmentSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                UpdateDrag(entityID, gameTime);
            }
        }
        /// <summary>
        /// Calculates the physic objects DragForce
        /// using the worlds parameters
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="gameTime"></param>
        private void UpdateDrag(int entityID, GameTime gameTime)
        {
            PhysicsComponent phy = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
            TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());

            float p = 0;
            // density of the fluid
            DensityType fluid = GetFluid(world, transform.Position);
            if (fluid == DensityType.Air)
                p = AirCoefficients.GetAirSoundSpeedAndDensity(world.Temperatur).Item2;
            else
                p = (float)fluid;

            float Cd = (float)phy.DragType;                                                 // drag coefficent eg 0.25 to 0.45 for car
            Vector3 V = -transform.Dirrection;                                              // V unit vector indicating the direction of the velocity (negativ to indicate drag opposite the velocity)
            float A = phy.ReferenceArea;                                                    // reference area
            Vector3 v = phy.Velocity; // -Wind; TODO: Från particleSystem                   // speed of the object relativ to the fluid???

            Vector3 Fd = -1 * 0.5f * p * Vector3Pow(v, 2) * A * Cd * -V;                              // 1/2pv^2ACdV = force of drag
            Console.WriteLine("DragF: " + Fd.Y + "Force: " + phy.Forces.Y);
            //TODO: Använda Dragforce (Fd) funktionen är helt korrekt men använder inte Fd atm.
            phy.Forces += Fd;
        }
        private void UpdateWind()
        {

        }
        /// <summary>
        /// Checks if worlds fluid dictionary contains the objects
        /// position and returns the corresponding fluid density kg/m^3
        /// returns Air as standard if not in dicitionary
        /// </summary>
        /// <param name="world"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        private DensityType GetFluid(WorldComponent world, Vector3 Position)
        {
            foreach (var fluid in world.WorldFluids)
                if (fluid.Value.Contains(Position))
                    return fluid.Key;
            return DensityType.Air;
        }
        /// <summary>
        /// Funktion to power an Vector3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        private Vector3 Vector3Pow(Vector3 v1, float scalar)
        {
            return new Vector3((float)Math.Pow(v1.X, scalar), (float)Math.Pow(v1.Y, scalar), (float)Math.Pow(v1.Z, scalar));
        }
    }
}
