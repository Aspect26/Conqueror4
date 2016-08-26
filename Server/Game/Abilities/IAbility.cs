namespace Server
{
    /// <summary>
    /// A simple interface for every game ability.
    /// The abilities here are very simple. If a unit uses an ability, and action
    /// is added to the game's (map instance's) action queue and in the next cycle
    /// the ability is immediatelly poccessed. This also means that currently it is not
    /// possible (using this interface) to implement an ability that takes effect after
    /// some time. Note that it is possible to implement ability that takes after over
    /// a time, it would simply add a new game action containing itself to the action
    /// queue every time the Process function is called until its effect runs out.
    /// </summary>
    public interface IAbility
    {
        /// <summary>
        /// Gets the unit that useed this ability.
        /// </summary>
        /// <value>The unit.</value>
        IUnit Source { get; }

        /// <summary>
        /// Gets the mana cost of the ability.
        /// </summary>
        /// <value>The mana cost.</value>
        int ManaCost { get; }

        /// <summary>
        /// Gets the identifier of the ability.
        /// </summary>
        /// <value>The identifier.</value>
        int ID { get; }

        /// <summary>
        /// Processes the ability.
        /// </summary>
        /// <param name="mapInstance">The map instance in which the ability 
        /// was used.</param>
        void Process(MapInstance mapInstance);

        /// <summary>
        /// Gets coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetCodedData();
    }
}
