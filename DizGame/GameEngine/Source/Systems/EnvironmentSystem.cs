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
            //p = (float)DensityType.Water_Pure;

            float Cd = (float)phy.DragType;                                                     // drag coefficent eg 0.25 to 0.45 for car
            Vector3 V = transform.Dirrection;                                                   // V unit vector indicating the direction of the velocity (negativ to indicate drag opposite the velocity)
            float A = phy.ReferenceArea;                                                        // reference area
            Vector3 v = phy.Velocity; // -Wind; TODO: Från particleSystem                       // speed of the object relativ to the fluid???

            //Vector3 Fd = -0.5f * p * Vector3Pow(v, 2) * A * Cd * V;                             // 1/2pv^2ACdV = force of drag
            Vector3 Fd = -Cd * p * Vector3Pow(v, 2) * A / 2 * V;
            Fd.X = (float)Math.Sqrt(Fd.X);
            Fd.Y = (float)Math.Sqrt(Fd.Y);
            Fd.Z = (float)Math.Sqrt(Fd.Z);
            //Fd = Fd / (Cd * p * A);
            //Console.WriteLine("DragF: " + Fd.Y + "Force: " + phy.Forces.Y + " Veclocity: " + v);

            CheckAndSetTerminalVelocity(phy, Fd);
            UpdateDownwardAcceleration(phy, Fd);
            //UpdateVelocity(phy, Cd, p, A);
            phy.Forces = Fd - phy.Weight;
            Console.WriteLine(Fd);
            
        }
        private void UpdateDownwardAcceleration(PhysicsComponent physic, Vector3 Drag)
        {
            Vector3 temp = (physic.Weight - Drag) / physic.Mass;
            temp.X = physic.Acceleration.X;
            temp.Z = physic.Acceleration.Z;
            physic.Acceleration = temp;
        }
        private void TerminalVelocity(PhysicsComponent physic, float Cd, float p, float A)
        {
            Vector3 temp = physic.Weight;
            temp.X = (float)Math.Sqrt((2 * physic.Weight.X) / (Cd * p * A));
            temp.Y = (float)Math.Sqrt((2 * physic.Weight.Y) / (Cd * p * A));
            temp.Z = (float)Math.Sqrt((2 * physic.Weight.Z) / (Cd * p * A));
            physic.Velocity = temp;
        }
        private void CheckAndSetTerminalVelocity(PhysicsComponent physic, Vector3 dragForce)
        {
            if (dragForce.X == physic.Mass)
                physic.Acceleration = new Vector3(0, physic.Acceleration.Y, physic.Acceleration.Z);
            if (dragForce.Y == physic.Mass)
                physic.Acceleration = new Vector3(physic.Acceleration.X, 0, physic.Acceleration.Z);
            if (dragForce.Z == physic.Mass)
                physic.Acceleration = new Vector3(physic.Acceleration.X, physic.Acceleration.Y, 0);
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
