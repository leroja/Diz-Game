using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class PhysicsSystem : IUpdate, IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>
    {
        private float frameCount = 0;
        private float timeSinceLastUpdate = 0;
        private float updateInterval = 1;
        private float framesPerSecond = 0;

        private PhysicsRigidBodySystem rigidBody;
        private PhysicsParticleSystem particle;
        private PhysicsProjectilesSystem projectile;
        private PhysicsRagdollSystem ragDoll;
        private PhysicsSoftSystem soft;
        private PhysicsStaticSystem _static;

        public PhysicsSystem()
        {
            rigidBody = new PhysicsRigidBodySystem();
            particle = new PhysicsParticleSystem();
            projectile = new PhysicsProjectilesSystem();
            ragDoll = new PhysicsRagdollSystem();
            soft = new PhysicsSoftSystem();
            _static = new PhysicsStaticSystem();

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

                UpdateForce(physic);
                UpdateGravity(physic);
                UpdateDistance(physic, dt);

                UpdatePhysicComponentByType(physic, dt);

                UpdateMaxAcceleration(physic);
                UpdateAcceleration(physic, dt);
                UpdateVelocity(physic, dt);

                TEMPFLOOR(ComponentManager.GetEntityComponent<TransformComponent>(entityID));

                //Console.WriteLine("Forces: " + physic.Forces);
                Console.WriteLine("Velocity: " + physic.Velocity);
                if (!physic.IsMoving)
                    UpdateLinearDeceleration(physic, dt);
            }
        }
        //TODO: TEMPFLOOR DELETE
        private void TEMPFLOOR(TransformComponent transform)
        {
            if (transform.Position.Y <= -15)
            {
                transform.Position = new Vector3(transform.Position.X, 0, transform.Position.Z);
                ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Velocity = new Vector3(
                    ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Velocity.X,
                    0,
                    ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Velocity.Z);
                ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Forces = new Vector3(
                    ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Forces.X,
                    0,
                    ComponentManager.GetEntityComponent<PhysicsComponent>(transform.ID).Forces.Z);
            }
        }
        /// <summary>
        /// Updates the objects linear acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateAcceleration(PhysicsComponent physic, float dt)
        {
            Vector3 new_ay = physic.Forces / physic.Mass;
            Vector3 avg_ay = 0.5f * new_ay;
            physic.Acceleration = avg_ay;
        }
        /// <summary>
        /// Updates the objects linear velocity
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateVelocity(PhysicsComponent physic, float dt)
        {
            physic.Velocity += physic.InitialVelocity + (physic.Acceleration * dt);

        }
        /// <summary>
        /// Updates the physic objects travled distance
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateDistance(PhysicsComponent physic, float dt)
        {
            physic.Distance = (physic.Velocity - physic.InitialVelocity * dt)
                + (0.5f * physic.Acceleration * dt * dt);
        }
        /// <summary>
        /// Updates the forces
        /// </summary>
        /// <param name="physic"></param>
        private void UpdateForce(PhysicsComponent physic)
        {
            float X, Y, Z;
            X = (physic.Mass * physic.Acceleration.X);
            Y = (physic.Mass * physic.Acceleration.Y);
            Z = (physic.Mass * physic.Acceleration.Z);

            physic.Forces = new Vector3(X, Y, Z);
        }
        /// <summary>
        /// Updates the gravity depending on physics gravity type
        /// </summary>
        /// <param name="physic"></param>
        private void UpdateGravity(PhysicsComponent physic)
        {
            switch(physic.GravityType)
            {
                case GravityType.None:
                    physic.Forces += new Vector3(physic.Forces.X, 0, physic.Forces.Z);
                    break;
                case GravityType.Self:
                    physic.Forces += new Vector3(physic.Forces.X, physic.Gravity, physic.Forces.Z);
                    break;
                case GravityType.World:
                    List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
                    WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
                    physic.Forces += new Vector3(physic.Forces.X, world.Gravity.Y, physic.Forces.Z);
                    break;
            }
                
        }
        /// <summary>
        /// Udates the corresponding object by physictype
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePhysicComponentByType(PhysicsComponent physic, float dt)
        {
                switch(physic.PhysicsType)
                {
                    case Enums.PhysicsType.Static:
                        _static.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Soft:
                        soft.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Rigid:
                        rigidBody.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Ragdoll:
                        ragDoll.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Projectiles:
                        projectile.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Particle:
                        particle.Update(physic, dt);
                        break;
                    default:
                        break;
            }
        }
        /// <summary>
        /// Calculates the physic objects Deaceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateLinearDeceleration(PhysicsComponent physic, float dt)
        {
            //TODO: Få denna skiten att fungera
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());

            Vector3 f = physic.Velocity;
            if (physic.IsFalling)
            {
                physic.Forces = new Vector3(physic.Forces.X, physic.Forces.Y, physic.Forces.Z);
            }
            else
            {
             f -= (physic.Velocity - physic.InitialVelocity) / dt;
                

                physic.Velocity = new Vector3(f.X, physic.Velocity.Y, f.X);
            }
        }
        /// <summary>
        /// Updates the objects heading depending on collision
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateReflection(PhysicsComponent target, PhysicsComponent hit)
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
        private void UpdateMaxAcceleration(PhysicsComponent physic)
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
        /// Observer funktion, Updates the reflection on two objects 
        /// on collision (retrieves data from collision system)
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(List<Tuple<BoundingSphereComponent, BoundingSphereComponent>> value)
        {
            foreach (var val in value)
                UpdateReflection(ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item1.ID), ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item2.ID));
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
    }
}
