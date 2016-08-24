using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// The one and only form of the application. It contains no components and the
    /// game is rendered directly on this form.
    /// </summary>
    public partial class MainWindow : Form
    {
        private Application application;
        private Thread applicationThread;

        private BufferedGraphicsContext context;
        private BufferedGraphics grafx;

        public MainWindow()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);


            this.Shown += (object sender, EventArgs e) => Start();
        }

        /// <summary>
        /// Starts the application. Initializes the <i>double buffered</i> graphics objects and
        /// the application. If it initializes successfully, then the main (rendering) loop
        /// is started in a different thread. It also passes user events (e.g.: mouse click, 
        /// key pressed etc.) to the application object so these events are handled 
        /// in the winforms UI thread.
        /// </summary>
        public void Start()
        {
            Graphics g = this.CreateGraphics();
            grafx = context.Allocate(g, new Rectangle(0, 0, this.Width, this.Height));

            try {
                application = new Application(this.ClientRectangle.Height, this.ClientRectangle.Width, grafx);
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Could not initilize game data, try reinstalling the game.");
                return;
            }

            applicationThread = new Thread(application.Start);
            applicationThread.Start();
        }

        private void OnClose(object sender, FormClosedEventArgs e)
        {
            applicationThread.Abort();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            application.OnKeyDown(e.KeyValue);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            application.OnKeyUp(e.KeyValue);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                application.OnMouseLeftDown(e.Location);
            else if (e.Button == MouseButtons.Right)
                application.OnMouseRightDown(e.Location);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                application.OnMouseLeftUp(e.Location);
            else if (e.Button == MouseButtons.Right)
                application.OnMouseRightUp(e.Location);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            application.OnMouseMove(e.X, e.Y);
        }
    }
}
