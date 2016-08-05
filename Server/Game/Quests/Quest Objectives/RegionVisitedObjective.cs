using System;
using System.Drawing;

namespace Server
{
    public sealed class RegionVisitedObjective : IQuestObjective
    {
        private Point regionCenter;
        private int centerDistance;

        public bool IsCompleted { get; private set; }

        public RegionVisitedObjective(Point regionCenter, int centerDistance)
        {
            this.regionCenter = regionCenter;
            this.centerDistance = centerDistance;
        }

        public string GetCodedData()
        {
            return "R^Whiteleaf Forest";
        }

        public bool MovedTo(int x, int y)
        {
            if (IsCompleted)
                return false; 

            Point movedPoint = new Point(x, y);
            if (regionCenter.DistanceFrom(movedPoint) <= centerDistance)
            {
                IsCompleted = true;
                return true;
            }

            return false;
        }

        public bool Killed(int unitId) { return false; }

        public bool Visited(int unitId) { return false; }
    }
}
