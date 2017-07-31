using GameEngine.Source.Systems;
using GameEngine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using System.Threading.Tasks;
using DizGame.Source.Random_Stuff;
using DizGame.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// System handles moving of an object,
    /// using inheritance from IUpdate
    /// </summary>
    public class MovingSystem : IUpdate
    {
        private Border border;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="border"></param>
        public MovingSystem(Border border)
        {
            this.border = border;
        }

        /// <summary>
        /// Updates an objects movement(position) using
        /// TransformComponent, PhysicComponent and KeyboardComponent.
        /// Updating the velocity instead of the acceleration because
        /// we want and an constant speed instead of slow acceleration
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var EntityDict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<KeyBoardComponent>();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Parallel.ForEach(EntityDict, entity =>
            {
                KeyBoardComponent key = ComponentManager.GetEntityComponent<KeyBoardComponent>(entity.Key);
                TransformComponent trans = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                PhysicsComponent phys = ComponentManager.GetEntityComponent<PhysicsComponent>(entity.Key);
                var animComp = ComponentManager.GetEntityComponent<AnimationComponent>(entity.Key);
                var stamComp = ComponentManager.GetEntityComponent<StaminaComponent>(entity.Key);
                Vector3 move = Vector3.Zero;
                float gravity = 0;
                switch (phys.GravityType)
                {
                    case GravityType.Self:
                        gravity = phys.Gravity;
                        break;
                    case GravityType.World:
                        List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
                        WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
                        gravity = world.Gravity.Y;
                        break;
                    default:
                        break;
                }

                if (!phys.IsInAir)
                {
                    if (key.GetState("Forward") == ButtonStates.Hold && key.GetState("Sprint") == ButtonStates.Hold)
                    {
                        if ((stamComp.IsRunning && stamComp.StaminaRatio > 0) || (!stamComp.IsRunning && stamComp.StaminaRatio >= stamComp.RunThreshold))
                        {
                            move += trans.Forward * 40;
                            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds * 2);
                            stamComp.IsRunning = true;
                        }
                        else
                        {
                            stamComp.IsRunning = false;
                            move += trans.Forward * 20;
                            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
                        }
                    }
                    else if (key.GetState("Forward") == ButtonStates.Hold)
                    {
                        stamComp.IsRunning = false;
                        move += trans.Forward * 20;
                        animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        stamComp.IsRunning = false;
                    }
                    if (key.GetState("Backwards") == ButtonStates.Hold)
                    {
                        move += -trans.Forward * 20;
                        animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    if (key.GetState("Left") == ButtonStates.Hold)
                    {
                        move += -trans.Right * 20;
                        //animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds); // TODO hur ska vi göra när man går åt sidan?
                    }
                    if (key.GetState("Right") == ButtonStates.Hold)
                    {
                        move += trans.Right * 20;
                        //animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    if (key.GetState("Up") == ButtonStates.Hold)
                    {
                        if (!phys.IsInAir)
                        {
                            //move = phys.Velocity;
                            phys.IsInAir = true;
                            //phys.Velocity += trans.Up ;
                            if (phys.Velocity.X == 0 && phys.Velocity.Z == 0)
                                phys.Velocity += trans.Up * -gravity * 7;
                            else
                                phys.Velocity = new Vector3(phys.Velocity.X, -gravity * 7, phys.Velocity.Z);
                        }
                    }
                }
#if !DEBUG
                if (trans.Position.X >= border.HighX)
                {
                    trans.Position = trans.PreviousPosition;
                    move = -move;
                }
                else if (trans.Position.X <= border.LowX)
                {
                    trans.Position = trans.PreviousPosition;
                    move = -move;
                }
                else if (trans.Position.Z <= border.HighZ)
                {
                    trans.Position = trans.PreviousPosition;
                    move = -move;
                }
                else if (trans.Position.Z >= border.LowZ)
                {
                    trans.Position = trans.PreviousPosition;
                    move = -move;
                }
#endif
                if (phys != null)
                {
                    float he = GetHeight(trans.Position);

                    if (!phys.IsInAir)
                        phys.Velocity = move;
                    //phys.Acceleration = CheckMaxVelocityAndGetVector(phys, move);


                    //Console.WriteLine(phys.Acceleration);

                    if (he != trans.Position.Y && !phys.IsInAir)
                    {
                        phys.Acceleration = new Vector3(phys.Acceleration.X, 0, phys.Acceleration.Z);
                    }

                    if (he >= trans.Position.Y || he <= trans.Position.Y && !phys.IsInAir)
                    {
                        phys.IsInAir = false;
                        trans.Position = new Vector3(trans.Position.X, he, trans.Position.Z);
                        phys.Acceleration = Vector3.Zero;
                        phys.Velocity = new Vector3(phys.Velocity.X, 0, phys.Velocity.Z);
                    }
                }
                else
                {
                    float he = GetHeight(trans.Position);
                    trans.Position = new Vector3(trans.Position.X, he, trans.Position.Z);
                }

            });
        }
        private Vector3 CheckMaxVelocityAndGetVector(PhysicsComponent physic, Vector3 move)
        {
            float X = move.X, Y = move.Y, Z = move.Z;
            if (physic.Velocity.X >= physic.MaxVelocity.X || physic.Velocity.X <= -physic.MaxVelocity.X)
                X = physic.Acceleration.X;

            if (physic.Velocity.Z >= physic.MaxVelocity.Z || physic.Velocity.Z <= -physic.MaxVelocity.Z)
                Z = physic.Acceleration.Z;
            return new Vector3(X, Y, Z);
        }

        /// <summary>
        /// Function to get the height of the current position
        /// using BarryCentric for an exact height in the current 
        /// triangle
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponent>();
            if (temp.Count != 0)
            {
                HeightmapComponent hmap = ComponentManager.Instance.GetEntityComponent<HeightmapComponent>(temp.First());
                float gridSquareSize = (hmap.Width * hmap.Height) / ((float)hmap.HeightMapData.Length - 1);
                int gridX = (int)Math.Floor(position.X / gridSquareSize);
                int gridZ = -(int)Math.Floor(position.Z / gridSquareSize);

                if (gridX >= hmap.HeightMapData.Length - 1 || gridZ >= hmap.HeightMapData.Length - 1 || gridX < 0 || gridZ < 0)
                {
                    return 0;
                }
                float xCoord = (position.X % gridSquareSize) / gridSquareSize;
                float zCoord = (position.Z % gridSquareSize) / gridSquareSize;
                float answer = 0;

                if (xCoord <= (1 - zCoord))
                {
                    answer = BarryCentric(
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ], 0),
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ], 0),
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ + 1], 1),
                        new Vector2(xCoord, zCoord));
                }
                else
                {
                    answer = BarryCentric(
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ], 0),
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ + 1], 1),
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ + 1], 1),
                        new Vector2(xCoord, zCoord));
                }
                return answer;
            }
            return 0;
        }

        private static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }
    }
}