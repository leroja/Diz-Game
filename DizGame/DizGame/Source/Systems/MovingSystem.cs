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
    public class MovingSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            Dictionary<int, IComponent> EntityDict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<KeyBoardComponent>();

            foreach(var entity in EntityDict)
            {
                KeyBoardComponent key = ComponentManager.GetEntityComponent<KeyBoardComponent>(entity.Key);
                TransformComponent trans = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                PhysicsComponent phys = ComponentManager.GetEntityComponent<PhysicsComponent>(entity.Key);

                Vector3 move = Vector3.Zero;

                if (key.State["Forward"] == ButtonStates.Hold)
                {
                    if (phys != null)
                        move += Vector3.Forward * 10; //+ new Vector3(0,0,-(phys.Mass * phys.Acceleration.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds) * PhysicsComponent.DEFAULT_WALKFORCE;
                    else
                        trans.Position += trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds * 20;
                    trans.Dirrection = Vector3.Forward;
                }
                if (key.State["Backwards"] == ButtonStates.Hold)
                {
                    if (phys != null)
                        move += Vector3.Backward * 10;// + new Vector3(0, 0, (phys.Mass * phys.Acceleration.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds) * PhysicsComponent.DEFAULT_WALKFORCE;
                    else
                        trans.Position -= trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
                    trans.Dirrection = Vector3.Backward;
                }
                if (key.State["Left"] == ButtonStates.Hold)
                {
                    if (phys != null)
                        move += Vector3.Left * 10;// + new Vector3(-(phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0) * PhysicsComponent.DEFAULT_WALKFORCE;
                    else
                        trans.Position -= trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
                    trans.Dirrection = Vector3.Left;
                }
                if (key.State["Right"] == ButtonStates.Hold)
                {
                    if (phys != null)
                        move += Vector3.Right * 10;// + new Vector3((phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0) * PhysicsComponent.DEFAULT_WALKFORCE;
                    else
                        trans.Position += trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
                    trans.Dirrection = Vector3.Right;
                }
                if (key.State["Up"] == ButtonStates.Hold)
                {
                    if (phys != null)
                        move += Vector3.Up * 8.91f * 10;// + new Vector3(0, (phys.Mass * phys.Acceleration.X) * (float)gameTime.ElapsedGameTime.TotalSeconds, 0) * PhysicsComponent.DEFAULT_LEGFORCE/1000;
                    trans.Dirrection = Vector3.Up;
                }
                if (phys != null)
                {
                    move.Y += phys.Forces.Y;
                    trans.Dirrection = Vector3.Down;
                    phys.Forces = move;
                    if (move == Vector3.Zero)
                    {
                        phys.IsMoving = false;
                        phys.IsFalling = true;
                    }
                    else
                        phys.IsMoving = true;
                }

            }
        }
    }
}
