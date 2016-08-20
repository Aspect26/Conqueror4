namespace Server
{
    public class ItemEquipedDifference : GenericDifference
    {
        private IItem item;

        public ItemEquipedDifference(IUnit unit, IItem item)
            :base (unit.UniqueID)
        {
            this.item = item;
        }

        public override string GetString()
        {
            return "IE&" + item.GetCodedData();
        }
    }
}
