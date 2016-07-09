using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Extensions
    {
        public static void DrawImageAt(this Graphics g, Image image, int x, int y, int width, int height)
        {
            g.DrawImage(image, x - image.Width / 2, y - image.Height / 2, width, height);
        }
    }
}
