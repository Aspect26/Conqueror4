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

        public int WIDTH { get { return position.Width; } }
        public int HEIGHT { get { return position.Height; } }
        public int X { get { return position.X; }  }
        public int Y { get { return position.Y; } }

        protected bool focused = false;

        public RectangleComponent(Point parentPosition, Rectangle position, Color background, IComponent neighbour = null)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;

            this.backgroundColor = background;
            this.neighbour = neighbour;
            this.backgroundBrush = new SolidBrush(background);
        }

        public RectangleComponent(Point parentPosition, Rectangle position, Image background, IComponent neighbour = null)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;

            this.backgroundImage = background;
            this.neighbour = neighbour;
        }

        public virtual void Render(Graphics g)
        {
            if (backgroundImage == null)
                g.FillRectangle(backgroundBrush, position);
            else
                g.DrawImage(backgroundImage, position);
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

        public void MoveX(int X)
        {
            position.X += X;
        }

        public void MoveY(int Y)
        {
            position.Y += Y;
        }

        public void ChangeWidth(int width)
        {
            position.Width = width;
        }

        public void ChangeHeight(int height)
        {
            position.Height = height;
        }

        public virtual Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        public bool IsAt(Point location)
        {
            return position.Contains(location);
        }

        public virtual void OnKeyDown(int key) { }
        public virtual void OnKeyUp(int key) { }
        public virtual void OnMouseLeftDown(Point position) { }
        public virtual void OnMouseLeftUp(Point position) { }
        public virtual void OnMouseRightDown(Point position) { }
        public virtual void OnMouseRightUp(Point position) { }
    }
}
