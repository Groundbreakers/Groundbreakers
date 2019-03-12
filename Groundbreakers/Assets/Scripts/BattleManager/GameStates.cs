namespace Assets.Scripts
{
    /// <summary>
    ///     The stages of the game phases.
    /// </summary>
    public enum GameStates
    {
        /// <summary>
        ///     The Initial State
        /// </summary>
        Null,

        /// <summary>
        ///     The state when all tiles should enter.
        /// </summary>
        Entering,

        /// <summary>
        ///     The state when combat should be resolved.
        /// </summary>
        Combating,

        /// <summary>
        ///     The state when all tiles should leave the map.
        /// </summary>
        Exiting
    }
}