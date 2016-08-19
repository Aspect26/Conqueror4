using System;

namespace Server
{
    public class Item : IItem
    {
        public ItemStats Stats { get; protected set; }
        public ItemType Type { get; protected set; }

        public Item(ItemStats stats, ItemType type)
        {
            this.Stats = stats;
            this.Type = type;
        }
    }
}
