using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class Button : RectangleComponent
    {
        private string text;
        private int x, y;

        private static Font font;
        private static Brush brush = Brushes.Yellow;

        public Button(string text, Rectangle position): base(position, Game.GetButtonBackground())
        {
            this.text = text;
            font = new Font(FontFamily.GenericMonospace, position.Height - 20);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            x = (position.X + position.Width / 2) - (int)g.MeasureString(text, font).Width / 2;
            y = (position.Y + position.Height/2) - (int)g.MeasureString(text, font).Height / 2;
            g.DrawString(text, font, brush, x, y);
        }

        private void setPressed(bool pressed)
        {
            if (pressed)
                this.backgroundImage = Game.GetButtonBackgroundPressed();
            else
                this.backgroundImage = Game.GetButtonBackground();
        }

        public override void OnLeftMouseDown(Point position)
        {
            setPressed(true);
        }

        public override void OnLeftMouseUp(Point position)
        {
            setPressed(false);
        }

        public override void SetFocused(bool focused)
        {
            base.SetFocused(focused);

            setPressed(focused);
        }
    }
}
