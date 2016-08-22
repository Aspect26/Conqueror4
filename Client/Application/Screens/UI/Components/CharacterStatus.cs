using Shared;
using System.Drawing;

namespace Client
{
    public class CharacterStatus : StaticAbstractComponent
    {
        private PlayedCharacter character;

        public CharacterStatus(PlayedCharacter character)
        {
            this.character = character;
        }

        private const int size = 51;
        private const int x = 10;
        private const int y = 10;
        private const int width = 200;
        private const int height = size;

        private const int STATUS_FONT_SIZE = 7;

        public override void Render(Graphics g)
        {
            // BORDER
            g.DrawRectangle(Pens.Black, x - 1, y - 1, width + 1, height + 1);

            // CHAR IMAGE
            g.FillRectangle(Brushes.Black, x, y, size, size);
            g.DrawImage(GameData.GetUnitImage(character.Spec), x, y, size, size);

            // LEVEL
            g.DrawString(character.Level.ToString(), GameData.GetFont(STATUS_FONT_SIZE), Brushes.Yellow, x + 2, y + 2);

            // STATUS
            int w = width - size;
            int currentY = y;
            
            // HitPoints
            float ratio = (float)character.GetActualHitPoints() / character.GetMaxHitPoints();
            g.FillRectangle(Brushes.DarkGreen, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Green, x + size, currentY, w * ratio, height / 3);
            g.DrawString("HP: " + character.GetActualHitPoints() + "/" + character.GetMaxHitPoints(), 
                GameData.GetFont(STATUS_FONT_SIZE), Brushes.White, x + size + 5, currentY + 2);

            // ManaPoints
            currentY += height / 3;
            ratio = (float)character.GetActualManaPoints() / character.GetMaxManaPoints();
            g.FillRectangle(Brushes.DarkBlue, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Blue, x + size, currentY, w * ratio, height / 3);
            g.DrawString("MP: " + character.GetActualManaPoints() + "/" + character.GetMaxManaPoints(), 
                GameData.GetFont(STATUS_FONT_SIZE), Brushes.White, x + size + 5, currentY + 2);

            // Experience
            currentY += height / 3;
            ratio = (float)character.Experience / character.ExperienceRequired;
            g.FillRectangle(Brushes.DarkViolet, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Violet, x + size, currentY, w * ratio, height / 3);
            g.DrawString("XP: " + character.Experience + "/" + character.ExperienceRequired, 
                GameData.GetFont(STATUS_FONT_SIZE), Brushes.White, x + size + 5, currentY + 2);
        }
    }
}
