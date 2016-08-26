using System.Collections.Generic;
using Shared;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// An interface for every unit in the game (player controlled or non player
    /// controlled).
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// Gets the unit identifier.
        /// This identifier identifies which type of a unit this is. For character
        /// players it determines its specialization.
        /// </summary>
        /// <value>The unit identifier.</value>
        int UnitID { get; }

        // TODO: BUG! change character's uid when it changes location! (e.g. after
        // telepor).
        /// <summary>
        /// Gets the unique identifier.
        /// This is a unique iddentifier for a unit. These identifiers are uniue in the 
        /// map instance scope.
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }

        /// <summary>
        /// Gets the map instance in which this unit is located.
        /// </summary>
        /// <value>The map instance.</value>
        MapInstance MapInstance { get; }

        /// <summary>
        /// Gets the hit range.
        /// </summary>
        /// <value>The hit range.</value>
        int HitRange { get; }

        /// <summary>
        /// Gets the maximum base stats stats. 
        /// </summary>
        /// <value>The maximum stats.</value>
        BaseStats MaxStats { get; }

        /// <summary>
        /// Gets the actual stats of a unit.
        /// </summary>
        /// <value>The actual stats.</value>
        BaseStats ActualStats { get; }

        /// <summary>
        /// Resets the actual stats so they have value of the maximal possible stats.
        /// </summary>
        void ResetStats();

        /// <summary>
        /// Gets the level of the unit.
        /// </summary>
        /// <value>The level.</value>
        int Level { get; }

        /// <summary>
        /// Gets the actual hit points.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetActualHitPoints();

        /// <summary>
        /// Gets the actual mana points.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetActualManaPoints();

        /// <summary>
        /// Gets the maximum hit points. These hit points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.HitPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxHitPoints();

        /// <summary>
        /// Gets the maximum mana points. These mana points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.ManaPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxManaPoints();

        /// <summary>
        /// Gets the damage. The return value is influenced bu character's equip. 
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDamage();

        /// <summary>
        /// Gets the armor. The return value is influenced bu character's equip. 
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetArmor();

        /// <summary>
        /// Gets the spell bonus. The return value is influenced bu character's equip. 
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetSpellBonus();


        /// <summary>
        /// Heals the unit by specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        void Heal(int amount);

        /// <summary>
        /// Decreases the actual mana points by specified value.
        /// </summary>
        /// <param name="amount">The amount.</param>
        void DecreaseActualManaPoints(int amount);

        /// <summary>
        /// Gets the fraction.
        /// </summary>
        /// <value>The fraction.</value>
        int Fraction { get; }

        /// <summary>
        /// Gets the respawn time.
        /// </summary>
        /// <value>The respawn time.</value>
        int RespawnTime { get; }

        /// <summary>
        /// Gets the unit's current location.
        /// </summary>
        /// <returns>Location.</returns>
        Location GetLocation();

        /// <summary>
        /// Gets the unit's respawn position. It is not used for player's characters.
        /// </summary>
        /// <value>The spawn position.</value>
        Point SpawnPosition { get; }

        /// <summary>
        /// Gets the unit's name.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetName();

        /// <summary>
        /// Determines whether this unit is a player.
        /// </summary>
        /// <returns><c>true</c> if this unit is aa player; otherwise, <c>false</c>.</returns>
        bool IsPlayer();

        /// <summary>
        /// Plays one game cycle for the unit.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        void PlayCycle(int timeSpan);

        /// <summary>
        /// Tries the hit the unit by missile. Checks whether this unit is close
        /// enough to the specified missile so it can be hit (it's not like the unit
        /// wants that but it must be done :) ).
        /// </summary>
        /// <param name="missile">The missile.</param>
        void TryHitByMissile(Missile missile);

        /// <summary>
        /// Hits the unit by a missile.
        /// </summary>
        /// <param name="missile">The missile.</param>
        void HitByMissile(Missile missile);

        /// <summary>
        /// Gets a value indicating whether this unit is dead.
        /// </summary>
        /// <value><c>true</c> if this unit is dead; otherwise, <c>false</c>.</value>
        bool IsDead { get; }

        /// <summary>
        /// Leaves the combat with specified unit. 
        /// </summary>
        /// <param name="unit">The unit to leave combat with.</param>
        void LeaveCombatWith(IUnit unit);

        /// <summary>
        /// Enters the combat with a spcified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        void EnterCombatWith(IUnit unit);

        /// <summary>
        /// Adds experience to the unit (only applied to player characters).
        /// </summary>
        /// <param name="xp">The xp.</param>
        void AddExperience(int xp);

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IUnit"/> moved 
        /// since the last send update.
        /// </summary>
        /// <value><c>true</c> if moved; otherwise, <c>false</c>.</value>
        bool Moved { get; set; }

        /// <summary>
        /// Gets the unit's differences between now and the last send update.
        /// </summary>
        /// <value>The differences.</value>
        List<IUnitDifference> Differences { get; }

        /// <summary>
        /// Gets the list of units that hitted this unit in the current combat.
        /// </summary>
        /// <value>The list of units.</value>
        List<IUnit> HittedBy { get; }

        /// <summary>
        /// Gets the list of units this unit is currently 'visiting' (are close enough).
        /// </summary>
        /// <value>The currently visited.</value>
        List<IUnit> CurrentlyVisited { get; }

        /// <summary>
        /// Gets the list of units this unit is in combat with.
        /// </summary>
        /// <value>The list of units.</value>
        List<IUnit> InCombatWith { get; }

        /// <summary>
        /// This function is called when this unit dies (and it must be a non player
        /// unit). It generates a item that this unit droppes after its death.
        /// </summary>
        /// <returns>The item.</returns>
        IItem GenerateDroppedItem();

        /// <summary>
        /// Adds a new difference between last send update and now.
        /// </summary>
        /// <param name="difference">The difference.</param>
        void AddDifference(IUnitDifference difference);

        /// <summary>
        /// Gets or sets a value indicating whether something about this unit changed
        /// between now and last save to SQL database.
        /// </summary>
        /// <value><c>true</c> if [SQL difference]; otherwise, <c>false</c>.</value>
        bool SQLDifference { get; set; }
    }
}
