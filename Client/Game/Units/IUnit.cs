using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IUnit
    {
        void PlayCycle(int timeSpan);
        void DrawUnit(Graphics g);
        Image GetCurrentImage();

        void StartMovingUp();
        void StartMovingRight();
        void StartMovingBottom();
        void StartMovingLeft();

        void StopMovingUp();
        void StopMovingRight();
        void StopMovingBottom();
        void StopMovingLeft();
    }

    public enum MovingDirection
    {
        None,

        Top,
        TopRight,

        Right,
        BottomRight,

        Bottom,
        BottomLeft,

        Left,
        TopLeft
    }
}
