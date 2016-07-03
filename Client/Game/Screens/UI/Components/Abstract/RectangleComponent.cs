using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class RectangleComponent : IComponent
    {
        protected Rectangle position;
        protected Color backgroundColor;
        protected Image backgroundImage;
        protected Brush backgroundBrush;
        protected IComponent neighbour;

        public int WIDTH { get; }
        public int HEIGHT { get; }
        public int X { get; }
        public int Y { get; }

        protected bool focused = false;

        public RectangleComponent(Point parentPosition, Rectangle position, Color background, IComponent neighbour = null)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;

            this.backgroundColor = background;
            this.neighbour = neighbour;
            this.backgroundBrush = new SolidBrush(background);

            this.WIDTH = position.Width;
            this.HEIGHT = position.Height;
            this.X = position.X;
            this.Y = position.Y;
        }

        public RectangleComponent(Point parentPosition, Rectangle position, Image background, IComponent neighbour = null)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;

            this.backgroundImage = background;
            this.neighbour = neighbour;

            this.WIDTH = position.Width;
            this.HEIGHT = position.Height;
            this.X = position.X;
            this.Y = position.Y;
        }

        public virtual void Render(Graphics g)
        {
            if (backgroundImage == null)
                g.FillRectangle(backgroundBrush, position);
            else
                g.DrawImage(backgroundImage, position);

            g.DrawRectangle(Pens.Red, position);
        }

        public virtual void SetFocused(bool focused)
        {
            this.focused = focused;
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
            return position.Contains(location);
        }

        public virtual void OnKeyDown(int key) { }
        public virtual void OnKeyUp(int key) { }
        public virtual void OnLeftMouseDown(Point position) { }
        public virtual void OnLeftMouseUp(Point position) { }
        public virtual void OnRightMouseDown(Point position) { }
        public virtual void OnRightMouseUp(Point position) { }
    }
}
