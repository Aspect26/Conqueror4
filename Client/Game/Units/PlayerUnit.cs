using Shared;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Implementation of IUnit interface for every character controlled by a player.
    /// It differs only in the fact that IsPlayer function returns true.
    /// </summary>
    /// <seealso cref="Client.GenericUnit" />
    public class PlayerUnit : GenericUnit
    {
        private string name;
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerUnit"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="name">The name.</param>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="maxStats">The maximum stats.</param>
        /// <param name="actualStats">The actual stats.</param>
        /// <param name="fraction">The fraction.</param>
        public PlayerUnit(Game game, string name, int unitId, int uniqueId, int x, int y, BaseStats maxStats,
            BaseStats actualStats, int fraction)
            : base(game, unitId, uniqueId, new Location(x, y), maxStats, actualStats, fraction)
        {
            this.name = name;
            this.game = game;
            this.nameBrush = Brushes.Blue;
        }

        /// <summary>
        /// Determines whether this unit is a player.
        /// It always returns true because this implementation is used only for players's
        /// characters.
        /// </summary>
        /// <returns><c>true</c> if this instance is a player; otherwise, <c>false</c>.</returns>
        public override bool IsPlayer()
        {
            return true;
        }
    }
}
