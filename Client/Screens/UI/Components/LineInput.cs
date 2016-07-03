using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class LineInput : BorderedRectangleComponent
    {
        protected string textInput = "";
        protected const int HEIGHT = 50;  // 10 + 10 borders, 20 text, 10 margin
        protected const int MARGIN = 5;
        protected const int FONTSIZE = 20;
        protected static Font font = new Font(FontFamily.GenericSerif, FONTSIZE);
        protected Brush brush;

        public LineInput(Point parentPosition, Point position, int width, Color textColor, Color background, IComponent neighbour = null) 
            : base(parentPosition, new Rectangle(position, new Size(width, HEIGHT)) , background, UI.DEFAULT_BORDER_HEIGHT, neighbour)
        {
            brush = new SolidBrush(textColor);
        }

        public virtual string getValue()
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

        protected virtual void RenderText(Graphics g)
        {
            g.DrawString(visibleText(g, textInput), font, brush, position.X + borderSize + MARGIN, position.Y + borderSize);
        }

        protected virtual void RenderCursor(Graphics g)
        {
            int size = 3;
            int textWidth = (int)g.MeasureString(visibleText(g, textInput), font).Width;
            g.FillRectangle(brush, position.X + borderSize * 2 + textWidth, 
                position.Y + borderSize + MARGIN + FONTSIZE - size, FONTSIZE, size);
        }

        protected string visibleText(Graphics g, string original)
        {
            string trimmedText = original;
            while ((int)g.MeasureString(trimmedText + "_", font).Width > position.Width - borderSize * 2 - MARGIN * 2)
                trimmedText = trimmedText.Remove(0, 1);

            return trimmedText;
        }


        public override void OnKeyDown(int key)
        {
            char k = (char)key;

            // text
            if( (k >= 'a' && k <='z') || (k >= 'A' && k <= 'Z') || (k >= '0' && k <= '9') )
                textInput += (char)key;

            // backspace
            if (k == 8 && textInput.Length != 0)
            {
                if (textInput.Length != 1)
                    textInput = textInput.Remove(textInput.Length - 2);
                else
                    textInput = "";
            }
        }
    }
}
