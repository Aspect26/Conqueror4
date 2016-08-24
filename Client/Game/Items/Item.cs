namespace Client
{
    /// <summary>
    /// An implementation of IItem interface.
    /// </summary>
    /// <seealso cref="Client.IItem" />
    public class Item : IItem
    {
        /// <summary>
        /// Gets the item bonuses
        /// </summary>
        /// <value>The item bonuses.</value>
        /// <seealso cref="Client.ItemStats" />
        public ItemStats Stats { get; protected set; }

        /// <summary>
        /// Gets the item slot it is associated with.
        /// </summary>
        /// <value>The slot.</value>
        public int Slot { get; protected set; }

        /// <summary>
        /// Gets the unique identifier of the item.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="stats">The item bonuses.</param>
        /// <param name="slot">The item slot.</param>
        /// <param name="uid">The item's unique identifier.</param>
        public Item(ItemStats stats, int slot, int uid)
        {
            this.Stats = stats;
            this.Slot = slot;
            this.UniqueID = uid;
        }
    }
}
