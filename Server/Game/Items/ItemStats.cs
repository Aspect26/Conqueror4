namespace Server
{
    /// <summary>
    /// A simple structure containing all item bonuses.
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
