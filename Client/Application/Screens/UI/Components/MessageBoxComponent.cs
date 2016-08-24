using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a message box. Note that only one message box can be visible in one instance of user interface. 
    /// The message box is always drawn in the middle of the screen.
    /// </summary>
    /// <seealso cref="Client.BorderedRectangleComponent" />
    public class MessageBoxComponent : BorderedRectangleComponent
    {
        /// <summary>
        /// Delegate OnCloseEvent
        /// </summary>
        /// <param name="messageBox">The message box.</param>
        public delegate void OnCloseEvent(MessageBoxComponent messageBox);
        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event OnCloseEvent Closed;

        private const int MARGIN = 5;

        private UserInterface container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxComponent"/> class.
        /// Inserts the LineText and Btton components into the container and sets the OnClick action.
        /// </summary>
        /// <param name="text">The text.</param>
        public MessageBoxComponent(string text)
            : base(new Point(0,0), new Rectangle((Application.WIDTH - (Application.WIDTH / 5) * 4) / 2, (Application.HEIGHT - 96) / 2,
                (Application.WIDTH / 5) * 4, 96), Color.White)
        {
            Point thisPoisition = new Point(this.position.X, this.position.Y);
            container = new UserInterface();
            container.AddComponent(
                new LineText(thisPoisition, text, Color.Black,
                    new Point(borderSize + MARGIN, borderSize + MARGIN), 12));

            int btn_width = 50;
            int btn_height = 30;
            Button OKButton = new Button(thisPoisition, "Ok", new Rectangle(
                this.WIDTH / 2 - btn_width / 2, this.HEIGHT - borderSize - MARGIN - btn_height, btn_width, btn_height));
            OKButton.OnClick += OnOKClicked;
            container.AddComponent(OKButton);

            container.SetFocusedComponent(OKButton);
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            container.Render(g);
        }

        private void OnOKClicked(Button b, EventArgs args)
        {
            Closed(this);
        }

        /// <summary>
        /// Called when [key down].
        /// Redirects the event to the container.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyDown(int key)
        {
            container.OnKeyDown(key);
        }

        /// <summary>
        /// Called on [key up] event.
        /// Redirects the event to the container.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyUp(int key)
        {
            container.OnKeyUp(key);
        }

        /// <summary>
        /// Called on [mouse left down] event.
        /// Redirects the event to the container.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftDown(Point position)
        {
            container.OnMouseLeftDown(position);
        }

        /// <summary>
        /// Called on [mouse left up] event.
        /// Redirects the event to the container.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftUp(Point position)
        {
            container.OnMouseLeftUp(position);
        }
    }
}
