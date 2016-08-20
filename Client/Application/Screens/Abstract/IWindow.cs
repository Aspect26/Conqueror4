using System;
using System.Drawing;

namespace Client
{
    public interface IWindow
    {
        void Render(Graphics g);

        void OnKeyDown(int key);
        void OnKeyUp(int key);
        void OnMouseLeftDown(Point location);
        void OnMouseLeftUp(Point location);
        void OnMouseRightDown(Point location);
        void OnMouseRightUp(Point location);
        void OnMouseMove(int x, int y);
    }
}
