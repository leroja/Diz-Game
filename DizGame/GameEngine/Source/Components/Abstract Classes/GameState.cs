using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine.Source.Components.Abstract_Classes
{
    /// <summary>
    /// Each GameState in the game should inherit from this class and 
    /// implement the requested methods to succesfully use the GameStateManager
    /// with each of the GameStates. 
    /// </summary>
    public abstract class GameState
    {
        /// <summary>
        /// List for the indecies for the entities added in the gamestate
        /// to keep track, is needed if we want to obscure and reveal states
        /// instead of adding and removing all entities whenever we leave or enter
        /// a gamestate.
        /// </summary>
        public abstract List<int> GameStateEntities { get; }
        /// <summary>
        /// should be called after the game state has been placed in the game state manager.
        /// could for example be used to initialize appropriate entities, components and systems (?)
        /// </summary>
        public abstract void Entered();
        /// <summary>
        /// shoudl be called right before the game state is removed (poped) from the game state manager,
        /// in other words the state is no longer the current state.
        /// </summary>
        public abstract void Exiting();
        /// <summary>
        /// this method is called right before another game state is stacked on top of this one.
        /// For example this could be whenever a pause state is pushed ontop of the world game state, 
        /// or an inventory screen is entered by the user. Then we dont want to destroy the entities/components
        /// in the previous state, we just dont wanna "show them".
        /// </summary>
        public abstract void Obscuring();
        /// <summary>
        /// called after the game state has become the topmost game state on the stack again,
        /// this is called as an example when the user exits the pause or inventory screen
        /// we might want to show the game as it was when the user last saw this state.
        /// </summary>
        public abstract void Revealed();

        /// <summary>
        /// Should contain the Gamestates logic incase of exiting,
        /// pausing or entering a new state, this might depend on the gamestate itself
        /// </summary>
        public abstract void Update(GameTime gameTime);
    }
}
