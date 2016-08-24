using System;
using System.Drawing;
using System.Text;

namespace Client
{
    /// <summary>
    /// Represents a item slot component. 
    /// This components holds an information about one item and if it is hovered over it shows a tooltip with 
    /// the description of the item. 
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
    public class ItemSlotComponent : RectangleComponent
    {
        /// <summary>
        /// The size of this component (it is a square).
        /// </summary>
        public const int EQUIP_SLOT_SIZE = 50;

        private PlayedCharacter character;
        private int itemSlot;
        protected Image itemImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSlotComponent"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="itemSlot">The item slot.</param>
        /// <param name="position">The position.</param>
        public ItemSlotComponent(PlayedCharacter character, int itemSlot, Point position)
            :base(new Point(0,0), new Rectangle(position.X, position.Y, EQUIP_SLOT_SIZE, EQUIP_SLOT_SIZE),
                 GameData.GetEquipSlotImage())
        {
            this.character = character;
            this.itemSlot = itemSlot;
            if (itemSlot != -1)
            {
                this.itemImage = GameData.GetItemImage(itemSlot);
            }
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            if (getRenderingItem() != null)
            {
                g.DrawImage(itemImage, position.X, position.Y, position.Width, position.Height);
            }
        }

        protected virtual IItem getRenderingItem()
        {
            return character.Equip.Items[itemSlot];
        }

        /// <summary>
        /// Renders the tooltip.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="position">The tooltip position.</param>
        public override void RenderTooltip(Graphics g, Point position)
        {
            tooltipText = createTooltipText(getRenderingItem());
            base.RenderTooltip(g, position);
        }

        private string createTooltipText(IItem item)
        {
            if (item == null)
                return "No item";

            StringBuilder str = new StringBuilder(GameData.GetItemName(item.Slot));

            if (item.Stats.Armor != 0)
                str.Append(Environment.NewLine).Append("Armor ").Append(item.Stats.Armor);

            if (item.Stats.Damage != 0)
                str.Append(Environment.NewLine).Append("Damage ").Append(item.Stats.Damage);

            if (item.Stats.HitPoints != 0)
                str.Append(Environment.NewLine).Append("+ ").Append(item.Stats.HitPoints).Append(" HP");

            if (item.Stats.ManaPoints != 0)
                str.Append(Environment.NewLine).Append("+ ").Append(item.Stats.ManaPoints).Append(" MP");

            if (item.Stats.SpellBonus != 0)
                str.Append(Environment.NewLine).Append("+ ").Append(item.Stats.SpellBonus).Append(" SP");

            return str.ToString();
        }

        /// <summary>
        /// Determines whether this instance has tooltip.
        /// </summary>
        /// <returns><c>true</c> if this instance has tooltip; otherwise, <c>false</c>.</returns>
        public override bool HasTooltip()
        {
            return true;
        }
    }
}
