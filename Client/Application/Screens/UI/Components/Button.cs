using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Class Button.
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
    public class Button : RectangleComponent
    {
        protected string text;
        protected int x, y;
        protected bool pressed = false;
        protected bool set = false;

        protected Font font;
        protected Brush brush = Brushes.Black;

        /// <summary>
        /// Delegate OnClickHandler
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public delegate void OnClickHandler(Button button, EventArgs e);

        /// <summary>
        /// Occurs when [on click].
        /// </summary>
        public event OnClickHandler OnClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="text">The text on the button.</param>
        /// <param name="position">The button's position.</param>
        public Button(Point parentPosition, string text, Rectangle position)
            : base(parentPosition, position, GameData.GetButtonBackground())
        {
            this.text = text;
            font = GameData.GetFont((position.Height / 3) * 2);
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);
            RenderText(g);
        }

        protected virtual void RenderText(Graphics g)
        {
            if (!set)
            {
                while (g.MeasureString(text, font).Width > this.WIDTH)
                {
                    int size = (int)font.Size - 1;
                    font = GameData.GetFont(size);
                }
                set = true;
            }

            x = (position.X + position.Width / 2) - (int)g.MeasureString(text, font).Width / 2;
            y = (position.Y + position.Height / 2) - (int)g.MeasureString(text, font).Height / 2;
            g.DrawString(text, font, brush, x, y);
        }

        protected virtual void setPressed(bool pressed)
        {
            if (pressed)
            {
                this.pressed = pressed;
                this.backgroundImage = GameData.GetButtonBackgroundPressed();
            }
            else
            {
                if (this.pressed && OnClick != null)
                    OnClick(this, null);

                this.pressed = pressed;
                this.backgroundImage = GameData.GetButtonBackground();
            }
        }

        /// <summary>
        /// Called when [key down].
        /// Sets the button to be pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyDown(int key)
        {
            if(key == 13) // enter
            {
                setPressed(true);
            }
        }

        /// <summary>
        /// Called on [key up] event.
        /// Unsets the pressed flag.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyUp(int key)
        {
            if (key == 13) // enter
            {
                setPressed(false);
            }
        }

        /// <summary>
        /// Called on [mouse left down] event.
        /// Sets the button to be pressed.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftDown(Point position)
        {
            setPressed(true);
        }

        /// <summary>
        /// Called on [mouse left up] event.
        /// Unsets the pressed flag.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftUp(Point position)
        {
            setPressed(false);
        }
    }
}
