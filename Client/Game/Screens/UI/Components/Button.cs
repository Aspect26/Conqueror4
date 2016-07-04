using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class Button : RectangleComponent
    {
        protected string text;
        protected int x, y;
        protected bool pressed = false;
        protected bool set = false;

        protected Font font;
        protected Brush brush = Brushes.Black;

        public delegate void OnClickHandler(Button m, EventArgs e);
        public event OnClickHandler Click;

        public Button(Point parentPosition, string text, Rectangle position)
            : base(parentPosition, position, Game.GetButtonBackground())
        {
            this.text = text;
            font = Game.GetFont((position.Height / 3) * 2);
        }

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
                    font = Game.GetFont(size);
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
                this.backgroundImage = Game.GetButtonBackgroundPressed();
            }
            else
            {
                if (this.pressed && Click != null)
                    Click(this, null);

                this.pressed = pressed;
                this.backgroundImage = Game.GetButtonBackground();
            }
        }

        public override void OnKeyDown(int key)
        {
            if(key == 13) // enter
            {
                setPressed(true);
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == 13) // enter
            {
                setPressed(false);
            }
        }

        public override void OnMouseLeftDown(Point position)
        {
            setPressed(true);
        }

        public override void OnMouseLeftUp(Point position)
        {
            setPressed(false);
        }

        public override void SetFocused(bool focused)
        {
            base.SetFocused(focused);

        }
    }
}
