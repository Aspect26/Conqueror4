using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary>
    /// Represents a UI component. 
    /// </summary>
    public interface IComponent
    {
        void Render(Graphics g);
        void SetFocused(bool focused);
        bool IsAt(Point location);

        void OnKeyDown(int key);
        void OnKeyUp(int key);
        void OnLeftMouseDown(Point position);
        void OnLeftMouseUp(Point position);
        void OnRightMouseDown(Point position);
        void OnRightMouseUp(Point position);

        int WIDTH { get; set; }
        int HEIGHT { get; set; }
    }
}
