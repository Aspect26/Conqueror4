﻿using Shared;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the overlay that is shown on the right side of the screen while ingame.
    /// It contains map of the current location, player's equip and a dropped item if any is nearby.
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
    public class RightPlayOverlay : RectangleComponent
    {
        private const int OVERLAY_WIDTH = 150;
        private const int PADDING = 10;
        private const int FONT_SIZE = 13;
        private const int FONT_DETAIL_SIZE = 7;
        private const int MAP_SIZE = OVERLAY_WIDTH - 2 * PADDING;
        private const int BLOCK_SPACE = 20;

        private PlayedCharacter playedCharacter;
        private Font font;
        private Font detailFont;
        private Brush fontBrush;
        private Image mapImage;
        private Image equipSlotImage;
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="RightPlayOverlay"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="character">The character.</param>
        public RightPlayOverlay(Game game, PlayedCharacter character)
            :base(new Point(0,0), new Rectangle(Application.WIDTH - OVERLAY_WIDTH, 0,
                OVERLAY_WIDTH, Application.HEIGHT), Color.Black)
        {
            this.playedCharacter = character;
            this.game = game;

            this.font = GameData.GetFont(FONT_SIZE);
            this.detailFont = GameData.GetFont(FONT_DETAIL_SIZE);
            this.fontBrush = Brushes.Aquamarine;
            this.mapImage = GameData.GetMapImage(playedCharacter.Location.MapID);
            this.equipSlotImage = GameData.GetEquipSlotImage();

            initialize();
        }

        private Point mapTextPosition;
        private Point mapImagePosition;
        private Point equipTextPosition;
        private ItemSlotComponent[] equipSlotComponent;
        private Point droppedItemTextPosition;
        private ItemDroppedSlotComponent droppedItemComponent;
        private Point droppedItemHelpPosition;

        private void initialize()
        {
            int currentY = position.Y + PADDING;
            int positionX = position.X + PADDING;

            // map
            mapTextPosition = new Point(positionX, currentY);
            currentY += PADDING + FONT_SIZE;
            mapImagePosition = new Point(positionX, currentY);
            currentY += MAP_SIZE + PADDING + BLOCK_SPACE;

            // inventory
            equipTextPosition = new Point(positionX, currentY);
            currentY += PADDING + FONT_SIZE;
            equipSlotComponent = new ItemSlotComponent[SharedData.ITEM_SLOTS];
            equipSlotComponent[SharedData.ITEM_SLOT_WEAPON] = new ItemSlotComponent(playedCharacter,
                SharedData.ITEM_SLOT_WEAPON, new Point(positionX, currentY));
            equipSlotComponent[SharedData.ITEM_SLOT_CHEST] = new ItemSlotComponent(playedCharacter,
                SharedData.ITEM_SLOT_CHEST, new Point(positionX + PADDING + ItemSlotComponent.EQUIP_SLOT_SIZE, currentY));
            currentY += 5 + ItemSlotComponent.EQUIP_SLOT_SIZE;
            equipSlotComponent[SharedData.ITEM_SLOT_HEAD] = new ItemSlotComponent(playedCharacter,
                SharedData.ITEM_SLOT_HEAD, new Point(positionX, currentY));
            equipSlotComponent[SharedData.ITEM_SLOT_PANTS] = new ItemSlotComponent(playedCharacter,
                SharedData.ITEM_SLOT_PANTS, new Point(positionX + PADDING + ItemSlotComponent.EQUIP_SLOT_SIZE, currentY));
            currentY += PADDING + BLOCK_SPACE + ItemSlotComponent.EQUIP_SLOT_SIZE;

            // dropped item
            droppedItemTextPosition = new Point(positionX, currentY);
            currentY += PADDING + FONT_SIZE;
            droppedItemComponent = new ItemDroppedSlotComponent(new Point(positionX, currentY), game);
            currentY += PADDING + ItemSlotComponent.EQUIP_SLOT_SIZE;
            droppedItemHelpPosition = new Point(positionX, currentY);
            currentY += PADDING + BLOCK_SPACE + FONT_DETAIL_SIZE;
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            // map
            g.DrawString("Map:", font, fontBrush, mapTextPosition);
            g.DrawImage(mapImage, mapImagePosition.X, mapImagePosition.Y, MAP_SIZE, MAP_SIZE);

            // items
            g.DrawString("Equip:", font, fontBrush, equipTextPosition);
            for(int i = 0; i < equipSlotComponent.Length; i++)
            {
                equipSlotComponent[i].Render(g);
            }

            // dropped item
            g.DrawString("Dropped Item:", font, fontBrush, droppedItemTextPosition);
            droppedItemComponent.Render(g);
            g.DrawString("Press 'T' to equip item.", detailFont, fontBrush, droppedItemHelpPosition);
        }

        /// <summary>
        /// It contains components that have tooltip (item slots) so it returns true so the ui redirects 
        /// tooltip call to this instance.
        /// </summary>
        /// <returns><c>true</c> if this instance has tooltip; otherwise, <c>false</c>.</returns>
        public override bool HasTooltip()
        {
            return true;
        }

        /// <summary>
        /// Renders the tooltip.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="position">The tooltip position.</param>
        public override void RenderTooltip(Graphics g, Point position)
        {
            foreach(ItemSlotComponent c in equipSlotComponent)
            {
                if (c.IsAt(position))
                {
                    c.RenderTooltip(g, position);
                }
            }

            if(droppedItemComponent.IsAt(position))
            {
                droppedItemComponent.RenderTooltip(g, position);
            }
        }
    }
}
