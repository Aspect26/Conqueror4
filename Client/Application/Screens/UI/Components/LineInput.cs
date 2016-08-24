using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represent a line input component.
    /// </summary>
    /// <seealso cref="Client.BorderedRectangleComponent" />
    public class LineInput : BorderedRectangleComponent
    {
        protected string textInput = "";
        protected const int HEIGHT = 35;  // 10 + 10 borders, 20 text, 10 margin
        protected const int MARGIN = 5;
        protected const int FONTSIZE = 15;
        protected static Font font = new Font(FontFamily.GenericSerif, FONTSIZE);
        protected Brush brush;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineInput"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="neighbour">The neighbour.</param>
        public LineInput(Point parentPosition, Point position, int width, Color textColor, IComponent neighbour = null) 
            : base(parentPosition, new Rectangle(position, new Size(width, HEIGHT)) , GameData.GetLineInputBackgground(), 
                  UserInterface.DEFAULT_BORDER_HEIGHT, neighbour)
        {
            brush = new SolidBrush(textColor);
        }

        /// <summary>
        /// Gets the test value in the component.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string getValue()
        {
            return textInput;
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            RenderText(g);
            if(focused)
                RenderCursor(g);
        }

        protected virtual void RenderText(Graphics g)
        {
            g.DrawString(visibleText(g, textInput), font, brush, position.X + borderSize + MARGIN, position.Y + borderSize);
        }

        protected virtual void RenderCursor(Graphics g)
        {
            int size = 3;
            int textWidth = (int)g.MeasureString(visibleText(g, textInput), font).Width;
            g.FillRectangle(brush, position.X + borderSize * 2 + textWidth, 
                position.Y + borderSize + MARGIN + FONTSIZE - size, FONTSIZE, size);
        }

        protected string visibleText(Graphics g, string original)
        {
            string trimmedText = original;
            while ((int)g.MeasureString(trimmedText + "_", font).Width > position.Width - borderSize * 2 - MARGIN * 2)
                trimmedText = trimmedText.Remove(0, 1);

            return trimmedText;
        }


        /// <summary>
        /// Called when [key down].
        /// If it is a alphanumeric character it is concatenated to the current value.
        /// </summary>
        /// <param name="key">The pressed key.</param>
        public override void OnKeyDown(int key)
        {
            char k = (char)key;

            // text
            if( (k >= 'a' && k <='z') || (k >= 'A' && k <= 'Z') )
                textInput += (char)key;

            // numbers
            if (k >= '0' && k <= '9')
                textInput += k.ToString();

            // backspace
            if (k == 8 && textInput.Length != 0)
            {
                if (textInput.Length != 0)
                    textInput = textInput.Remove(textInput.Length - 1);
                else
                    textInput = "";
            }
        }
    }
}
