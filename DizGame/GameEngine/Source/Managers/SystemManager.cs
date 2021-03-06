﻿using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameEngine.Source.Managers
{
    /// <summary>
    /// A manager that stores the different systems in gameEngine and game
    /// </summary>
    public class SystemManager
    {
        private static SystemManager instance;

        /// <summary>
        /// A spriteBatch
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        List<IRender> renderSystems = new List<IRender>();
        static List<IUpdate> updateSystems = new List<IUpdate>();

        private SystemManager()
        {

        }

        /// <summary>
        /// The instance of the Manager
        /// </summary>
        public static SystemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Adds a system
        /// </summary>
        /// <param name="system"> The system that is going to be added </param>
        public void AddSystem(ISystem system)
        {
            Type type = system.GetType();
            if (system is IUpdate)
            {
                updateSystems.Add((IUpdate)system);
            }
            if (system is IRender)
            {
                renderSystems.Add((IRender)system);
            }
        }

        /// <summary>
        /// Function for clearing the systems in SystemManager
        /// </summary>
        public void ClearSystems()
        {
            updateSystems.Clear();
            renderSystems.Clear();
        }

        /// <summary>
        /// Removes a system 
        /// </summary>
        /// <param name="system"> The system that is to be removed </param>
        public void RemoveSystem(ISystem system)
        {
            Type type = system.GetType();
            if (system is IUpdate)
            {
                if (updateSystems.Contains(system))
                    updateSystems.Remove((IUpdate)system);
            }
            if (system is IRender)
            {
                if (renderSystems.Contains(system))
                    renderSystems.Remove((IRender)system);
            }
        }

        /// <summary>
        /// A method for retrieving a specific system
        /// </summary>
        /// <typeparam name="T"> The type of the system. e.g Update or Render </typeparam>
        /// <param name="system"> the name of the system. e.g CameraSystem </param>
        /// <returns></returns>
        public ISystem RetrieveSystem<T>(string system) where T : ISystem
        {
            Type type = typeof(T);
            ISystem sys;
            if (type.Equals(typeof(IUpdate)))
            {

                sys = updateSystems.Find(x => x.ToString().Contains(system));
                return (sys);
            }
            if (type.Equals(typeof(IRender)))
            {
                sys = renderSystems.Find(x => x.ToString().Contains(system));
                return (sys);
            }
            return default(T);
        }

        /// <summary>
        /// Runs all updatable systems
        /// </summary>
        //public void RunUpdateSystems()
        public void RunUpdateSystems(GameTime gameTime)
        {
            foreach (IUpdate system in updateSystems)
            {
                system.Update(gameTime);
            }
            GameStateManager.Instance.UpdateGameState(gameTime);
        }
        /// <summary>
        /// Runs all rendering systems
        /// </summary>
        public void RunRenderSystems(GameTime gameTime)
        {
            for (int i = 0; i < renderSystems.Count; i++)
            {
                renderSystems[i].Draw(gameTime);
            }
        }
    }
}