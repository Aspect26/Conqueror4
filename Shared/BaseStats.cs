namespace Shared
{
    public class BaseStats
    {
        public int HitPoints { get; set; }
        public int ManaPoints { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int SpellBonus { get; set; }

        public BaseStats(int hitPoints, int manaPoints, int damage, int armor, int spellBonus)
        {
            this.HitPoints = hitPoints;
            this.ManaPoints = manaPoints;
            this.Damage = damage;
            this.Armor = armor;
            this.SpellBonus = spellBonus;
        }

        public BaseStats(int hitPoints, int manaPoints, int damage, int armor)
            : this(hitPoints, manaPoints, damage, armor, 0)
        {

        }

        public BaseStats(int hitPoints, int damage, int armor)
            :this(hitPoints, 0, damage, armor, 0)
        {

        }

        public BaseStats()
            :this(0,0,0)
        {

        }

        public BaseStats Copy()
        {
            return new BaseStats(HitPoints, ManaPoints, Damage, Armor, SpellBonus);
        }
    }
}
