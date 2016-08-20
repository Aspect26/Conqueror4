using System.Drawing;

namespace Client
{
    public class ItemDroppedSlotComponent : ItemSlotComponent
    {
        private Game game;

        public ItemDroppedSlotComponent(Point position, Game game)
            :base(null, -1, position)
        {
            this.game = game;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
        }

        protected override IItem getRenderingItem()
        {
            IItem item = game.GetDroppedItem();
            if (item != null)
            {
                this.itemImage = GameData.GetItemImage(item.Slot);
            }

            return game.GetDroppedItem();
        }
    }
}
