using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// System handles moving of an object,
    /// using inheritance from IUpdate
    /// </summary>
    public class MovingSystem : IUpdate
    {
        /// <summary>
        /// Updates an objects movement(position) using
        /// Transformcomponents, PhysicComponent and KeyboardComponent.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Dictionary<int, IComponent> EntityDict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<KeyBoardComponent>();

            foreach (var entity in EntityDict)
            {
                KeyBoardComponent key = ComponentManager.GetEntityComponent<KeyBoardComponent>(entity.Key);
                TransformComponent trans = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                PhysicsComponent phys = ComponentManager.GetEntityComponent<PhysicsComponent>(entity.Key);
                Vector3 move = Vector3.Zero;
                if (!phys.IsInAir)
                {
                    if (key.GetState("Forward") == ButtonStates.Hold)
                    {
                        if (phys != null)
                            move += trans.Forward * 10; //+ new Vector3(0,0,-(phys.Mass * phys.Acceleration.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds) * PhysicsComponent.DEFAULT_WALKFORCE;
                        else
                            //Console.WriteLine(trans.Position);
                            trans.Position += trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                        trans.Dirrection = trans.Forward;
                    }
                    if (key.GetState("Backwards") == ButtonStates.Hold)
                    {
                        if (phys != null)
                            move += -trans.Forward * 10;// + new Vector3(0, 0, (phys.Mass * phys.Acceleration.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds) * PhysicsComponent.DEFAULT_WALKFORCE;
                        else
                            trans.Position -= trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                        trans.Dirrection = -trans.Forward;
                    }
                    if (key.GetState("Left") == ButtonStates.Hold)
                    {
                        if (phys != null)
                            move += -trans.Right * 10;// + new Vector3(-(phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0) * PhysicsComponent.DEFAULT_WALKFORCE;
                        else
                            trans.Position -= trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                        trans.Dirrection = -trans.Right;
                    }
                    if (key.GetState("Right") == ButtonStates.Hold)
                    {
                        if (phys != null)
                            move += trans.Right * 10;// + new Vector3((phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0) * PhysicsComponent.DEFAULT_WALKFORCE;
                        else
                            trans.Position += trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                        trans.Dirrection = trans.Right;
                    }
                    if (key.GetState("Up") == ButtonStates.Hold)
                    {
                        if (phys != null)
                        {
                            if (!phys.IsInAir)
                            {
                                phys.IsInAir = true;
                                move += trans.Up * 8.91f * 250;// + new Vector3(0, (phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0) * PhysicsComponent.DEFAULT_LEGFORCE/1000;
                            }
                        }
                        trans.Dirrection = trans.Up;
                    }
                }
                float he = BasicGetHeight(trans.Position);
                if (phys != null)
                {
                    trans.Dirrection = new Vector3(trans.Dirrection.X, Vector3.Down.Y, trans.Dirrection.Z);
                    phys.Acceleration = move;
                    //phys.Acceleration = CheckMaxVelocityAndGetVector(phys, move);



                    if (he != trans.Position.Y && !phys.IsInAir)
                    {
                        phys.Acceleration = new Vector3(phys.Acceleration.X, 0, phys.Acceleration.Z);
                    }

                    if (he >= trans.Position.Y)
                    {
                        phys.IsInAir = false;
                        trans.Position = new Vector3(trans.Position.X, he, trans.Position.Z);
                    }
                }
                else
                {
                    trans.Position = new Vector3(trans.Position.X, he, trans.Position.Z);
                }
            }
        }
        private Vector3 CheckMaxVelocityAndGetVector(PhysicsComponent physic, Vector3 move)
        {
            float X = move.X, Y = move.Y, Z = move.Z;
            if (physic.Velocity.X >= physic.MaxVelocity.X || physic.Velocity.X <= -physic.MaxVelocity.X)
                X = 0;

            if (physic.Velocity.Z >= physic.MaxVelocity.Z || physic.Velocity.Z <= -physic.MaxVelocity.Z)
                Z = 0;
            return new Vector3(X, Y, Z);
        }

        private float BasicGetHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            if (temp.Count != 0)
            {
                HeightmapComponentTexture hmap = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(temp.First());

                int roundX = (int)Math.Round(position.X); int roundY = (int)Math.Round(position.Z);
                if (roundX >= hmap.Width - 1 || roundY >= hmap.Height - 1)
                {
                    return 0;
                }
                if (roundY <= 0 && roundX >= 0)
                    return hmap.HeightMapData[roundX, -roundY];
            }
                return 0;
        }
    }
}
