using System.Drawing;

namespace Client
{
    public class RightPlayOverlay : RectangleComponent
    {
        private const int OVERLAY_WIDTH = 150;
        private const int PADDING = 10;
        private const int FONT_SIZE = 13;
        private const int FONT_DETAIL_SIZE = 7;
        private const int MAP_SIZE = OVERLAY_WIDTH - 2 * PADDING;
        private const int EQUIP_SLOT_SIZE = 50;
        private const int BLOCK_SPACE = 20;

        private const int EQUIP_SLOTS = 4;
        private const int EQUIP_SLOT_WEAPON = 0;
        private const int EQUIP_SLOT_CHEST = 1;
        private const int EQUIP_SLOT_HEAD = 2;
        private const int EQUIP_SLOT_PANTS = 3;

        private PlayedCharacter playedCharacter;
        private Font font;
        private Font detailFont;
        private Brush fontBrush;
        private Image mapImage;
        private Image equipSlotImage;

        public RightPlayOverlay(PlayedCharacter character)
            :base(new Point(0,0), new Rectangle(Application.WIDTH - OVERLAY_WIDTH, 0,
                OVERLAY_WIDTH, Application.HEIGHT), Color.Black)
        {
            this.playedCharacter = character;

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
        private Point[] equipSlotPosition;
        private Point droppedItemTextPosition;
        private Point droppedItemSlotPosition;
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
            equipSlotPosition = new Point[EQUIP_SLOTS];
            equipSlotPosition[EQUIP_SLOT_WEAPON] = new Point(positionX, currentY);
            equipSlotPosition[EQUIP_SLOT_CHEST] = new Point(positionX + PADDING + EQUIP_SLOT_SIZE, currentY);
            currentY += 5 + EQUIP_SLOT_SIZE;
            equipSlotPosition[EQUIP_SLOT_HEAD] = new Point(positionX, currentY);
            equipSlotPosition[EQUIP_SLOT_PANTS] = new Point(positionX + PADDING + EQUIP_SLOT_SIZE, currentY);
            currentY += PADDING + BLOCK_SPACE + EQUIP_SLOT_SIZE;

            // dropped item
            droppedItemTextPosition = new Point(positionX, currentY);
            currentY += PADDING + FONT_SIZE;
            droppedItemSlotPosition = new Point(positionX, currentY);
            currentY += PADDING + EQUIP_SLOT_SIZE;
            droppedItemHelpPosition = new Point(positionX, currentY);
            currentY += PADDING + BLOCK_SPACE + FONT_DETAIL_SIZE;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            // map
            g.DrawString("Map:", font, fontBrush, mapTextPosition);
            g.DrawImage(mapImage, mapImagePosition.X, mapImagePosition.Y, MAP_SIZE, MAP_SIZE);

            // items
            g.DrawString("Equip:", font, fontBrush, equipTextPosition);
            for(int i = 0; i < EQUIP_SLOTS; i++)
            {
                g.DrawImage(equipSlotImage, equipSlotPosition[i].X, equipSlotPosition[i].Y,
                    EQUIP_SLOT_SIZE, EQUIP_SLOT_SIZE);
            }

            // dropped item
            g.DrawString("Dropped Item:", font, fontBrush, droppedItemTextPosition);
            g.DrawImage(equipSlotImage, droppedItemSlotPosition.X, droppedItemSlotPosition.Y,
                EQUIP_SLOT_SIZE, EQUIP_SLOT_SIZE);
            g.DrawString("Press 'T' to equip item.", detailFont, fontBrush, droppedItemHelpPosition);
        }
    }
}
