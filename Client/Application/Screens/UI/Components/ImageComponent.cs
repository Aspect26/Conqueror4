using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a component whose background is an image.
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
    public class ImageComponent : RectangleComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageComponent"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="image">The image.</param>
        public ImageComponent(Point parentPosition, Rectangle position, Image image)
            :base(parentPosition, position, image)
        {
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);
        }
    }
}
