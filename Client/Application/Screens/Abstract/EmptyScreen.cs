using System.Drawing;

namespace Client
{
    /// <summary>
    /// Base abstract class for all windows. Handles all the input events passed by the main form -> passes them to 
    /// the interface and renders the interface and screen's background.
    /// </summary>
    /// <seealso cref="Client.IWindow" />
    public abstract class EmptyScreen : IWindow
    {
        protected Application application;
        protected UserInterface userInterface = new UserInterface();
        protected Image background;
        protected ServerConnection server;

        private Rectangle screenRect = new Rectangle(0, 0, Application.WIDTH, Application.HEIGHT);

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyScreen"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="server">The server connection.</param>
        /// <param name="background">The background image.</param>
        public EmptyScreen(Application application, ServerConnection server, Image background = null)
        {
            this.application = application;
            this.background = background;
            this.server = server;
        }

        /// <summary>
        /// Renders the user interface and background on the graphics object. 
        /// </summary>
        /// <param name="g">Graphics object.</param>
        public virtual void Render(Graphics g)
        {
            if (background != null)
                g.DrawImage(background, screenRect);
            else
                g.Clear(Color.Black);

            userInterface.Render(g);
        }

        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void OnKeyDown(int key)
        {
            userInterface.OnKeyDown(key);
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void OnKeyUp(int key)
        {
            userInterface.OnKeyUp(key);
        }

        /// <summary>
        /// Called when [left mouse button down].
        /// </summary>
        /// <param name="location">The location.</param>
        public virtual void OnMouseLeftDown(Point location)
        {
            userInterface.OnMouseLeftDown(location);
        }

        /// <summary>
        /// Called when [left mouse button up].
        /// </summary>
        /// <param name="location">The location.</param>
        public virtual void OnMouseLeftUp(Point location)
        {
            userInterface.OnMouseLeftUp(location);
        }

        /// <summary>
        /// Called when [right mouse button down].
        /// </summary>
        /// <param name="location">The location.</param>
        public virtual void OnMouseRightDown(Point location)
        {
            userInterface.OnMouseRightDown(location);
        }

        /// <summary>
        /// Called when [right mouse button up].
        /// </summary>
        /// <param name="location">The location.</param>
        public virtual void OnMouseRightUp(Point location)
        {
            userInterface.OnMouseRightUp(location);
        }

        /// <summary>
        /// Called when [mouse moves].
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        public virtual void OnMouseMove(int x, int y)
        {
            userInterface.OnMouseMove(x, y);
        }
    }
}
