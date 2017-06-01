using GameEngine.Source.Managers;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for the systems in the game engine and the game
    /// </summary>
    public abstract class ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        protected ComponentManager ComponentManager { get; } = ComponentManager.Instance;
    }
}
