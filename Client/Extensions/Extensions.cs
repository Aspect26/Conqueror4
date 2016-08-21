using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

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

        public static void DrawCircleAt(this Graphics g, Pen pen, Point center, int r)
        {
            g.DrawEllipse(pen, center.X - r, center.Y - r, 2*r, 2*r);
        }

        public static int DistanceFrom(this Point source, Point host)
        {
            Point distancePoint = new Point(host.X - source.X, host.Y - source.Y);
            return (int)Math.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);
        }

        /// <summary>
        /// method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE: 
        /// Positive values will rotate clockwise
        /// negative values will rotate counter-clockwise
        /// </param>
        /// <returns></returns>
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, 0, 0);

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        public static long GetCurrentMillis()
        {
            return (Stopwatch.GetTimestamp() * 1000) / Stopwatch.Frequency;
        }
    }
}
