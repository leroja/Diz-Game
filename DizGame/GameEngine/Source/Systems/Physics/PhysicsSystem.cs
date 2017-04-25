using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;

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

                UpdatePosition(entityID, dt);

                UpdateMaxAcceleration(physic);
                UpdateLinearAcceleration(physic, dt);
                UpdateLinearVelocity(physic, dt);

                UpdatePhysicComponentByType(entityID, dt);
                //Console.WriteLine("Forces: " + physic.Forces);
                Console.WriteLine("Velocity: " + physic.Velocity);
                if (!physic.IsMoving)
                    UpdateLinearDeceleration(physic, dt);
            }
        }
        /// <summary>
        /// Updates the object position using its velocity * dt
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePosition(int entityID, float dt)
        {
            ComponentManager.GetEntityComponent<TransformComponent>(entityID).Position
                += ComponentManager.GetEntityComponent<PhysicsComponent>(entityID).Velocity * dt;
        }
        /// <summary>
        /// Udates the corresponding object by physictype
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePhysicComponentByType(int entityID, float dt)
        {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                switch(physic.PhysicsType)
                {
                    case Enums.PhysicsType.Static:
                        _static.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Soft:
                        soft.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Rigid:
                        rigidBody.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Ragdoll:
                        ragDoll.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Projectiles:
                        projectile.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Particle:
                        particle.Update(entityID, dt);
                        break;
                    default:
                        break;
            }
        }
        /// <summary>
        /// Updates the objects linear acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateLinearAcceleration(PhysicsComponent physic, float dt)
        {
            physic.Acceleration = physic.Forces / physic.Mass;
        }
        /// <summary>
        /// Updates the objects linear velocity
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateLinearVelocity(PhysicsComponent physic, float dt)
        {
            physic.Velocity += physic.Acceleration * dt;
        }
        private void UpdateLinearDeceleration(PhysicsComponent physic, float dt)
        {
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());

            Vector3 f = physic.Velocity;
            if (physic.Velocity != Vector3.Zero)
                f -= (physic.Velocity - physic.InitialVelocity) / dt;
            if (f.X < 0)
                f.X = 0;
            if (f.Z < 0)
                f.Z = 0;
            if(physic.IsFalling)
            {
                physic.Forces = new Vector3(physic.Forces.X, world.Gravity.Y, physic.Forces.Z);
            }
            physic.Velocity = new Vector3(f.X, physic.Velocity.Y, f.X);
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

        public void OnNext(List<Tuple<BoundingSphereComponent, BoundingSphereComponent>> value)
        {
            foreach (var val in value)
                UpdateReflection(ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item1.ID), ComponentManager.GetEntityComponent<PhysicsComponent>(val.Item2.ID));
        }

        public void OnError(Exception error)
        {
            //TODO: OnError
        }

        public void OnCompleted()
        {
            //TODO: OnCompleted
        }
    }
}
