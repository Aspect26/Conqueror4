using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a UI component.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        void Render(Graphics g);

        /// <summary>
        /// Renders the tooltip.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="position">The tooltip position.</param>
        void RenderTooltip(Graphics g, Point position);

        /// <summary>
        /// Sets the component focused or not focused.
        /// </summary>
        /// <param name="focused">if set to <c>true</c> then the component becomes focused.</param>
        void SetFocused(bool focused);

        /// <summary>
        /// Determines whether this component is focused.
        /// </summary>
        /// <returns><c>true</c> if this component is focused; otherwise, <c>false</c>.</returns>
        bool IsFocused();

        /// <summary>
        /// Determines whether the component is at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns><c>true</c> if the component is at the specified location; <c>false</c> otherwise.</returns>
        bool IsAt(Point location);

        /// <summary>
        /// Gets the neighbour component.
        /// </summary>
        /// <returns>the neighbour component or null if there is none.</returns>
        IComponent GetNeighbour();

        /// <summary>
        /// Sets the neighbour.
        /// </summary>
        /// <param name="neighbour">The neighbour.</param>
        void SetNeighbour(IComponent neighbour);

        /// <summary>
        /// Moves the x location by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        void MoveX(int amount);

        /// <summary>
        /// Moves the y location by the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        void MoveY(int amount);

        /// <summary>
        /// Changes the width of the component.
        /// </summary>
        /// <param name="width">The new width.</param>
        void ChangeWidth(int width);

        /// <summary>
        /// Changes the height of the component.
        /// </summary>
        /// <param name="height">The new height.</param>
        void ChangeHeight(int height);

        // ************************************
        // EVENTS
        // ************************************
        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="key">The key.</param>
        void OnKeyDown(int key);

        /// <summary>
        /// Called on [key up] event.
        /// </summary>
        /// <param name="key">The key.</param>
        void OnKeyUp(int key);

        /// <summary>
        /// Called on [mouse left down] event.
        /// </summary>
        /// <param name="position">The position.</param>
        void OnMouseLeftDown(Point position);

        /// <summary>
        /// Called on [mouse left up] event.
        /// </summary>
        /// <param name="position">The position.</param>
        void OnMouseLeftUp(Point position);

        /// <summary>
        /// Called on [mouse right down] event.
        /// </summary>
        /// <param name="position">The position.</param>
        void OnMouseRightDown(Point position);

        /// <summary>
        /// Called on [mouse right up] event.
        /// </summary>
        /// <param name="position">The position.</param>
        void OnMouseRightUp(Point position);

        // ************************************
        // FIELDS
        // ************************************
        /// <summary>
        /// Gets the width of the component.
        /// </summary>
        /// <value>The width.</value>
        int WIDTH { get; }

        /// <summary>
        /// Gets the height of the component.
        /// </summary>
        /// <value>The height.</value>
        int HEIGHT { get; }
        /// <summary>
        /// Gets the x position of the component.
        /// </summary>
        /// <value>The x.</value>
        int X { get; }

        /// <summary>
        /// Gets the y position of the component.
        /// </summary>
        /// <value>The y.</value>
        int Y { get; }



        /// <summary>
        /// Gets the client area.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        Rectangle GetClientArea();

        /// <summary>
        /// Sets whether the component shall be shown.
        /// </summary>
        /// <param name="shown">if set to <c>true</c> then the component will be rendered when the ui it belongs to is being rendered.</param>
        void SetShown(bool shown);

        /// <summary>
        /// Gets a value indicating whether this <see cref="IComponent"/> is shown.
        /// </summary>
        /// <value><c>true</c> if shown; otherwise, <c>false</c>.</value>
        bool Shown { get; }

        /// <summary>
        /// Determines whether this instance has tooltip.
        /// </summary>
        /// <returns><c>true</c> if this instance has tooltip; otherwise, <c>false</c>.</returns>
        bool HasTooltip();
    }
}
