using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class LineInput : BorderedRectangleComponent
    {
        private string textInput = "";
        private const int HEIGHT = 50;  // 10 + 10 borders, 20 text, 10 margin
        private const int MARGIN = 5;
        private const int FONTSIZE = 20;
        private static Font font = new Font(FontFamily.GenericSerif, FONTSIZE);
        private Brush brush;

        public LineInput(Point position, int width, Color textColor, Color background) 
            : base(new Rectangle(position, new Size(width, HEIGHT)) , background)
        {
            brush = new SolidBrush(textColor);
        }

        public string getValue()
        {
            return textInput;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            RenderText(g);
            if(focused)
                RenderCursor(g);
        }

        private void RenderText(Graphics g)
        {
            g.DrawString(textInput, font, brush, position.X + borderSize * 2, position.Y + borderSize);
        }

        private void RenderCursor(Graphics g)
        {
            int size = 3;
            int textWidth = (int)g.MeasureString(textInput, font).Width;
            g.FillRectangle(brush, position.X + borderSize * 2 + textWidth, 
                position.Y + borderSize + MARGIN + FONTSIZE - size, FONTSIZE, size);
        }


        public override void OnKeyDown(int key)
        {
            char k = (char)key;

            // text
            if( (k >= 'a' && k <='z') || (k >= 'A' && k <= 'Z') || (k >= '0' && k <= '9') )
                textInput += (char)key;

            // backspace
            if (k == 8 && textInput.Length != 0)
                textInput = textInput.Remove(textInput.Length - 2);
        }
    }
}
