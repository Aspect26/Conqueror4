using System.Drawing;

namespace Client
{
    /// <summary>
    /// An attempt for a panel component nut ut is not working very well and is not really finished. 
    /// The newly components added to this panel are automatically moved and resized so they fit the size of
    /// this panel and won't stick out of it.
    /// </summary>
    /// <seealso cref="Client.BorderedRectangleComponent" />
    public class Panel : BorderedRectangleComponent
    {
        protected UserInterface container = new UserInterface();

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="offsetPosition">The offset position.</param>
        /// <param name="position">The position.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="neighbour">The neighbour.</param>
        public Panel(Point offsetPosition, Rectangle position, int borderSize = UserInterface.DEFAULT_BORDER_HEIGHT, 
            IComponent neighbour = null)
            :base (offsetPosition, position, GameData.GetPanelBackground(), borderSize, neighbour)
        {
            
        }

        /// <summary>
        /// Renders this as a BorderedComponent plus renders a special image for a panel and it's containing components.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <seealso cref="Client.BorderedRectangleComponent"/>
        public override void Render(Graphics g)
        {
            base.Render(g);
            g.DrawImage(backgroundImage, new Rectangle(position.X + borderSize, position.Y + borderSize,
                WIDTH - 2 * borderSize, HEIGHT - 2 * borderSize));
            container.Render(g);
        }

        /// <summary>
        /// Adds a new component to the panel and resizes and moves it so it fits the panel.
        /// </summary>
        /// <param name="component">The new component.</param>
        public void AddComponent(IComponent component)
        {
            // top margin
            if (component.X < borderSize)
                component.MoveX(borderSize - component.X);

            // left margin
            if (component.Y < borderSize)
                component.MoveY(borderSize - component.Y);

            // right margin
            if(component.X + component.WIDTH > position.X + WIDTH - borderSize*2)
                component.MoveX( (borderSize + GetClientArea().Width) - (component.X + component.WIDTH));

            // bottom margin
            if (component.Y + component.HEIGHT > position.Y + HEIGHT - borderSize*2)
                component.MoveY((borderSize + GetClientArea().Height) - (component.Y + component.HEIGHT));

            component.MoveX(position.X);
            component.MoveY(position.Y);

            container.AddComponent(component);
        }

        /// <summary>
        /// Always returns falls because panel cannot be focused.
        /// </summary>
        /// <param name="focused">irelevant.</param>
        public override void SetFocused(bool focused) { }

        // ******************************************************
        // EVENTS - pass down to container
        // ******************************************************
        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyDown(int key)
        {
            container.OnKeyDown(key);
        }

        /// <summary>
        /// Called on [key up] event.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyUp(int key)
        {
            container.OnKeyUp(key);
        }

        /// <summary>
        /// Called on [mouse left down] event.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftDown(Point position)
        {
            container.OnMouseLeftDown(position);
        }

        /// <summary>
        /// Called on [mouse right down] event.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseRightDown(Point position)
        {
            container.OnMouseRightDown(position);
        }

        /// <summary>
        /// Called on [mouse right up] event.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseRightUp(Point position)
        {
            container.OnMouseRightUp(position);
        }

        /// <summary>
        /// Called on [mouse left up] event.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftUp(Point position)
        {
            container.OnMouseLeftUp(position);
        }
    }
}
