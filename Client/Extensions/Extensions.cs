using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Client
{
    /// <summary>
    /// Contains all custom extension functions and miscellaneous functions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Draws the image at specified location.
        /// Difference between Graphics.DrawImage is that the position passed to the function
        /// is understood as a center of the image, not top left corner.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="image">The image.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void DrawImageAt(this Graphics g, Image image, int x, int y, int width, int height)
        {
            g.DrawImage(image, x - image.Width / 2, y - image.Height / 2, width, height);
        }

        /// <summary>
        /// Draws the image to the graphics with specified center.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="image">The image.</param>
        /// <param name="position">The position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void DrawImageAt(this Graphics g, Image image, Point position, int width, int height)
        {
            g.DrawImageAt(image, position.X, position.Y, width, height);
        }

        /// <summary>
        /// Draws the image to the graphics with specified center and doesn't change its proportions
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="image">The image.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void DrawImageAt(this Graphics g, Image image, int x, int y)
        {
            g.DrawImageAt(image, x, y, image.Width, image.Height);
        }

        /// <summary>
        /// Draws the image to the graphics with specified center and doesn't change its proportions
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="image">The image.</param>
        /// <param name="location">The location.</param>
        public static void DrawImageAt(this Graphics g, Image image, Point location)
        {
            g.DrawImageAt(image, location.X, location.Y, image.Width, image.Height);
        }

        /// <summary>
        /// Draws the circle with specified center and radius.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="center">The center.</param>
        /// <param name="r">The radius.</param>
        public static void DrawCircleAt(this Graphics g, Pen pen, Point center, int r)
        {
            g.DrawEllipse(pen, center.X - r, center.Y - r, 2*r, 2*r);
        }

        /// <summary>
        /// Computes a distance from the point given.
        /// </summary>
        /// <param name="source">The source point.</param>
        /// <param name="host">The remote point.</param>
        /// <returns>The distance.</returns>
        public static int DistanceFrom(this Point source, Point host)
        {
            Point distancePoint = new Point(host.X - source.X, host.Y - source.Y);
            return (int)Math.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);
        }

        /// <summary>
        /// A method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE:
        /// Positive values will rotate clockwise
        /// Negative values will rotate counter-clockwise</param>
        /// <returns>The rotated image.</returns>
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

        /// <summary>
        /// Gets the current milliseconds.
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long GetCurrentMillis()
        {
            return (Stopwatch.GetTimestamp() * 1000) / Stopwatch.Frequency;
        }
    }
}
