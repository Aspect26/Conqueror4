using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a special effect (e.g.: from an ability).
    /// It shall take care of rendering the effect and it's evolution over time.
    /// </summary>
    public interface ISpecialEffect
    {
        /// <summary>
        /// Renders the effect.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        void Render(Graphics g);

        /// <summary>
        /// Plays one cycle of the effect.
        /// </summary>
        /// <param name="timeSpan">The time span from the last cycle.</param>
        void PlayCycle(int timeSpan);

        /// <summary>
        /// Gets a value indicating whether this instance is dead.
        /// If it is dead, it is removed from the game later in the same 
        /// game cycle.
        /// </summary>
        /// <value><c>true</c> if this instance is dead; otherwise, <c>false</c>.</value>
        bool IsDead { get; }
    }
}
