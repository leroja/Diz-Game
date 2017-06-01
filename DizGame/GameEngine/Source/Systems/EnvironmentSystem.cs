using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System to update environment changes etc.
    /// </summary>
    public class EnvironmentSystem : IUpdate
    {
        /// <summary>
        /// Function to update environment functions.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var ents = ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>();
            Parallel.ForEach(ents, ent =>
            {
                UpdateDrag(ent, gameTime);
            });
        }

        /// <summary>
        /// Calculates the physic objects DragForce
        /// using the worlds parameters
        /// which gives and kg-m^2/s^2
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

            Vector3 V = phy.Velocity;
            if (V != Vector3.Zero)
            {
                V.Normalize();
                float Cd = (float)phy.DragType;                                                     // drag coefficent eg 0.25 to 0.45 for car
                float A = phy.ReferenceArea;                                                        // reference area
                Vector3 v = phy.Velocity; // -Wind; TODO: Från particleSystem                       // speed of the object relativ to the fluid???

                Vector3 Fd = -Cd * p * Vector3Pow(v, 2) * A / 2 * V;
                phy.Acceleration += (Fd / phy.Mass); // A = F / M
            }
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
                foreach (var pos in fluid.Value)
                    if (Vector3BiggerOrEquals(Position, pos.Item1) && Vector3SmallerOrEquals(Position, pos.Item2))
                        return fluid.Key;
            return DensityType.Air;
        }

        /// <summary>
        /// Just an private method to check Smaller or equals by two vector3
        /// </summary>
        /// <param name="smaller"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        private bool Vector3SmallerOrEquals(Vector3 smaller, Vector3 then)
        {
            if (smaller.X <= then.X)
                if (smaller.Y <= then.Y)
                    if (smaller.Z <= then.Z)
                        return true;
            return false;
        }

        /// <summary>
        /// Just an private method to check bigger or equals by two vector3
        /// </summary>
        /// <param name="bigger"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        private bool Vector3BiggerOrEquals(Vector3 bigger, Vector3 then)
        {
            if (bigger.X >= then.X)
                if (bigger.Y >= then.Y)
                    if (bigger.Z >= then.Z)
                        return true;
            return false;
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
