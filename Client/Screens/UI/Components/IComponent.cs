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
        IComponent GetNeighbour();
        void SetNeighbour(IComponent neighbour);

        void OnKeyDown(int key);
        void OnKeyUp(int key);
        void OnLeftMouseDown(Point position);
        void OnLeftMouseUp(Point position);
        void OnRightMouseDown(Point position);
        void OnRightMouseUp(Point position);

        int WIDTH { get; }
        int HEIGHT { get; }
        int X { get; }
        int Y { get; }
    }
}
