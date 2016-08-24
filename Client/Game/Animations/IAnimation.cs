using System.Drawing;

namespace Client
{
    /// <summary>
    /// Simple interface for animations.
    /// </summary>
    interface IAnimation
    {
        /// <summary>
        /// Renders the animated object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        void Render(Graphics g);

        /// <summary>
        /// Animates one game cycle.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        void AnimateCycle(int timeSpan);

        /// <summary>
        /// Gets the current image of the animation.
        /// </summary>
        /// <returns>Image.</returns>
        Image GetCurrentImage();
    }
}
