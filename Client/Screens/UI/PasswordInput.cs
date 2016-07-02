using System.Drawing;

namespace Client
{
    public sealed class PasswordInput : LineInput
    {
        public PasswordInput(Point position, int width, Color textColor, Color background) 
            : base(position, width, textColor, background)
        {
        }

        protected override void RenderText(Graphics g)
        {
            string hiddenString = new string('*', textInput.Length);
            g.DrawString(hiddenString, font, brush, position.X + borderSize * 2, position.Y + borderSize);
        }
    }
}
