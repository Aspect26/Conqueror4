using System.Drawing;

namespace Client
{
    /// <summary>
    /// An abstract class for all ui components in a shape of a rectangle. It takes care of all the common behaviour
    /// for rectangular components such as function IsAt, setters and getters and fields.
    /// </summary>
    /// <seealso cref="Client.IComponent" />
    public abstract class RectangleComponent : BaseComponent
    {
        protected Rectangle position;
        protected Color backgroundColor;
        protected Image backgroundImage;
        protected Brush backgroundBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleComponent"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The component's position.</param>
        /// <param name="background">The background.</param>
        /// <param name="neighbour">The neighbour.</param>
        /// <param name="shown">if set to <c>true</c> the component shall be shown.</param>
        /// <param name="hasTooltip">if set to <c>true</c> the component has a tooltip.</param>
        public RectangleComponent(Point parentPosition, Rectangle position, Color background, 
            IComponent neighbour = null, bool shown = true, bool hasTooltip = false)
            : this(parentPosition, position, neighbour, shown, hasTooltip)
        {
            this.backgroundColor = background;
            this.backgroundBrush = new SolidBrush(background);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleComponent"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The component's position.</param>
        /// <param name="background">The background.</param>
        /// <param name="neighbour">The neighbour.</param>
        /// <param name="shown">if set to <c>true</c> the component shall be shown.</param>
        /// <param name="hasTooltip">if set to <c>true</c> the component has a tooltip.</param>
        public RectangleComponent(Point parentPosition, Rectangle position, Image background, 
            IComponent neighbour = null, bool shown = true, bool hasTooltip = false)
            : this (parentPosition, position, neighbour, shown, hasTooltip)
        {
            this.backgroundImage = background;
        }

        /// <summary>
        /// A private c'tor that has to be called every time! The private variables are being assigned and instantiated 
        /// here.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The component's position.</param>
        /// <param name="neighbour">The neighbour.</param>
        /// <param name="shown">if set to <c>true</c> the component shall be shown..</param>
        /// <param name="hasTooltip">if set to <c>true</c> the component has a tooltip.</param>
        private RectangleComponent(Point parentPosition, Rectangle position, IComponent neighbour = null,
            bool shown = true, bool hasTooltip = false)
        {
            this.position = position;
            this.position.X += parentPosition.X;
            this.position.Y += parentPosition.Y;
            this.neighbour = neighbour;
            this.Shown = shown;
            this.hasTooltip = hasTooltip;
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            if (backgroundImage == null)
                g.FillRectangle(backgroundBrush, position);
            else
                g.DrawImage(backgroundImage, position);
        }

        /// <summary>
        /// Gets the height of the component.
        /// </summary>
        /// <value>The height.</value>
        public override int HEIGHT
        {
            get { return position.Height; }
            protected set { position.Height = value; }
        }

        public override int WIDTH
        {
            get { return position.Width; }
            protected set { position.Width = value; }
        }

        public override int X
        {
            get { return position.X; }
            protected set { position.X = value; }
        }

        public override int Y
        {
            get { return position.Y; }
            protected set { position.Y = value; }
        }

        /// <summary>
        /// Gets the client area.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        public override Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Determines whether the component is at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns><c>true</c> if the component is at the specified location; <c>false</c> otherwise.</returns>
        public override bool IsAt(Point location)
        {
            return position.Contains(location);
        }
    }
}
