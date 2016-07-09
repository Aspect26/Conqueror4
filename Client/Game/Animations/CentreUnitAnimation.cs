using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CentreUnitAnimation : UnitAnimation
    {

        public CentreUnitAnimation(SimpleUnit unit, string baseImagePath)
            :base(unit, baseImagePath)
        {

        }

        public override void Render(Graphics g)
        {
            int size = unit.UnitSize;

            if (!moving)
            {
                g.DrawImageAt(noAnimationImage, Application.WIDTH / 2, Application.HEIGHT / 2, size, size);
            }
            else
            {
                g.DrawImageAt(images[currentImageSide][currentImageIndex],
                    Application.WIDTH / 2, Application.HEIGHT / 2, size, size);
            }
        }
    }
}
