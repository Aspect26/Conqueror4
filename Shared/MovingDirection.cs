using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
