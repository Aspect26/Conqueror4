using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a item slot in which a nearby dropped item is shown. This components is visible in the game in the panel 
    /// at the right side of the screen.
    /// </summary>
    /// <seealso cref="Client.ItemSlotComponent" />
    public class ItemDroppedSlotComponent : ItemSlotComponent
    {
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDroppedSlotComponent"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="game">The game.</param>
        public ItemDroppedSlotComponent(Point position, Game game)
            :base(null, -1, position)
        {
            this.game = game;
        }

        /// <summary>
        /// Renders the component.
        /// </summary>
        /// <param name="g">The g.</param>
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
