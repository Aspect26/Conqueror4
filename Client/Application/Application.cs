using System.Drawing;

namespace Client
{ 
    /// <summary>
    /// Represents the whole application. It holds the information about current IWindow
    /// and renders it in the main loop. It also passes the user input actions  (e.g.:
    /// mouse click or key pressed) to that window.
    /// </summary>
    public class Application
    {
        private BufferedGraphics graphics;
        private IWindow gameScreen;

        public Account Account { get; set; }
        public ServerConnection server { get; set; }

        public static int HEIGHT { get; private set; }
        public static int WIDTH { get; private set; }
        public static Point MIDDLE { get; private set; }

        /// <summary>
        /// Initializes the application and server conection. 
        /// </summary>
        /// <param name="height">application client size height</param>
        /// <param name="width">application client size width</param>
        /// <param name="graphics">form's graphics</param>
        public Application(int height, int width, BufferedGraphics graphics)
        {
            HEIGHT = height;
            WIDTH = width;
            MIDDLE = new Point(width / 2, height / 2);

            this.graphics = graphics;
            this.server = new ServerConnection(this);
        }

        /// <summary>
        /// Sets the current screen to login screen and starts the infinite game cycle.
        /// </summary>
        public void Start()
        {
            gameScreen = new LoginScreen(this, server);
            while (true)
            {
                render();
            }
        }

        public void ChangeWindow(IWindow screen)
        {
            this.gameScreen = screen;
        }

        private void render()
        {
            graphics.Render();
            gameScreen.Render(graphics.Graphics);
        }

        // Events are passeed here from the form and propagated to the current game screen (this is done in the UI thread).
        public void OnKeyDown(int key)
        {
            gameScreen.OnKeyDown(key);
        }

        public void OnKeyUp(int key)
        {
            gameScreen.OnKeyUp(key);
        }

        public void OnMouseLeftDown(Point location)
        {
            gameScreen.OnMouseLeftDown(location);
        }

        public void OnMouseRightDown(Point location)
        {
            gameScreen.OnMouseRightDown(location);
        }

        public void OnMouseLeftUp(Point location)
        {
            gameScreen.OnMouseLeftUp(location);
        }

        public void OnMouseRightUp(Point location)
        {
            gameScreen.OnMouseRightUp(location);
        }

        public void OnMouseMove(int x, int y)
        {
            if (gameScreen != null)
            {
                gameScreen.OnMouseMove(x, y);
            }
        }
    }
}
