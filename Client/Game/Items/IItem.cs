namespace Client
{
    /// <summary>
    /// A simple interface for item.
    /// There are currently 4 types of items: weapon, chest, head and pants.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Gets the item bonuses
        /// </summary>
        /// <value>The item bonuses.</value>
        /// <seealso cref="Client.ItemStats"/>
        ItemStats Stats { get; }

        /// <summary>
        /// Gets the item slot it is associated with.
        /// </summary>
        /// <value>The slot.</value>
        int Slot { get; }

        /// <summary>
        /// Gets the unique identifier of the item.
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }
    }

    /// <summary>
    /// A structure containing all the item bonuses.
    /// Currently the suported item bonuses are: hit points, mana points, armor, damage and spell bonus.
    /// </summary>
    public struct ItemStats
    {
        public int HitPoints;
        public int ManaPoints;

        public int Armor;
        public int Damage;

        public int SpellBonus;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStats"/> struct.
        /// </summary>
        /// <param name="hitPoints">The hit points.</param>
        /// <param name="manaPoints">The mana points.</param>
        /// <param name="armor">The armor.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="spellBonus">The spell bonus.</param>
        public ItemStats(int hitPoints, int manaPoints, int armor, int damage, int spellBonus)
        {
            this.HitPoints = hitPoints;
            this.ManaPoints = manaPoints;
            this.Armor = armor;
            this.Damage = damage;
            this.SpellBonus = spellBonus;
        }
    }
}
