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
            g.DrawImage(GameData.GetCharacterImage(character.Spec), x, y, size, size);

            // STATUS
            int w = width - size;
            int currentY = y;
            // HitPoints
            float ratio = (float)character.HitPoints / character.MaxHitPoints;
            g.FillRectangle(Brushes.DarkGreen, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Green, x + size, currentY, w * ratio, height / 3);
            g.DrawString(character.HitPoints + "/" + character.MaxHitPoints, GameData.GetFont(STATUS_FONT_SIZE),
                Brushes.White, x + size + 5, currentY + 2);

            // ManaPoints
            currentY += height / 3;
            ratio = (float)character.ManaPoints / character.MaxManaPoints;
            g.FillRectangle(Brushes.DarkBlue, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Blue, x + size, currentY, w * ratio, height / 3);
            g.DrawString(character.ManaPoints + "/" + character.MaxManaPoints, GameData.GetFont(STATUS_FONT_SIZE),
                Brushes.White, x + size + 5, currentY + 2);

            // Experience
            currentY += height / 3;
            ratio = (float)character.Experience / GameData.GetNextLevelXPRequired(character.Level);
            g.FillRectangle(Brushes.DarkViolet, x + size, currentY, w, height / 3);
            g.FillRectangle(Brushes.Violet, x + size, currentY, w * ratio, height / 3);
            g.DrawString(character.Experience + "/" + GameData.GetNextLevelXPRequired(character.Level), 
                GameData.GetFont(STATUS_FONT_SIZE), Brushes.White, x + size + 5, currentY + 2);
        }
    }
}
