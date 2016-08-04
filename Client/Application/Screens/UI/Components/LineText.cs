using System.Drawing;

namespace Client
{
    public class LineText : IComponent
    {
        protected readonly string text = "";
        protected Point position;
        protected readonly Brush brush;
        protected readonly int size;
        protected IComponent neighbour;

        private bool set = false;
        private Font font;

        public int HEIGHT { get; set; }
        public int WIDTH { get; set; }
        public int X { get { return position.X; } }
        public int Y { get { return position.Y; } }

        public bool Shown { get; protected set; }

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

        public void Render(Graphics g)
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

        public void SetShown(bool shown)
        {
            this.Shown = shown;
        }

        public IComponent GetNeighbour()
        {
            return neighbour;
        }

        public void SetNeighbour(IComponent neighbour)
        {
            this.neighbour = neighbour;
        }

        public void MoveX(int X)
        {
            position.X += X;
        }

        public void MoveY(int Y)
        {
            position.Y += Y;
        }

        public void ChangeWidth(int width) { }

        public void ChangeHeight(int height) { }

        public Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        public bool IsAt(Point location)
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT).Contains(location);
        }

        public void OnKeyDown(int key) { }
        public void OnKeyUp(int key) { }
        public void OnMouseLeftDown(Point position) { }
        public void OnMouseLeftUp(Point position) { }
        public void OnMouseRightDown(Point position) { }
        public void OnMouseRightUp(Point position) { }
        public void SetFocused(bool focused) { }
    }
}
