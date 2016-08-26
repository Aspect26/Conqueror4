namespace Server
{
    /// <summary>
    /// Represents a game tiemdd action. It is not really 'timed' thanks to this
    /// interface but the way it is used in the MapInstance's code.
    /// </summary>
    /// <seealso cref="MapInstance" />
    public interface ITimedAction
    {
        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="map">The map instance it belongs to.</param>
        void Process(MapInstance map);
    }
}
