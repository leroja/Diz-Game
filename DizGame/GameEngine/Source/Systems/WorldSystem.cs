using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Factories;
using GameEngine.Source.RandomStuff;
using System.Threading;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System that updates the world using 
    /// derived from IUpdate
    /// </summary>
    public class WorldSystem : IUpdate
    {
        private Game game;
        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="game"></param>
        public WorldSystem(Game game)
        {
            this.game = game;
            ComponentManager.AddComponentToEntity(ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>()[0],
                new TextComponent("WorldTime", 
                new Vector2(game.GraphicsDevice.Viewport.Width/2 - 50, 0), 
                Color.White, 
                game.Content.Load<SpriteFont>("Fonts/font"), 
                true));
        }

        /// <summary>
        /// Updates the worldComponents.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>())
            {
                UpdateTime(entityID, gameTime);
                UpdateSun(entityID, dt);
            }
        }

        /// <summary>
        /// Update the World time using the gameTime
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="gameTime"></param>
        private void UpdateTime(int entityID, GameTime gameTime)
        {
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(entityID);
            
            if (world.Noon && world.Hour == 12 && world.Second > 1)
                world.Noon = false;

            world.Hour += (float)gameTime.ElapsedGameTime.TotalSeconds / world.DefineHour;
            world.Minute += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 / world.DefineHour;
            world.Second += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 * 60 / world.DefineHour;
            world.Millisecond += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 * 60 * 60 / world.DefineHour;

            if (world.Minute > 60)
                world.Minute = 0;
            if (world.Second > 60)
                world.Second = 0;
            if (world.Millisecond > 100)
                world.Millisecond = 0;

            if (world.Hour > (int)world.Notation)
            {
                world.Hour = 0;
                if (world.Notation == WorldComponent.ClockNotation.HOURS12 && !world.Noon)
                    world.Noon = true;
                else if (world.Notation == WorldComponent.ClockNotation.HOURS12 && world.Noon)
                {
                    world.Noon = false;
                    world.Day++;
                }
                if (world.Notation == WorldComponent.ClockNotation.HOURS24)
                    world.Day++;
            }
            UpdateClockText(world);
        }

        private void UpdateClockText(WorldComponent world)
        {
            TextComponent text = ComponentManager.GetEntityComponent<TextComponent>(world.ID);
            string hour = Math.Floor(world.Hour).ToString(), minute = Math.Floor(world.Minute).ToString(), second = Math.Floor(world.Second).ToString();
            if (Math.Floor(world.Hour) < 9.99f)
                hour = "0" + Math.Floor(world.Hour);
            if (Math.Floor(world.Minute) < 9.99f)
                minute = "0" + Math.Floor(world.Minute);
            if (Math.Floor(world.Second) < 9.99f)
                second = "0" + Math.Floor(world.Second);

            text.Text = "Day: " + world.Day + "\n" + hour + ":" + minute + ":" + second;
        }
        private float c = 0;
        private bool reverse = false;
        /// <summary>
        /// Updates the sun rotation
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdateSun(int entityID, float dt)
        {
            // TODO: fixa denna
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(entityID);
            FlareComponent flare = null;
            if (world.IsSunActive)
            {
                flare = ComponentManager.GetEntityComponent<FlareComponent>(world.ID);
                if (flare != null)
                {
                    if (c > 1)
                    {
                        flare.LightDirection = new Vector3(flare.LightDirection.X,
                            flare.LightDirection.Y * 1.1f,
                            flare.LightDirection.Z * 1.1f);
                        //flare.LightDirection.Normalize();
                        //Console.WriteLine(flare.LightDirection);
                        c = 0;
                    }
                    c += dt;
                    if (flare.LightDirection.Z > 10)
                    {
                        flare.LightDirection = new Vector3(flare.LightDirection.X,
                            flare.StartLightDirection.Y,
                            flare.StartLightDirection.Z);
                    }
                }
            }
            else
            {
                flare = ComponentManager.GetEntityComponent<FlareComponent>(world.ID);
                if (flare != null)
                {
                    flare.IsActive = false;
                }
            }
        }
    }
}
