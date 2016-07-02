using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class BorderedRectangleComponent : IComponent
    {
        protected Rectangle position;
        protected Color background;
        protected int borderSize;

        private Image topBorderImage;
        private Image bottomBorderImage;
        private Image verticalBorderImage;

        private Image topBorderImageFocused;
        private Image bottomBorderImageFocused;
        private Image verticalBorderImageFocused;

        private Rectangle topBorderRect;
        private Rectangle leftBorderRect;
        private Rectangle bottomBorderRect;
        private Rectangle rightBorderRect;

        public int WIDTH {  get; set; }
        public int HEIGHT { get; set; }

        protected bool focused = false;

        public BorderedRectangleComponent(Rectangle position, Color background, int borderSize = UI.DEFAULT_BORDER_HEIGHT)
        {
            this.position = position;
            this.background = background;
            this.borderSize = borderSize;

            this.WIDTH = position.Width;
            this.HEIGHT = position.Height;

            this.topBorderImage = Game.GetUIHorizontalBorder();
            this.bottomBorderImage = Game.GetUIHorizontalBorder(); bottomBorderImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            this.verticalBorderImage = Game.GetUIVerticalBorder();

            this.topBorderImageFocused = Game.GetUIHorizontalBorderFocused();
            this.bottomBorderImageFocused = Game.GetUIHorizontalBorderFocused(); bottomBorderImageFocused.RotateFlip(RotateFlipType.RotateNoneFlipY);
            this.verticalBorderImageFocused = Game.GetUIVerticalBorderFocused();

            topBorderRect = new Rectangle(position.X, position.Y, position.Width, borderSize);
            bottomBorderRect = new Rectangle(position.X, position.Y + position.Height - borderSize, position.Width, borderSize);
            leftBorderRect = new Rectangle(position.X, position.Y + borderSize, borderSize, position.Height - 2 * borderSize);
            rightBorderRect = new Rectangle(position.X + position.Width - borderSize, position.Y + borderSize, borderSize, position.Height - 2 * borderSize);
        }
             
        public virtual void Render(Graphics g)
        {
            renderBorder(g);
        }

        private void renderBorder(Graphics g)
        {
            if (focused)
            {
                g.DrawImage(topBorderImageFocused, topBorderRect);
                g.DrawImage(verticalBorderImageFocused, leftBorderRect);
                g.DrawImage(bottomBorderImageFocused, bottomBorderRect);
                g.DrawImage(verticalBorderImageFocused, rightBorderRect);
            }
            else
            {
                g.DrawImage(topBorderImage, topBorderRect);
                g.DrawImage(verticalBorderImage, leftBorderRect);
                g.DrawImage(bottomBorderImage, bottomBorderRect);
                g.DrawImage(verticalBorderImage, rightBorderRect);
            }

            g.DrawRectangle(Pens.Green, position);
        }

        public void SetFocused(bool focused)
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
