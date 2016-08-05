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

        public CentreUnitAnimation(PlayedCharacter unit, string baseImagePath)
            :base(unit, baseImagePath)
        {

        }

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
