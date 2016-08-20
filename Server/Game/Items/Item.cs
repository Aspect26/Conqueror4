using System;
using System.Text;

namespace Server
{
    public class Item : IItem
    {
        public ItemStats Stats { get; protected set; }
        public int Slot { get; protected set; }
        public int UniqueID { get; protected set; }

        public Item(ItemStats stats, int slot, int uid)
        {
            this.Stats = stats;
            this.Slot = slot;
            this.UniqueID = uid;
        }

        public string GetCodedData()
        {
            StringBuilder str = new StringBuilder(Slot + "&" + UniqueID);

            if (this.Stats.HitPoints != 0)
                str.Append("&H^").Append(this.Stats.HitPoints);

            if (this.Stats.ManaPoints != 0)
                str.Append("&M^").Append(this.Stats.ManaPoints);

            if (this.Stats.Damage != 0)
                str.Append("&D^").Append(this.Stats.Damage);

            if (this.Stats.Armor != 0)
                str.Append("&A^").Append(this.Stats.Armor);

            if (this.Stats.SpellBonus != 0)
                str.Append("&S^").Append(this.Stats.SpellBonus);

            return str.ToString();
        }
    }
}
