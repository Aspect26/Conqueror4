using System;

namespace Server
{
    public class RemoveItemAction : ITimedAction
    {
        private int itemUID;

        public RemoveItemAction(int itemUID)
        {
            this.itemUID = itemUID;
        }

        public void Process(MapInstance map)
        {
            map.GetDroppedItem(itemUID);
        }
    }
}
