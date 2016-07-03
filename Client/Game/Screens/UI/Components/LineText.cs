using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class LineText : IComponent
    {
        protected readonly string text = "";
        protected readonly Point position;
        protected readonly Brush brush;
        protected readonly int size;
        protected IComponent neighbour;

        private bool set = false;
        private Font font;

        public int HEIGHT { get; set; }
        public int WIDTH { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public LineText(Point parentPosition, string text, Color color, Point position, int size, IComponent neighbour = null)
        {
            this.text = text;
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;
            this.size = size;
            this.neighbour = neighbour;

            this.brush = new SolidBrush(color);
            this.font = new Font(FontFamily.GenericMonospace, size);

            this.X = position.X;
            this.Y = position.Y;
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
            g.DrawRectangle(Pens.Red, new Rectangle(position, new Size(WIDTH, HEIGHT)));
        }

        public IComponent GetNeighbour()
        {
            return neighbour;
        }

        public void SetNeighbour(IComponent neighbour)
        {
            this.neighbour = neighbour;
        }

        public bool IsAt(Point location)
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT).Contains(location);
        }

        public void OnKeyDown(int key) { }
        public void OnKeyUp(int key) { }
        public void OnLeftMouseDown(Point position) { }
        public void OnLeftMouseUp(Point position) { }
        public void OnRightMouseDown(Point position) { }
        public void OnRightMouseUp(Point position) { }
        public void SetFocused(bool focused) { }
    }
}
