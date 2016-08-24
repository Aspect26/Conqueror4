using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents one game window - e.g.: login screen, characters screen, ...
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// Renders everything on the screen
        /// </summary>
        /// <param name="g">graphics object</param>
        void Render(Graphics g);

        // event handlers
        void OnKeyDown(int key);
        void OnKeyUp(int key);
        void OnMouseLeftDown(Point location);
        void OnMouseLeftUp(Point location);
        void OnMouseRightDown(Point location);
        void OnMouseRightUp(Point location);
        void OnMouseMove(int x, int y);
    }
}
