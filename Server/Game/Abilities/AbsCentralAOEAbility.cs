using System.Drawing;

namespace Server
{
    public abstract class CentralAOEAbility : Ability
    {
        protected int range;
        protected bool hitFriendly;
        protected Point center;

        public CentralAOEAbility(IUnit source, int manaCost, int range, bool hitFriendly = false)
            :base(source, manaCost)
        {
            this.range = range;
            this.hitFriendly = hitFriendly;
            this.center = new Point(source.GetLocation().X, source.GetLocation().Y);
        }

        public override void Process(MapInstance mapInstance)
        {
            base.Process(mapInstance);

            foreach(IUnit unit in mapInstance.GetUnits())
            {
                Point unitPoint = new Point(unit.GetLocation().X, unit.GetLocation().Y);
                int distance = unitPoint.DistanceFrom(center);
                if (distance > range)
                    continue;

                if(unit.Fraction != Source.Fraction && !hitFriendly)
                {
                    hitUnit(unit, distance);
                }
                else if (unit.Fraction == Source.Fraction && hitFriendly)
                {
                    hitUnit(unit, distance);
                }
            }
        }

        protected abstract void hitUnit(IUnit unit, int distance);

        public override string GetCodedData()
        {
            return Source.GetLocation().X + "&" + Source.GetLocation().Y;
        }
    }
}
