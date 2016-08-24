using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a component shown during the game in top left corner.
    /// It contains visual information about player's level, hitpoints, manapoints. experience and image.
    /// </summary>
    /// <seealso cref="Client.StaticAbstractComponent" />
    public class CharacterStatus : StaticAbstractComponent
    {
        private PlayedCharacter character;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatus"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
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

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            // TODO: separate this function into multiple smaller ones
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
