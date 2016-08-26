namespace Server
{
    /// <summary>
    /// When a unit is killed it has a chance that it drops a item. The item however
    /// is not bound to be equiped or taken by somehow so it needs to be removed from
    /// the game after some time. The clients later need to be informed about this 
    /// removal.
    /// </summary>
    /// <seealso cref="Server.ITimedAction" />
    public class RemoveItemAction : ITimedAction
    {
        private int itemUID;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveItemAction"/> class.
        /// </summary>
        /// <param name="itemUID">The item uid.</param>
        public RemoveItemAction(int itemUID)
        {
            this.itemUID = itemUID;
        }

        /// <summary>
        /// Removed the item from the map instnce.
        /// </summary>
        /// <param name="map">The map instance it belongs to.</param>
        public void Process(MapInstance map)
        {
            map.GetDroppedItem(itemUID);
        }
    }
}
