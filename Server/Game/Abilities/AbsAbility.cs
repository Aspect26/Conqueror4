using System;

namespace Server
{
    /// <summary>
    /// An abstract base class for every ability. Every ability shall at least
    /// inherit from this class. It takes care of fields and reducing caster's
    /// mana points when the abillity is used.
    /// </summary>
    /// <seealso cref="Server.IAbility" />
    public abstract class Ability : IAbility
    {
        /// <summary>
        /// Gets the mana cost of the ability.
        /// </summary>
        /// <value>The mana cost.</value>
        public int ManaCost { get; }

        /// <summary>
        /// Gets the identifier of the ability.
        /// </summary>
        /// <value>The identifier.</value>
        public abstract int ID { get; }

        /// <summary>
        /// Gets the unit that useed this ability.
        /// </summary>
        /// <value>The unit.</value>
        public IUnit Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class with specified
        /// caster and mana cost.
        /// </summary>
        /// <param name="source">The caster.</param>
        /// <param name="manaCost">The mana cost.</param>
        public Ability(IUnit source, int manaCost)
        {
            this.Source = source;
            this.ManaCost = manaCost;
        }

        /// <summary>
        /// Processes the ability. This base implementation reduces the caster's mana
        /// points by the mana cost amount.
        /// </summary>
        /// <param name="mapInstance">The map instance in which the ability
        /// was used.</param>
        public virtual void Process(MapInstance mapInstance)
        {
            Source.DecreaseActualManaPoints(ManaCost);
        }

        /// <summary>
        /// Gets coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string GetCodedData();
    }
}
