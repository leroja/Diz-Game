﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.Interfaces;
using GameEngine.Source.Systems.AbstractClasses;

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
        /// (Rigid, projectiles, and static)
        /// in to the list.
        /// The list is then looped over in the update function which then updates the corresponding 
        /// system to the current component
        /// </summary>
        public PhysicsSystem()
        {
            physicSystems = new List<IPhysicsTypeSystem>
            {
                new PhysicsRigidBodySystem(this),
                new PhysicsProjectilesSystem(this),
                //new PhysicsStaticSystem(this)
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
            Parallel.ForEach(ents, ent =>
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(ent);
                if (physic.PhysicsType == PhysicsType.Static)
                    UpdateMass(physic);
                else
                {
                    physicSystems.Where(x => x.PhysicsType == physic.PhysicsType).SingleOrDefault().Update(physic, dt);
                }
                
            });
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
        /// Calculates the physic objects Deceleration
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
            if (!physic.IsMoving)
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
            //TODO: physic.Velocity += physic.InitialVelocity + ((physic.Acceleration - friction) * dt);
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
            switch (physic.GravityType)
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



        private void UpdateReflection2(PhysicsComponent target, PhysicsComponent hit)
        {
            if (target != null && hit != null)
            {
                float tmp = 1.0f / (target.Mass + hit.Mass);
                float e = 0.0f;
                e = hit.Bounciness * target.Bounciness;
                if (target.PhysicsType != PhysicsType.Static && hit.PhysicsType != PhysicsType.Static)
                {
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
                else if (target.PhysicsType != PhysicsType.Static && hit.PhysicsType == PhysicsType.Static)
                {
                    Vector3 velocity = (
                        (e + 1.0f) * hit.Mass * hit.Velocity +
                        target.Velocity * (target.Mass - (e * hit.Mass))
                        ) * tmp;
                    target.Velocity = velocity;
                }
                else if (target.PhysicsType == PhysicsType.Static && hit.PhysicsType != PhysicsType.Static)
                {
                    Vector3 velocity = (
                        (e + 1.0f) * target.Mass * target.Velocity +
                        hit.Velocity * (target.Mass - (e * target.Mass))
                        ) * tmp;
                    hit.Velocity = velocity;
                }
            }
        }

        /// <summary>
        /// Observer function, Updates the reflection on two objects 
        /// on collision (retrieves data from collision system)
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            UpdateReflection2(ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item1), ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item2));
            //UpdateReflection2(ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item2), ComponentManager.GetEntityComponent<PhysicsComponent>((int)value.Item1));
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
