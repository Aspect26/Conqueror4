using System;

namespace Shared
{
    [Flags]
    public enum MovingDirection
    {
        None,
        Up,
        Right,
        Bottom = 4,
        Left = 8,
    }
}
