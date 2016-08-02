using System;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    public static class Extensions
    {
        public static int DistanceFrom(this Point source, Point host)
        {
            Point distancePoint = new Point(host.X - source.X, host.Y - source.Y);
            return (int)Math.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);
        }

        public static long GetCurrentMillis()
        {
            return (Stopwatch.GetTimestamp() * 1000) / Stopwatch.Frequency;
        }
    }
}
