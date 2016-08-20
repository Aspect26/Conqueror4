namespace Server
{
    public interface IItem
    {
        ItemStats Stats { get; }
        ItemType Type { get; }

        string GetCodedData();
    }

    public enum ItemType
    {
        WEAPON,
        CHEST,
        HEAD,
        PANTS
    }

    public struct ItemStats
    {
        public int HitPoints;
        public int ManaPoints;

        public int Armor;
        public int Damage;

        public int SpellBonus;

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
