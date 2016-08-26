namespace Server
{
    /// <summary>
    /// Represents a simple interface for wearable item.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Gets the item stats.
        /// </summary>
        /// <value>The stats.</value>
        ItemStats Stats { get; }

        /// <summary>
        /// Gets the slot the item belongs to.
        /// </summary>
        /// <value>The slot.</value>
        int Slot { get; }

        /// <summary>
        /// Gets the item's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }

        /// <summary>
        /// Gets the coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetCodedData();
    }
}
