using System;
using Shared;

namespace Server
{
    public class ItemRemovedDifference : NonUnitDifference
    {
        private IItem item;

        public ItemRemovedDifference(IItem item)
            : base(SharedData.DIFFERENCE_TYPE_ITEM)
        {
            this.item = item;
        }

        public override string GetString()
        {
            return "R&" + item.UniqueID;
        }
    }
}
