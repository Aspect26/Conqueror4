namespace Server
{
    /// <summary>
    /// Represents a player action. Everytime an information about a player's 
    /// character's action is received by the sever, it creates a corresponding
    /// IPlayerAction inherited object and enqueues it to the game (map instance)
    /// action queue to be processed in the next game cycle.
    /// </summary>
    public interface IPlayerAction
    {
        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="mapInstance">The map instance in which it happened.</param>
        /// <param name="timeStamp">The time stamp.</param>
        void Process(MapInstance mapInstance, long timeStamp);

        /// <summary>
        /// Gets the character this action is associated with.
        /// </summary>
        /// <returns>Character.</returns>
        Character GetCharacter();
    }
}
