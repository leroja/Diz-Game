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
        /// Updating the velocity instead of the acceleration because
        /// we want and an constant speed instead of slow accleration
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
                        move += trans.Forward * 20;
                    }
                    if (key.GetState("Backwards") == ButtonStates.Hold)
                    {
                        move += -trans.Forward * 20;
                    }
                    if (key.GetState("Left") == ButtonStates.Hold)
                    {
                            move += -trans.Right * 20;
                    }
                    if (key.GetState("Right") == ButtonStates.Hold)
                    {
                            move += trans.Right * 20;
                    }
                    if (key.GetState("Up") == ButtonStates.Hold)
                    {
                        if (!phys.IsInAir)
                        {
                            phys.IsInAir = true;
                            move += trans.Up * 8.91f * 20;
                        }
                    }
                }
                if (phys != null)
                {
                    float he = GetHeight(trans.Position);

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
            }
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
        private float GetHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            if (temp.Count != 0)
            {
                HeightmapComponentTexture hmap = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(temp.First());
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

                if(xCoord <= (1 - zCoord))
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
                        new Vector2(xCoord,zCoord));
                }
                return answer;
            }
            return 0;
        }
        private float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }
    }
}
