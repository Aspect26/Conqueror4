using System;
using System.Drawing;

namespace Client
{
    public static class Extensions
    {
        public static void DrawImageAt(this Graphics g, Image image, int x, int y, int width, int height)
        {
            g.DrawImage(image, x - image.Width / 2, y - image.Height / 2, width, height);
        }

        public static void DrawImageAt(this Graphics g, Image image, Point position, int width, int height)
        {
            g.DrawImageAt(image, position.X, position.Y, width, height);
        }

        public static void DrawImageAt(this Graphics g, Image image, int x, int y)
        {
            g.DrawImageAt(image, x, y, image.Width, image.Height);
        }

        public static void DrawImageAt(this Graphics g, Image image, Point location)
        {
            g.DrawImageAt(image, location.X, location.Y, image.Width, image.Height);
        }

        public static int DistanceFrom(this Point source, Point host)
        {
            Point distancePoint = new Point(host.X - source.X, host.Y - source.Y);
            return (int)Math.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);
        }
    }
}
