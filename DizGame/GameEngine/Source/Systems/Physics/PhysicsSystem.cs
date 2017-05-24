using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.Interfaces;
using GameEngine.Source.Systems.AbstractClasses;
using AnimationContentClasses;

namespace GameEngine.Source.Systems
{
    
    //TODO: Fixa så alla siffror är korrekta efter metric.
    /// <summary>
    /// PhysicSystem which handles all the physic
    /// </summary>
    public class PhysicsSystem : IUpdate, IPhysics, IObserver<Tuple<object, object>>
    {
        float dt = 0;
        /// <summary>
        /// List which is then looped over and updates the systems
        /// </summary>
        private List<IPhysicsTypeSystem> physicSystems;
        /// <summary>
        /// Constructor which adds the basic physicsystems 
        /// (Rigid, particle, projectiles, ragdoll, soft and static)
        /// in to the list.
        /// The list is then looped over in the update function which then updates the corresponding 
        /// system to the current component
        /// </summary>
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
        /// <summary>
        /// Function to add new PhysicTypeSystem into the list
        /// </summary>
        /// <param name="system"></param>
        public void AddIPhysicsTypeSystem(IPhysicsTypeSystem system)
        {
            physicSystems.Add(system);
        }
        /// <summary>
        /// Function to remove PhysicTypeSystems from the list
        /// </summary>
        /// <param name="system"></param>
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
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var ents = ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>();
            Parallel.ForEach(ents, ent => {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(ent);
                physicSystems.Where(x => x.PhysicsType == physic.PhysicsType).SingleOrDefault().Update(physic, dt);
            });
            //foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            //{
            //    PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
            //    physicSystems.Where(x => x.PhysicsType == physic.PhysicsType).SingleOrDefault().Update(physic, dt);
            //}
            //CheckCollision(dt);
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
            if (physic.Mass == 0)
                physic.InverseMass = 0;
            else
                physic.InverseMass = 1 / physic.Mass;
        }
        /// <summary>
        /// Updates the objects Euler acceleration
        /// This function should be updated BEFORE position,
        /// and position updating should use LastAcceleration
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateEulerAcceleration(PhysicsComponent physic)
        {
            physic.LastAcceleration = physic.Acceleration;
            Vector3 new_acceleration = physic.Forces / physic.Mass;
            Vector3 avg_acceleration = (physic.LastAcceleration + new_acceleration) / 2;
            physic.Acceleration = avg_acceleration;
        }
        /// <summary>
        /// Calculates the physic objects Deaceleration
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateDeceleration(PhysicsComponent physic)
        {
            if (!physic.IsInAir)
            {
                if (physic.Acceleration.X == 0)
                    physic.Velocity = new Vector3(0, physic.Velocity.Y, physic.Velocity.Z);
                if (physic.Acceleration.Y == 0)
                    physic.Velocity = new Vector3(physic.Velocity.X, 0, physic.Velocity.Z);
                if (physic.Acceleration.Z == 0)
                    physic.Velocity = new Vector3(physic.Velocity.X, physic.Velocity.Y, 0);
            }
            if(!physic.IsMoving)
            {
                float X = physic.Acceleration.X, Y = physic.Acceleration.Y, Z = physic.Acceleration.Z;
                if (physic.Acceleration.X < 0)
                    X *= physic.Friction;
                else
                    X *= -physic.Friction;

                if (physic.Acceleration.Y < 0)
                    Y *= physic.Friction;
                else
                    Y *= -physic.Friction;

                if (physic.Acceleration.Z < 0)
                    Z *= physic.Friction;
                else
                    Z *= -physic.Friction;
            }
            //Console.WriteLine(physic.Acceleration);
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
            physic.Velocity += physic.Acceleration * dt;
            //TODO: physic.Velocity += physic.InitialVelocity + ((physic.Acceleration - frictio) * dt);
            //Console.WriteLine(physic.Velocity);
        }
        /// <summary>
        /// Updates the forces
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateForce(PhysicsComponent physic)
        {
            physic.Forces = physic.Mass * physic.Acceleration;
        }
        /// <summary>
        /// Updates the Acceleration using formula 
        /// A = F/M
        /// </summary>
        /// <param name="physic"></param>
        public virtual void UpdateAcceleration(PhysicsComponent physic)
        {
            physic.Acceleration = (physic.Forces / physic.Mass);
        }
        /// <summary>
        /// Updates the gravity depending on physics gravity type
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public virtual void UpdateGravity(PhysicsComponent physic, float dt)
        {
            switch(physic.GravityType)
            {
                case GravityType.Self:
                    physic.Acceleration = new Vector3(physic.Acceleration.X, physic.Gravity, physic.Acceleration.Z);
                    break;
                case GravityType.World:
                    List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
                    WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
                    physic.Acceleration = new Vector3(physic.Acceleration.X + world.Gravity.X, 
                        physic.Acceleration.Y + world.Gravity.Y, 
                        physic.Acceleration.Z + world.Gravity.Z);
                    break;
            }
                
        }
        /// <summary>
        /// Updates the objects heading depending on collision
        /// </summary>
        /// <param name="target"></param>
        /// <param name="hit"></param>
        public virtual void UpdateReflection(PhysicsComponent target, PhysicsComponent hit)
        {
            if (target != null && hit != null)
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
        }
        private void UpdateReflection2(PhysicsComponent target, PhysicsComponent hit, float dt)
        {
            if (target != null && hit != null)
            {
                if (target.PhysicsType != PhysicsType.Static && hit.PhysicsType != PhysicsType.Static)
                {
                    float tmp = 1.0f / (target.Mass + hit.Mass);
                    float e = 0.0f;

                    Vector3 velocity1 = (
                        (e + 1.0f) * hit.Mass * hit.Velocity +
                        target.Velocity * (target.Mass - (e * hit.Mass))
                        ) * tmp;

                    Vector3 velocity2 = (
                        (e + 1.0f) * target.Mass * target.Velocity +
                        hit.Velocity * (hit.Mass - (e * target.Mass))
                        ) * tmp;

                    target.Velocity = velocity1;
                    hit.Velocity = velocity2;
                }
                else if(target.PhysicsType != PhysicsType.Static && hit.PhysicsType == PhysicsType.Static)
                {
                    Vector3 dir = target.Velocity;
                    dir.Normalize();
                    //ComponentManager.GetEntityComponent<TransformComponent>(target.ID).Position += (-dir * 2) * target.Velocity * dt;
                                        //target.Velocity = Vector3.Zero;
                    target.Velocity *= -dir * new Vector3(1, 0, 1);
                    ComponentManager.GetEntityComponent<TransformComponent>(target.ID).Position *= target.Velocity * dt;
                                        //Console.WriteLine(ComponentManager.GetEntityComponent<TransformComponent>(target.ID).Position);
                    
                                        //Console.WriteLine(target.Velocity);
                     // TODO: Fixa collisionen
                }
            }
        }
        private void CheckCollision(float dt)
        {
            List<int> done = new List<int>();
            foreach (int entityIDUno in ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>())
            {
                ModelComponent model = ComponentManager.GetEntityComponent<ModelComponent>(entityIDUno);
                if (model.BoundingVolume == null)
                    continue;
                foreach (int entityIDDos in ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>())
                {
                    if (entityIDUno == entityIDDos)
                        continue;

                    ModelComponent model2 = ComponentManager.GetEntityComponent<ModelComponent>(entityIDDos);
                    if (model2.BoundingVolume == null)
                        continue;

                    if (model.BoundingVolume.Bounding.Intersects(model2.BoundingVolume.Bounding) && !done.Contains(entityIDDos))
                        UpdateReflection2(ComponentManager.GetEntityComponent<PhysicsComponent>(entityIDUno), ComponentManager.GetEntityComponent<PhysicsComponent>(entityIDDos), dt);

                }
                done.Add(entityIDUno);
            }
        }
        private bool IsPointInsideAABB(TransformComponent transform, BoundingBox box)
        {
            return (transform.Position.X >= box.Min.X && transform.Position.X <= box.Max.X) &&
                (transform.Position.Y >= box.Min.Y && transform.Position.Y <= box.Max.Y) &&
                (transform.Position.Z >= box.Min.Z && transform.Position.Z <= box.Max.Z);
        }
        private bool IsPointInsideSphere(TransformComponent tranform, BoundingSphere sphere)
        {
            var distance = Math.Sqrt((tranform.Position.X - sphere.Center.X) * (tranform.Position.X - sphere.Center.X) +
                (tranform.Position.Y - sphere.Center.Y) * (tranform.Position.Y - sphere.Center.Y) +
                (tranform.Position.Z - sphere.Center.Z) * (tranform.Position.Z - sphere.Center.Z));
            return distance < sphere.Radius;
        }
        /// <summary>
        /// Observer funktion, Updates the reflection on two objects 
        /// on collision (retrieves data from collision system)
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            UpdateReflection2(ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item1), ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item2), 1);
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
