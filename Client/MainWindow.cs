using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class MainWindow : Form
    {
        private Application game;
        private Thread gameThread;

        private BufferedGraphicsContext context;
        private BufferedGraphics grafx;

        public MainWindow()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);


            this.Shown += (object sender, EventArgs e) => Start();
        }

        public void Start()
        {
            Graphics g = this.CreateGraphics();
            grafx = context.Allocate(g, new Rectangle(0, 0, this.Width, this.Height));

            try {
                game = new Application(this.ClientRectangle.Height, this.ClientRectangle.Width, grafx);
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Could not initilize game data, try reinstalling the game.");
                return;
            }

            gameThread = new Thread(game.Start);
            gameThread.Start();
        }

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            gameThread.Abort();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.OnKeyDown(e.KeyValue);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            game.OnKeyUp(e.KeyValue);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                game.OnMouseLeftDown(e.Location);
            else if (e.Button == MouseButtons.Right)
                game.OnMouseRightDown(e.Location);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                game.OnMouseLeftUp(e.Location);
            else if (e.Button == MouseButtons.Right)
                game.OnMouseRightUp(e.Location);
        }
    }
}
