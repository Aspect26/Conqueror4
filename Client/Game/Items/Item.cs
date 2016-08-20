using System;

namespace Client
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
    }
}
