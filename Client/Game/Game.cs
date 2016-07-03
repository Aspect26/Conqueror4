using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{ 
    public partial class Game
    {
        private BufferedGraphics graphics;
        private IWindow gameScreen;

        public Player Player { get; set; }
        public Map Map { get; set; }
        public ServerConnection server { get; set; }

        public static int HEIGHT;
        public static int WIDTH;

        public Game(int height, int width, BufferedGraphics graphics)
        {
            HEIGHT = height;
            WIDTH = width;

            this.graphics = graphics;
            this.server = new ServerConnection(this);
        }

        public void Start()
        {
            gameScreen = new LoginScreen(/*this, server*/);
            while (true)
            {
                render();
            }
        }

        public void ChangeWindow(IWindow screen)
        {
            graphics.Graphics.Clear(Color.Black);
            this.gameScreen = screen;
        }

        private void render()
        {
            graphics.Render();
            gameScreen.Render(graphics.Graphics);
        }

        public void CreateMap()
        {
            Map = new Map();
            Map.Create(getMapFilePath(Player.CurrentLocation));
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
