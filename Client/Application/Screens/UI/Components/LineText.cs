using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a component with one line of a text.
    /// </summary>
    /// <seealso cref="Client.BaseComponent" />
    public class LineText : BaseComponent
    {
        protected readonly string text = "";
        protected Point position;
        protected readonly Brush brush;
        protected readonly int size;

        private bool set = false;
        private Font font;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineText"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="neighbour">The neighbour.</param>
        /// <param name="shown">if set to <c>true</c> [shown].</param>
        public LineText(Point parentPosition, string text, Color color, Point position, int size, 
            IComponent neighbour = null, bool shown = true)
        {
            this.text = text;
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;
            this.size = size;
            this.neighbour = neighbour;
            this.Shown = shown;

            this.brush = new SolidBrush(color);
            this.font = new Font(FontFamily.GenericMonospace, size);
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            if (!set)
            {
                set = true;
                SizeF textSize = g.MeasureString(text, font);

                this.WIDTH = (int)textSize.Width;
                this.HEIGHT = (int)textSize.Height;
            }

            g.DrawString(text, font, brush, position);
        }

        /// <summary>
        /// You can't change a width of a text that easily!
        /// </summary>
        /// <param name="width">Irelevant.</param>
        public override void ChangeWidth(int width) { }

        /// <summary>
        /// You can't change a height of a text that easily!
        /// </summary>
        /// <param name="height">Irelevant.</param>
        public override void ChangeHeight(int height) { }

        /// <summary>
        /// Gets the client area.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        public override Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Determines whether the component is at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns><c>true</c> if the component is at the specified location; <c>false</c> otherwise.</returns>
        public override bool IsAt(Point location)
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT).Contains(location);
        }
    }
}
