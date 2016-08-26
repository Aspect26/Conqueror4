using System;
using System.Text;

namespace Server
{
    /// <summary>
    /// The only implementation of the IItem iterface.
    /// </summary>
    /// <seealso cref="Server.IItem" />
    public class Item : IItem
    {
        /// <summary>
        /// Gets the item's stats.
        /// </summary>
        /// <value>The stats.</value>
        public ItemStats Stats { get; protected set; }

        /// <summary>
        /// Gets the slot the item belongs to.
        /// </summary>
        /// <value>The slot.</value>
        public int Slot { get; protected set; }

        /// <summary>
        /// Gets the item's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <param name="slot">The slot.</param>
        /// <param name="uid">The uid.</param>
        public Item(ItemStats stats, int slot, int uid)
        {
            this.Stats = stats;
            this.Slot = slot;
            this.UniqueID = uid;
        }

        /// <summary>
        /// Gets the coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
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
