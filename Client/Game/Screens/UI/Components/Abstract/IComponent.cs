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

        void MoveX(int X);
        void MoveY(int Y);
        void ChangeWidth(int width);
        void ChangeHeight(int height);

        void OnKeyDown(int key);
        void OnKeyUp(int key);
        void OnMouseLeftDown(Point position);
        void OnMouseLeftUp(Point position);
        void OnMouseRightDown(Point position);
        void OnMouseRightUp(Point position);

        int WIDTH { get; }
        int HEIGHT { get; }
        int X { get; }
        int Y { get; }
    }
}
