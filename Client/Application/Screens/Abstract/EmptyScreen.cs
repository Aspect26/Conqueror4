using System.Drawing;

namespace Client
{
    public abstract class EmptyScreen : IWindow
    {
        protected Application application;
    
        protected UI userInterface = new UI();
        protected Image background;
        protected ServerConnection server;

        private Rectangle screenRect = new Rectangle(0, 0, Application.WIDTH, Application.HEIGHT);

        public EmptyScreen(Application application, ServerConnection server, Image background = null)
        {
            this.application = application;
            this.background = background;
            this.server = server;
        }

        public virtual void Render(Graphics g)
        {
            if (background != null)
                g.DrawImage(background, screenRect);
            else
                g.Clear(Color.Black);

            userInterface.Render(g);
        }

        public virtual void OnKeyDown(int key)
        {
            userInterface.OnKeyDown(key);
        }

        public virtual void OnKeyUp(int key)
        {
            userInterface.OnKeyUp(key);
        }

        public virtual void OnMouseLeftDown(Point location)
        {
            userInterface.OnMouseLeftDown(location);
        }

        public virtual void OnMouseLeftUp(Point location)
        {
            userInterface.OnMouseLeftUp(location);
        }

        public virtual void OnMouseRightDown(Point location)
        {
            userInterface.OnMouseRightDown(location);
        }

        public virtual void OnMouseRightUp(Point location)
        {
            userInterface.OnMouseRightUp(location);
        }
    }
}
