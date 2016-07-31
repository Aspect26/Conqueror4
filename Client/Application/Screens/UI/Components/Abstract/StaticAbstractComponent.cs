using System;
using System.Drawing;

namespace Client
{
    public abstract class StaticAbstractComponent : IComponent
    {
        public int WIDTH { get { return 1; } }
        public int HEIGHT { get { return 1; } }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        protected IComponent neighbour;

        public void ChangeHeight(int height)
        {
            return;
        }

        public void ChangeWidth(int width)
        {
            return;
        }

        public Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        public IComponent GetNeighbour()
        {
            return neighbour;
        }

        public bool IsAt(Point location)
        {
            return false;
        }

        public void MoveX(int X)
        {
            this.X += X;
        }

        public void MoveY(int Y)
        {
            this.Y += Y;
        }

        public void OnKeyDown(int key) { }

        public void OnKeyUp(int key) { }

        public void OnMouseLeftDown(Point position) { }

        public void OnMouseLeftUp(Point position) { }

        public void OnMouseRightDown(Point position) { }

        public void OnMouseRightUp(Point position) { }

        public abstract void Render(Graphics g);

        public void SetFocused(bool focused) { }

        public void SetNeighbour(IComponent neighbour)
        {
            this.neighbour = neighbour;
        }
    }
}
