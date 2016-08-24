using System.Drawing;

namespace Client
{
    /// <summary>
    /// An abstract class for ui components in a shape of rectangle with a border. It inherits most of the behavriour 
    /// from the RecrangleComponent class (it really only adds the border).
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderedRectangleComponent"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="background">The background.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="neighbour">The neighbour.</param>
        public BorderedRectangleComponent(Point parentPosition, Rectangle position, Color background, 
            int borderSize = UserInterface.DEFAULT_BORDER_HEIGHT, IComponent neighbour = null)
            : base(parentPosition, position, background, neighbour)
        {
            init(borderSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderedRectangleComponent"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="background">The background.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="neighbour">The neighbour.</param>
        public BorderedRectangleComponent(Point parentPosition, Rectangle position, Image background, 
            int borderSize = UserInterface.DEFAULT_BORDER_HEIGHT, IComponent neighbour = null)
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

        /// <summary>
        /// Gets the client area.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        public override Rectangle GetClientArea()
        {
            return new Rectangle(X + borderSize, Y + borderSize, WIDTH - 2*borderSize, HEIGHT - 2*borderSize);
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
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
