using Shared;
using System.Text;

namespace Server
{
    public class ItemDroppedDifference : GenericDifference
    {
        private Location location;
        private IItem item;

        public ItemDroppedDifference(IItem item, IUnit unit) :
            base(unit.UniqueID)
        {
            this.location = unit.GetLocation();
            this.item = item;
        }

        public override string GetString()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("I");

            if (item.Stats.HitPoints != 0)
                msg.Append("&H^").Append(item.Stats.HitPoints);

            if (item.Stats.ManaPoints != 0)
                msg.Append("&M^").Append(item.Stats.ManaPoints);

            if (item.Stats.Damage != 0)
                msg.Append("&D^").Append(item.Stats.Damage);

            if (item.Stats.Armor != 0)
                msg.Append("&A^").Append(item.Stats.Armor);

            if (item.Stats.SpellBonus != 0)
                msg.Append("&S^").Append(item.Stats.SpellBonus);

            return msg.ToString();
        }
    }
}
