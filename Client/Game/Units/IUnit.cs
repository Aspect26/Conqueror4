using Shared;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Interface for every unit in the game.
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// Gets the unit identifier.
        /// This identifier identifies what type of a unit it is (e.g.: 3 is priest,
        /// 7 is  Wolf...)
        /// </summary>
        /// <value>The unit identifier.</value>
        int UnitID { get; }

        /// <summary>
        /// Gets the unique identifier.
        /// This identifier is unique for every unit in the game.
        /// Actually it is unique for every unit in a MapInstance (see server side).
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }

        /// <summary>
        /// Gets the maximum possible stats of this unit.
        /// </summary>
        /// <value>The maximum stats.</value>
        BaseStats MaxStats { get; }

        /// <summary>
        /// Gets the actual stats of the unit.
        /// </summary>
        /// <value>The actual stats.</value>
        BaseStats ActualStats { get; }

        /// <summary>
        /// Gets the actual hit points of the unit.
        /// </summary>
        /// <returns>The actual hit points.</returns>
        int GetActualHitPoints();

        /// <summary>
        /// Gets the actual mana points of the unit.
        /// </summary>
        /// <returns>The actual mana points.</returns>
        int GetActualManaPoints();

        /// <summary>
        /// Gets the maximum possible hit points.
        /// It shall be sum of a hit point from MaxStats and from item bonuses.
        /// </summary>
        /// <returns>The maximal possible hit points with regards to unit's level and equip.</returns>
        int GetMaxHitPoints();

        /// <summary>
        /// Gets the maximum possible mana points.
        /// It shall be sum of a mana point from MaxStats and from item bonuses.
        /// </summary>
        /// <returns>The maximal possible mana points with regards to unit's level and equip.</returns>
        int GetMaxManaPoints();

        /// <summary>
        /// Gets a value indicating whether this unit is dead.
        /// Units marked as dead are remove latter in the game cycle.
        /// </summary>
        /// <value><c>true</c> if this unit is dead; otherwise, <c>false</c>.</value>
        bool IsDead { get; }

        /// <summary>
        /// Determines whether this unit is a player.
        /// </summary>
        /// <returns><c>true</c> if this instance is a player; otherwise, <c>false</c>.</returns>
        bool IsPlayer();

        /// <summary>
        /// Updates the actual stats.
        /// </summary>
        /// <param name="stats">The actual stats.</param>
        void UpdateActualStats(BaseStats stats);

        /// <summary>
        /// Updates the maximum stats.
        /// </summary>
        /// <param name="stats">The maximum stats.</param>
        void UpdateMaxStats(BaseStats stats);

        /// <summary>
        /// Gets the unit's fraction.
        /// </summary>
        /// <value>The fraction.</value>
        int Fraction { get; }

        /// <summary>
        /// Plays one game cycle.
        /// </summary>
        /// <param name="timeSpan">The time span between last played cycle and now.</param>
        void PlayCycle(int timeSpan);

        /// <summary>
        /// Draws the unit.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        void DrawUnit(Graphics g);

        /// <summary>
        /// Gets the current image of the unit.
        /// </summary>
        /// <returns>The image.</returns>
        Image GetCurrentImage();

        /// <summary>
        /// Sets the unit's location.
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        void SetLocation(int x, int y);

        /// <summary>
        /// Tries to hit the unit with a missile.
        /// </summary>
        /// <param name="missile">The missile.</param>
        void TryHitByMissile(Missile missile);

        /// <summary>
        /// Kills this unit.
        /// </summary>
        void Kill();

        /// <summary>
        /// Gets or sets the unit's location.
        /// </summary>
        /// <value>The unit's location.</value>
        Location Location { get; set; }
    }
}
