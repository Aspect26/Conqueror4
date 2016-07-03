using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class EmptyScreen : IWindow
    {
        protected Game game;
    
        protected UI userInterface = new UI();
        protected Image background;

        private Rectangle screenRect = new Rectangle(0, 0, Game.WIDTH, Game.HEIGHT);

        public EmptyScreen(Game game, Image background = null)
        {
            this.game = game;
            this.background = background;
        }

        public virtual void Render(Graphics g)
        {
            if (background != null)
                g.DrawImage(background, screenRect);
            else
                g.Clear(Color.Black);

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
