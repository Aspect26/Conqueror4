using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public abstract class BorderedRectangleComponent : RectangleComponent
    {
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

        public BorderedRectangleComponent(Point parentPosition, Rectangle position, Color background, 
            int borderSize = UI.DEFAULT_BORDER_HEIGHT, IComponent neighbour = null)
            : base(parentPosition, position, background, neighbour)
        {
            init(borderSize);
        }

        public BorderedRectangleComponent(Point parentPosition, Rectangle position, Image background, 
            int borderSize = UI.DEFAULT_BORDER_HEIGHT, IComponent neighbour = null)
            : base(parentPosition, position, background, neighbour)
        {
            init(borderSize);
        }

        private void init(int borderSize)
        {
            this.borderSize = borderSize;

            this.topBorderImage = GameData.GetUIHorizontalBorder();
            this.bottomBorderImage = GameData.GetUIHorizontalBorder(); bottomBorderImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            this.verticalBorderImage = GameData.GetUIVerticalBorder();

            this.topBorderImageFocused = GameData.GetUIHorizontalBorderFocused();
            this.bottomBorderImageFocused = GameData.GetUIHorizontalBorderFocused(); bottomBorderImageFocused.RotateFlip(RotateFlipType.RotateNoneFlipY);
            this.verticalBorderImageFocused = GameData.GetUIVerticalBorderFocused();

            topBorderRect = new Rectangle(position.X, position.Y, position.Width, borderSize);

            bottomBorderRect = new Rectangle(position.X, position.Y + position.Height - borderSize, 
                position.Width, borderSize);

            leftBorderRect = new Rectangle(position.X, position.Y + borderSize, borderSize, 
                position.Height - 2 * borderSize);

            rightBorderRect = new Rectangle(position.X + position.Width - borderSize, position.Y + borderSize, 
                borderSize, position.Height - 2 * borderSize);
        }

        public override Rectangle GetClientArea()
        {
            return new Rectangle(X + borderSize, Y + borderSize, WIDTH - 2*borderSize, HEIGHT - 2*borderSize);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

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
        }
    }
}
