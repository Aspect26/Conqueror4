using System;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Static class containing all the extension functions on the server side.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Counts the distance of a point from another.
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
        /// Gets the current milliseconds.
        /// </summary>
        /// <returns>The current miliseconds.</returns>
        public static long GetCurrentMillis()
        {
            return (Stopwatch.GetTimestamp() * 1000) / Stopwatch.Frequency;
        }
    }
}
