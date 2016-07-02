using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class EmptyScreen : IWindow
    {
        protected UI userInterface = new UI();

        public virtual void Render(Graphics g)
        {
            userInterface.Render(g);
        }

        public void OnKeyDown(int key)
        {
            userInterface.OnKeyDown(key);
        }

        public void OnKeyUp(int key)
        {
            userInterface.OnKeyUp(key);
        }

        public void OnMouseLeftDown(Point location)
        {
            userInterface.OnMouseLeftDown(location);
        }

        public void OnMouseLeftUp(Point location)
        {
            userInterface.OnMouseLeftUp(location);
        }

        public void OnMouseRightDown(Point location)
        {
            userInterface.OnMouseRightDown(location);
        }

        public void OnMouseRightUp(Point location)
        {
            userInterface.OnMouseRightUp(location);
        }
    }
}
