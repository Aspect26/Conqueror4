using System;

namespace Shared
{
    /// <summary>
    /// Represents a moving direction of a unit. It is a flag meaning that for 
    /// example a unit can be moving at the same time to the Bottom and Top. In
    /// that case the effect is nulled.
    /// NOTE: Not sure if it is a bug but when the Botton value was not 
    /// initialized it used the same value as Right.
    /// </summary>
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
