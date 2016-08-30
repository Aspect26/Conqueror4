using System.Drawing;
using System.Linq;

namespace Client
{
    /// <summary>
    /// Class BaseComponent.
    /// </summary>
    /// <seealso cref="Client.IComponent" />
    public abstract class BaseComponent : IComponent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseComponent"/> class. 
        /// Initializes the tooltip helper objects.
        /// </summary>
        public BaseComponent()
        {
            this.tooltipFont = GameData.GetFont(TOOLTIP_FONT_SIZE);
            this.tooltipBorderPen = Pens.White;
            this.tooltipBrush = Brushes.White;
            this.tooltipRectangleBrush = Brushes.Black;
        }

        /// <summary>
        /// Gets the height of the component.
        /// </summary>
        /// <value>The height.</value>
        public virtual int HEIGHT { get; protected set; }
        /// <summary>
        /// Gets the width of the component.
        /// </summary>
        /// <value>The width.</value>
        public virtual int WIDTH { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IComponent" /> is shown.
        /// </summary>
        /// <value><c>true</c> if shown; otherwise, <c>false</c>.</value>
        public bool Shown { get; protected set; }


        /// <summary>
        /// Gets the x position of the component.
        /// </summary>
        /// <value>The x.</value>
        public virtual int X { get; protected set; }

        /// <summary>
        /// Gets the y position of the component.
        /// </summary>
        /// <value>The y.</value>
        public virtual int Y { get; protected set; }

        /// <summary>
        /// Changes the height of the component.
        /// </summary>
        /// <param name="height">The new height.</param>
        public virtual void ChangeHeight(int height)
        {
            this.HEIGHT = height;
        }

        /// <summary>
        /// Changes the width of the component.
        /// </summary>
        /// <param name="width">The new width.</param>
        public virtual void ChangeWidth(int width)
        {
            this.WIDTH = width;
        }

        /// <summary>
        /// Gets the client area.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        public abstract Rectangle GetClientArea();

        protected IComponent neighbour;
        /// <summary>
        /// Gets the neighbour component.
        /// </summary>
        /// <returns>the neighbour component or null if there is none.</returns>
        public IComponent GetNeighbour()
        {
            return this.neighbour;
        }

        protected bool hasTooltip;
        /// <summary>
        /// Determines whether this instance has tooltip.
        /// </summary>
        /// <returns><c>true</c> if this instance has tooltip; otherwise, <c>false</c>.</returns>
        public virtual bool HasTooltip()
        {
            return hasTooltip;
        }

        /// <summary>
        /// Determines whether the component is at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns><c>true</c> if the component is at the specified location; <c>false</c> otherwise.</returns>
        public abstract bool IsAt(Point location);

        /// <summary>
        /// Moves the x location by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void MoveX(int amount)
        {
            this.X += amount;
        }

        /// <summary>
        /// Moves the y location by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void MoveY(int amount)
        {
            this.Y += amount;
        }

        /// <summary>
        /// Called when [key down].
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void OnKeyDown(int key) {}

        /// <summary>
        /// Called on [key up] event.
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void OnKeyUp(int key) { }

        /// <summary>
        /// Called on [mouse left down] event.
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="position">The position.</param>
        public virtual void OnMouseLeftDown(Point position) { }

        /// <summary>
        /// Called on [mouse left up] event.
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="position">The position.</param>
        public virtual void OnMouseLeftUp(Point position) { }

        /// <summary>
        /// Called on [mouse right down] event.
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="position">The position.</param>
        public virtual void OnMouseRightDown(Point position) { }

        /// <summary>
        /// Called on [mouse right up] event.
        /// The implicit behaviour for this event is to do nothing.
        /// </summary>
        /// <param name="position">The position.</param>
        public virtual void OnMouseRightUp(Point position) { }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public abstract void Render(Graphics g);

        protected string tooltipText;
        protected Font tooltipFont;
        protected Brush tooltipBrush;
        protected Brush tooltipRectangleBrush;
        protected Pen tooltipBorderPen;
        private const int TOOLTIP_FONT_SIZE = 8;
        /// <summary>
        /// Renders the tooltip.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="position">The tooltip position.</param>
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
            for (int i = 0; i < tooltipLines.Length; i++)
            {
                g.DrawString(tooltipLines[i], tooltipFont, tooltipBrush, x + 2, currentY);
                currentY += TOOLTIP_FONT_SIZE + 2;
            }
        }

        protected bool focused;
        /// <summary>
        /// Sets the component focused or not focused.
        /// </summary>
        /// <param name="focused">if set to <c>true</c> then the component becomes focused.</param>
        public virtual void SetFocused(bool focused)
        {
            this.focused = focused;
        }

        /// <summary>
        /// Determines whether this component is focused.
        /// </summary>
        /// <returns><c>true</c> if this component is focused; otherwise, <c>false</c>.</returns>
        public bool IsFocused()
        {
            return this.focused;
        }

        /// <summary>
        /// Sets the neighbour.
        /// </summary>
        /// <param name="neighbour">The neighbour.</param>
        public void SetNeighbour(IComponent neighbour)
        {
            this.neighbour = neighbour;
        }

        /// <summary>
        /// Sets whether the component shall be shown.
        /// </summary>
        /// <param name="shown">if set to <c>true</c> then the component will be rendered when the ui it belongs to is being rendered.</param>
        public void SetShown(bool shown)
        {
            this.Shown = shown;
        }
    }
}
