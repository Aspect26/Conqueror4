using System;
using System.Collections.Generic;
using System.Drawing;

namespace Server
{
    public sealed class RegionVisitedObjective : QuestObjective
    {
        private Point regionCenter;
        private int centerDistance;

        public override bool IsCompleted { get; protected set; }

        public RegionVisitedObjective(Point regionCenter, int centerDistance)
        {
            this.regionCenter = regionCenter;
            this.centerDistance = centerDistance;
        }

        public override string GetCodedData()
        {
            return "R^Whiteleaf Forest";
        }

        public override bool MovedTo(int x, int y)
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

        public override void Reset()
        {
            IsCompleted = false;
        }

        public override IQuestObjective Copy()
        {
            return new RegionVisitedObjective(new Point(regionCenter.X, regionCenter.Y), centerDistance);
        }
    }
}
