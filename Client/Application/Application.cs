using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{ 
    public partial class Application
    {
        private BufferedGraphics graphics;
        private IWindow gameScreen;

        public Account Account { get; set; }
        public ServerConnection server { get; set; }

        public static int HEIGHT { get; private set; }
        public static int WIDTH { get; private set; }
        public static Point MIDDLE { get; private set; }

        public Application(int height, int width, BufferedGraphics graphics)
        {
            HEIGHT = height;
            WIDTH = width;
            MIDDLE = new Point(width / 2, height / 2);

            this.graphics = graphics;
            this.server = new ServerConnection(this);
        }

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
    }
}
