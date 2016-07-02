﻿using System.Drawing;

namespace Client
{
    public sealed class PasswordInput : LineInput
    {
        public PasswordInput(Point position, int width, Color textColor, Color background) 
            : base(position, width, textColor, background)
        {
        }

        protected override void RenderText(Graphics g)
        {
            string hiddenString = new string('*', textInput.Length);
            g.DrawString(hiddenString, font, brush, position.X + borderSize * 2, position.Y + borderSize);
        }

        protected override void RenderCursor(Graphics g)
        {
            int size = 3;
            string hiddenString = new string('*', textInput.Length);
            int textWidth = (int)g.MeasureString(hiddenString, font).Width;
            g.FillRectangle(brush, position.X + borderSize * 2 + textWidth,
                position.Y + borderSize + MARGIN + FONTSIZE - size, FONTSIZE, size);
        }
    }
}
