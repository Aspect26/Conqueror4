using Shared;
using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// A base abstract class for every special effect.
    /// It takes care of fields and parsing an effect from server message.
    /// </summary>
    /// <seealso cref="Client.ISpecialEffect" />
    public abstract class SpecialEffect : ISpecialEffect
    {
        /// <summary>
        /// Gets a value indicating whether this instance is dead.
        /// If it is dead, it is removed from the game later in the same
        /// game cycle.
        /// </summary>
        /// <value><c>true</c> if this instance is dead; otherwise, <c>false</c>.</value>
        public bool IsDead { get; protected set; }

        /// <summary>
        /// Renders the effect.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public abstract void Render(Graphics g);

        /// <summary>
        /// Plays one cycle of the effect.
        /// </summary>
        /// <param name="timeSpan">The time span from the last cycle.</param>
        public abstract void PlayCycle(int timeSpan);


        // PARSE SPELLS
        /// <summary>
        /// A static function that parses those effects that belong to a ability
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="data">The server message.</param>
        /// <returns>An instance of special effect.</returns>
        public static SpecialEffect ParseAbilityEffect(Game game, string data)
        {
            string[] parts = data.Split('&');
            int spellId = Convert.ToInt32(parts[1]);
            switch (spellId)
            {
                case SharedData.ABILITY_PRIEST_HEAL:
                    return parsePriestHeal(game, parts);

                default:
                    Console.WriteLine("Unimplemented ability: " + spellId + "!");
                    return null;
            }
        }

        private static SpecialEffect parsePriestHeal(Game game, string[] data)
        {
            int x = Convert.ToInt32(data[2]);
            int y = Convert.ToInt32(data[3]);
            return new AbilityPriestHeal(game, new Point(x, y));
        }
    }
}
