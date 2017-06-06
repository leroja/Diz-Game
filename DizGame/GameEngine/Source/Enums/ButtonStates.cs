namespace GameEngine.Source.Enums
{
    /// <summary>
    /// An enum for the states of keyboard keys
    /// </summary>
    public enum ButtonStates
    {
        /// <summary>
        /// For holding down a Key
        /// </summary>
        Hold,
        /// <summary>
        /// A state for when the key just have been pressed
        /// </summary>
        Pressed,
        /// <summary>
        /// A state for when the key has just been released
        /// </summary>
        Released,
        /// <summary>
        /// A state for when the key has not been pressed
        /// </summary>
        Not_Pressed
    };
}
