using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.Interface;
using GameEngine.Source.RandomStuff;
using AnimationContentClasses;

namespace GameEngine.Source.Systems
{
    //TODO: Fixa så alla siffror är korrekta efter metric.
    public class PhysicsSystem : IUpdate, IPhysics, IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>
    {
        private float frameCount = 0;
        private float timeSinceLastUpdate = 0;
        private float updateInterval = 1;
        private float framesPerSecond = 0;

        private List<IPhysicsTypeSystem> physicSystems;

        public PhysicsSystem()
        {
            physicSystems = new List<IPhysicsTypeSystem>
            {
                new PhysicsRigidBodySystem(this),
                new PhysicsParticleSystem(this),
                new PhysicsProjectilesSystem(this),
                new PhysicsRagdollSystem(this),
                new PhysicsSoftSystem(this),
                new PhysicsStaticSystem(this)
        };
        }
        public void AddIPhysicsTypeSystem(IPhysicsTypeSystem system)
        {
            physicSystems.Add(system);
        }
        public void RemoveIPhysicsTypeSystem(IPhysicsTypeSystem system)
        {
            physicSystems.Remove(system);
        }
        /// <summary>
        /// Updates all the necessary part for the physicsystem
        /// using one loop
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            CountFPS(gameTime);
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                physicSystems.Where(x => x.PhysicsType == physic.PhysicsType).SingleOrDefault().Update(physic, dt);
            }
        }
        /// <summary>
        /// Using Euler order -> Acceleration -> Position -> Velocity
        /// will provide better accuracy but only when
        /// acceleration is constant
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateEulerOrder(PhysicsComponent physic, float dt)
        {
            UpdateMaxAcceleration(physic);
            UpdateMass(physic);
            UpdateGravity(physic, dt);
            UpdateForce(physic);

            UpdateEulerAcceleration(physic);

            //UpdatePhysicComponentByType(physic, dt);

            UpdateVelocity(physic, dt);

            UpdateDeceleration(physic);
        }
        /// <summary>
        /// Using Non Euler order does work but with less accurac        /// except when acceleration is not constant
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateNonEulerOrder(PhysicsComponent physic, float dt)
        {
            UpdateAcceleration(physic);
            UpdateMaxAcceleration(physic);
            UpdateMass(physic);
            UpdateGravity(physic, dt);
            UpdateForce(physic);
            UpdateVelocity(physic, dt);
            //UpdatePhysicComponentByType(physic, dt);

            UpdateDeceleration(physic);
        }
        /// <summary>
        /// Updates the mass using density * volume
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateMass(PhysicsComponent physic)
        {
            physic.Mass = (physic.Density * physic.Volume);
        }
        /// <summary>
        /// Updates the objects weigth W = m * g
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="gravity"></param>
        public virtual void UpdateWeight(PhysicsComponent physic, float gravity)
        {
            physic.Weight = -Vector3.Down * (physic.Mass * gravity);
        }
        /// <summary>
        /// Updates the objects Euler acceleration
        /// This function should be updated BEFORE position,
        /// and position updating should use LastAcceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public virtual void UpdateEulerAcceleration(PhysicsComponent physic)
        {
            // Creating more vectors than necessary for an understanding of what is what.  
            physic.LastAcceleration = physic.Acceleration;
            Vector3 new_acceleration = physic.Forces / physic.Mass;
            Vector3 avg_acceleration = (physic.LastAcceleration + new_acceleration) / 2;
            physic.Acceleration = avg_acceleration;
            //Vector3 new_ay = physic.Forces / physic.Mass;
            //Vector3 avg_ay = 0.5f * new_ay;
            //physic.Acceleration = avg_ay;
        }

        /// <summary>
        /// Calculates the physic objects Deaceleration
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateDeceleration(PhysicsComponent physic)
        {
            if (!physic.IsInAir)
            {
                if (physic.Forces.X == 0)
                    physic.Velocity = new Vector3(0, physic.Velocity.Y, physic.Velocity.Z);
                if (physic.Forces.Y == 0)
                    physic.Velocity = new Vector3(physic.Velocity.X, 0, physic.Velocity.Z);
                if (physic.Forces.Z == 0)
                    physic.Velocity = new Vector3(physic.Velocity.X, physic.Velocity.Y, 0);
            }
        }
        /// <summary>
        /// Updates the objects linear velocity
        /// using its initialVelocity if any 
        /// using m/s
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public virtual void UpdateVelocity(PhysicsComponent physic, float dt)
        {
            physic.Velocity += physic.InitialVelocity + (physic.Acceleration * dt);
            //Console.WriteLine(physic.Velocity);
        }
        /// <summary>
        /// Updates the forces
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateForce(PhysicsComponent physic)
        {
            //Console.WriteLine("For1': " + physic.Forces);
            //physic.Forces =  physic.Mass * physic.Acceleration;
            float X, Y, Z;
            X = (physic.Mass * physic.Acceleration.X);
            Y = (physic.Mass * physic.Acceleration.Y);
            Z = (physic.Mass * physic.Acceleration.Z);

            physic.Forces = new Vector3(X, Y, Z);
            //Console.WriteLine("For': " + physic.Forces);
        }
        /// <summary>
        /// Updates the Acceleration using formula 
        /// A = F/M
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateAcceleration(PhysicsComponent physic)
        {
            float X, Y, Z;
            X = (physic.Forces.X / physic.Mass);
            //Y = (physic.Forces.Y / physic.Mass);
            Z = (physic.Forces.Z / physic.Mass);

            physic.Acceleration = new Vector3(X, physic.Acceleration.Y, Z);

            //physic.Acceleration = (physic.Forces / physic.Mass);
        }
        /// <summary>
        /// Updates the gravity depending on physics gravity type
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateGravity(PhysicsComponent physic, float dt)
        {
            switch(physic.GravityType)
            {
                case GravityType.None:
                    physic.Forces += new Vector3(physic.Forces.X, 0, physic.Forces.Z);
                    UpdateWeight(physic, 0);
                    break;
                case GravityType.Self:
                    physic.Forces += Vector3.Down * physic.Gravity;
                    UpdateWeight(physic, physic.Gravity);
                    break;
                case GravityType.World:
                    List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
                    WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
                    physic.Forces += world.Gravity;
                    UpdateWeight(physic, world.Gravity.Y);
                    break;
            }
                
        }
        /// <summary>
        /// Udates the corresponding object by physictype
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        //public virtual void UpdatePhysicComponentByType(PhysicsComponent physic, float dt)
        //{
        //        switch(physic.PhysicsType)
        //        {
        //            case Enums.PhysicsType.Static:
        //                _static.Update(physic, dt);
        //                break;
        //            case Enums.PhysicsType.Soft:
        //                soft.Update(physic, dt);
        //                break;
        //            case Enums.PhysicsType.Rigid:
        //                rigidBody.Update(physic, dt);
        //                break;
        //            case Enums.PhysicsType.Ragdoll:
        //                ragDoll.Update(physic, dt);
        //                break;
        //            case Enums.PhysicsType.Projectiles:
        //                projectile.Update(physic, dt);
        //                break;
        //            case Enums.PhysicsType.Particle:
        //                particle.Update(physic, dt);
        //                break;
        //            default:
        //                break;
        //    }
        //}

        /// <summary>
        /// Updates the objects heading depending on collision
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public virtual void UpdateReflection(PhysicsComponent target, PhysicsComponent hit)
        {
           int N = 1; //dunno
           int e = 0; //Should be 0 or 1 (0 (totally plastic) to 1 (totally elastic)). 


            float ratioA = hit.Mass / (target.Mass + hit.Mass);                     // ratioa = Mb / (Ma + Mb)
            float ratioB = target.Mass / (target.Mass + hit.Mass);                  // ratiob = Ma / (Ma + Mb)
            Vector3 Vr = target.Velocity * hit.Velocity;                            // Vr = Va - Vb relativVelocity
            Vector3 I = (1 + e) * N * (Vr * N) / (1 / target.Mass + 1 / hit.Mass);  // I = (1+e)*N*(Vr • N) / (1/Ma + 1/Mb)

            target.Velocity -= I * 1 / target.Mass;                                 // Va - = I * 1/Ma
            hit.Velocity += I * 1 / hit.Mass;                                       // Vb + = I * 1/Mb
        }
        /// <summary>
        /// Updates maxacceleration in Meters per second each second 
        /// divided by FPS to give meters per second each frame.
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateMaxAcceleration(PhysicsComponent physic)
        {
                physic.MaxAcceleration = (physic.Forces / physic.Mass) / framesPerSecond;
        }
        /// <summary>
        /// Counts the frames per second
        /// </summary>
        /// <param name="gameTime"></param>
        private void CountFPS(GameTime gameTime)
        {
            frameCount++;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > updateInterval)
            {
                framesPerSecond = frameCount / timeSinceLastUpdate;
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
        }
        
        /// <summary>
        /// Does nothing atm
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            //TODO: OnError
        }
        /// <summary>
        /// Does nothing atm
        /// </summary>
        public void OnCompleted()
        {
            //TODO: OnCompleted
        }
        /// <summary>
        /// Observer funktion, Updates the reflection on two objects 
        /// on collision (retrieves data from collision system)
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>> value)
        {
            foreach (var val in value)
                UpdateReflection(ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item1.Key), ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item2.Key));

        }
    }
}
