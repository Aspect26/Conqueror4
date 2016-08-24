using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a password input type. It differs from the LineInput in the way that instead of text it 
    /// draws a sequence of '*' characters.
    /// </summary>
    /// <seealso cref="Client.LineInput" />
    public sealed class PasswordInput : LineInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordInput"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="neighbour">The neighbour.</param>
        public PasswordInput(Point parentPosition, Point position, int width, Color textColor, IComponent neighbour = null) 
            : base(parentPosition, position, width, textColor, neighbour)
        {
        }

        protected override void RenderText(Graphics g)
        {
            string hiddenString = new string('*', textInput.Length);
            g.DrawString(visibleText(g, hiddenString), font, brush, position.X + borderSize * 2, position.Y + borderSize);
        }

        protected override void RenderCursor(Graphics g)
        {
            int size = 3;
            string hiddenString = new string('*', textInput.Length);
            int textWidth = (int)g.MeasureString(visibleText(g, hiddenString), font).Width;
            g.FillRectangle(brush, position.X + borderSize * 2 + textWidth,
                position.Y + borderSize + MARGIN + FONTSIZE - size, FONTSIZE, size);
        }
    }
}
