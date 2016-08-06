using System.Drawing;

namespace Client
{
    public class QuestLog : Panel
    {
        private PlayedCharacter character;
        private Font titleFont;
        private Font descriptionFont;

        private const int PADDING = 7;

        public QuestLog(Rectangle position, PlayedCharacter character) 
            :base(new Point(0,0), position)
        {
            this.Shown = false;
            this.character = character;
            this.titleFont = GameData.GetFont(12);
            this.descriptionFont = GameData.GetFont(8);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            IQuest quest = character.Quest;
            int width = WIDTH - 2 * PADDING;
            int x = X + PADDING;
            int y = Y + PADDING;

            // quest name
            g.DrawString(quest.Title, titleFont, Brushes.Black, new RectangleF(x, y, width, 20));
            y += 20 + PADDING;

            // quest requirements
            foreach(QuestObjective objective in quest.Objectives)
            {
                Brush brush = objective.Completed ? Brushes.LightGreen : Brushes.Red;
                g.DrawString(objective.Text, descriptionFont, brush, new RectangleF(x+10, y, width, 12));
                y += 12;
            }
            y += 2*PADDING;

            // quest description
            g.DrawString(quest.Description, descriptionFont, Brushes.Black,
                new RectangleF(x, y, width, HEIGHT - (y - Y)));
        }
    }
}
