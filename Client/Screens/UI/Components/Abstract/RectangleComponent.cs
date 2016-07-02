using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class RectangleComponent : IComponent
    {
        protected Rectangle position;
        protected Color backgroundColor;
        protected Image backgroundImage;
        protected Brush backgroundBrush;

        public int WIDTH { get; set; }
        public int HEIGHT { get; set; }

        protected bool focused = false;

        public RectangleComponent(Rectangle position, Color background)
        {
            this.position = position;
            this.backgroundColor = background;

            this.backgroundBrush = new SolidBrush(background);

            this.WIDTH = position.Width;
            this.HEIGHT = position.Height;
        }

        public RectangleComponent(Rectangle position, Image background)
        {
            this.position = position;
            this.backgroundImage = background;

            this.WIDTH = position.Width;
            this.HEIGHT = position.Height;
        }

        public virtual void Render(Graphics g)
        {
            if (backgroundImage == null)
                g.FillRectangle(backgroundBrush, position);
            else
                g.DrawImage(backgroundImage, position);

            g.DrawRectangle(Pens.Red, position);
        }

        public virtual void SetFocused(bool focused)
        {
            this.focused = focused;
        }

        public bool IsAt(Point location)
        {
            return position.Contains(location);
        }

        public virtual void OnKeyDown(int key) { }
        public virtual void OnKeyUp(int key) { }
        public virtual void OnLeftMouseDown(Point position) { }
        public virtual void OnLeftMouseUp(Point position) { }
        public virtual void OnRightMouseDown(Point position) { }
        public virtual void OnRightMouseUp(Point position) { }
    }
}
