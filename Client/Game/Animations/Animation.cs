using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    interface IAnimation
    {
        void Render(Graphics g);
        void Render(Graphics g, Location offset);
        void AnimateCycle(int timeSpan);
        Image GetCurrentImage();
    }
}
