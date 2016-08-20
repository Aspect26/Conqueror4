using System;
using System.Drawing;
using System.Linq;

namespace Client
{
    public abstract class RectangleComponent : IComponent
    {
        protected Rectangle position;
        protected Color backgroundColor;
        protected Image backgroundImage;
        protected Brush backgroundBrush;
        protected IComponent neighbour;

        protected bool hasTooltip;
        protected string tooltipText;
        protected Font tooltipFont;
        protected Brush tooltipBrush;
        protected Brush tooltipRectangleBrush;
        protected Pen tooltipBorderPen;
        private const int TOOLTIP_FONT_SIZE = 8;

        public int WIDTH { get { return position.Width; } }
        public int HEIGHT { get { return position.Height; } }
        public int X { get { return position.X; }  }
        public int Y { get { return position.Y; } }

        public bool Shown { get; protected set; }

        protected bool focused = false;

        public RectangleComponent(Point parentPosition, Rectangle position, Color background, 
            IComponent neighbour = null, bool shown = true, bool hasTooltip = false)
            : this(parentPosition, position, neighbour, shown, hasTooltip)
        {
            this.backgroundColor = background;
            this.backgroundBrush = new SolidBrush(background);
        }

        public RectangleComponent(Point parentPosition, Rectangle position, Image background, 
            IComponent neighbour = null, bool shown = true, bool hasTooltip = false)
            : this (parentPosition, position, neighbour, shown, hasTooltip)
        {
            this.backgroundImage = background;
        }

        private RectangleComponent(Point parentPosition, Rectangle position, IComponent neighbour = null,
            bool shown = true, bool hasTooltip = false)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;
            this.neighbour = neighbour;
            this.Shown = shown;
            this.hasTooltip = hasTooltip;
            this.tooltipFont = GameData.GetFont(TOOLTIP_FONT_SIZE);
            this.tooltipBorderPen = Pens.White;
            this.tooltipBrush = Brushes.White;
            this.tooltipRectangleBrush = Brushes.Black;
        }

        public virtual void Render(Graphics g)
        {
            if (backgroundImage == null)
                g.FillRectangle(backgroundBrush, position);
            else
                g.DrawImage(backgroundImage, position);
        }

        public virtual void SetShown(bool shown)
        {
            this.Shown = shown;
        }

        public virtual void SetFocused(bool focused)
        {
            this.focused = focused;
        }

        public IComponent GetNeighbour()
        {
            return neighbour;
        }

        public void SetNeighbour(IComponent neighbour)
        {
            this.neighbour = neighbour;
        }

        public void MoveX(int X)
        {
            position.X += X;
        }

        public void MoveY(int Y)
        {
            position.Y += Y;
        }

        public void ChangeWidth(int width)
        {
            position.Width = width;
        }

        public void ChangeHeight(int height)
        {
            position.Height = height;
        }

        public virtual Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        public bool IsAt(Point location)
        {
            return position.Contains(location);
        }

        public virtual void RenderTooltip(Graphics g, Point position)
        {
            string[] tooltipLines = tooltipText.Split('\n');

            // border
            int height = tooltipLines.Length * (TOOLTIP_FONT_SIZE + 5) + 2;
            int width = (int)g.MeasureString(tooltipLines.OrderByDescending(line => line.Length).First(), 
                tooltipFont).Width + 5;
            int y = position.Y - height;
            int x = position.X;

            g.FillRectangle(tooltipRectangleBrush, x, y, width, height);
            g.DrawRectangle(tooltipBorderPen, x, y, width, height);

            // text
            int currentY = y + 4;
            for(int i = 0; i<tooltipLines.Length; i++)
            {
                g.DrawString(tooltipLines[i], tooltipFont, tooltipBrush, x + 2, currentY);
                currentY += TOOLTIP_FONT_SIZE + 2;
            }
        }

        public virtual bool HasTooltip()
        {
            return hasTooltip;
        }

        public virtual void OnKeyDown(int key) { }
        public virtual void OnKeyUp(int key) { }
        public virtual void OnMouseLeftDown(Point position) { }
        public virtual void OnMouseLeftUp(Point position) { }
        public virtual void OnMouseRightDown(Point position) { }
        public virtual void OnMouseRightUp(Point position) { }
    }
}
