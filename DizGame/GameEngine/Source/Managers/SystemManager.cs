using GameEngine.Source.Systems.Abstract_classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Managers
{
    /// <summary>
    /// A manager that stores the different systems in gameEngine and game
    /// </summary>
    public class SystemManager
    {
        private static SystemManager instance;

        public GameTime GameTime { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice Device { get; set; }
        
        List<IRender> renderSystems = new List<IRender>();
        List<IUpdate> updateSystems = new List<IUpdate>();

        private SystemManager()
        {
        }

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
        /// Runs all updateable systems
        /// </summary>
        public void RunUpdateSystems()
        {
            if (updateSystems.Count > 0)
            {
                foreach (IUpdate system in updateSystems)
                {
                    system.Update(GameTime);
                }
            }
        }

        /// <summary>
        /// Runs all rendering systems
        /// </summary>
        public void RunRenderSystems()
        {
            if (renderSystems.Count > 0)
            {
                foreach (IRender system in renderSystems)
                {
                    system.Draw(GameTime);
                }
            }
        }
    }
}
