using System.Drawing;

namespace Client
{
    public class ImageComponent : RectangleComponent
    {
        public ImageComponent(Point parentPosition, Rectangle position, Image image)
            :base(parentPosition, position, image)
        {
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
        }
    }
}
