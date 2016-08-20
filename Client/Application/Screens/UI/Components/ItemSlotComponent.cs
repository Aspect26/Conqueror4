using System;
using System.Drawing;
using System.Text;

namespace Client
{
    public class ItemSlotComponent : RectangleComponent
    {
        public const int EQUIP_SLOT_SIZE = 50;

        private PlayedCharacter character;
        private int itemSlot;
        protected Image itemImage;

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
                str.Append(Environment.NewLine).Append("+ ").Append(item.Stats.HitPoints).Append(" MP");

            if (item.Stats.SpellBonus != 0)
                str.Append(Environment.NewLine).Append("+ ").Append(item.Stats.HitPoints).Append(" SP");

            return str.ToString();
        }

        public override bool HasTooltip()
        {
            return true;
        }
    }
}
