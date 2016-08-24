using System.Drawing;

namespace Client
{
    /// <summary>
    /// A specific animation for player's character.
    /// Actually no other animation is currently imeplemented
    /// </summary>
    /// <seealso cref="Client.UnitAnimation" />
    public class CentreUnitAnimation : UnitAnimation
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CentreUnitAnimation"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="baseImagePath">The base image path.</param>
        public CentreUnitAnimation(PlayedCharacter unit, string baseImagePath)
            :base(unit, baseImagePath)
        {

        }

        /// <summary>
        /// Renders the animated object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {

            if (!moving)
            {
                g.DrawImageAt(noAnimationImage, Application.WIDTH / 2, Application.HEIGHT / 2);
            }
            else
            {
                g.DrawImageAt(images[currentImageSide][currentImageIndex],
                    Application.WIDTH / 2, Application.HEIGHT / 2);
            }
        }
    }
}
