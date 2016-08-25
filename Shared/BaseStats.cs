namespace Shared
{
    /// <summary>
    /// This class represents a unit's base attributes (e.g.: hit points, damage, armor,
    /// ...). It is used to represent the unit's actual stats, as well as the unit's
    /// maximal stats.
    /// </summary>
    public class BaseStats
    {
        public int HitPoints { get; set; }
        public int ManaPoints { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int SpellBonus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStats"/> class.
        /// </summary>
        /// <param name="hitPoints">The hit points.</param>
        /// <param name="manaPoints">The mana points.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="armor">The armor.</param>
        /// <param name="spellBonus">The spell bonus.</param>
        public BaseStats(int hitPoints, int manaPoints, int damage, int armor, int spellBonus)
        {
            this.HitPoints = hitPoints;
            this.ManaPoints = manaPoints;
            this.Damage = damage;
            this.Armor = armor;
            this.SpellBonus = spellBonus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStats"/> class.
        /// </summary>
        /// <param name="hitPoints">The hit points.</param>
        /// <param name="manaPoints">The mana points.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="armor">The armor.</param>
        public BaseStats(int hitPoints, int manaPoints, int damage, int armor)
            : this(hitPoints, manaPoints, damage, armor, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStats"/> class.
        /// </summary>
        /// <param name="hitPoints">The hit points.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="armor">The armor.</param>
        public BaseStats(int hitPoints, int damage, int armor)
            :this(hitPoints, 0, damage, armor, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStats"/> class.
        /// </summary>
        public BaseStats()
            :this(0,0,0)
        {

        }

        /// <summary>
        /// Create a 'deep' copy of this instance.
        /// </summary>
        /// <returns>The deep copy.</returns>
        public BaseStats Copy()
        {
            return new BaseStats(HitPoints, ManaPoints, Damage, Armor, SpellBonus);
        }
    }
}
