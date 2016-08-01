using System;
using System.Drawing;

namespace Server
{
    public class Missile
    {
        private Point direction;
        private int lifeSpan;
        private Point location;
        public IUnit source { get; protected set; }
        public bool IsDead { get; protected set; }
        public int Damage { get; protected set; }

        private const int IMPLIICT_LIFE_SPAN = 333;

        public Missile(IUnit source, Point location, Point direction, int lifeSpan = IMPLIICT_LIFE_SPAN)
        {
            this.source = source;
            this.lifeSpan = lifeSpan;
            this.location = location;
            this.direction = direction;
            this.IsDead = false;

            // TODO: get from character
            this.Damage = 20;
        }

        public Point GetLocation()
        {
            return this.location;
        }

        public void HitUnit(IUnit unit)
        {
            IsDead = true;
            Console.WriteLine("I hit: (" + this.source.GetName() + "=>" + unit.GetName() + ").");
        }

        public void PlayCycle(long timeSpan)
        {
            if (lifeSpan < 1)
            {
                IsDead = true;
                return;
            }

            int movePoints = (int)timeSpan / 2;
            this.location.X += (int)(movePoints * (direction.X / 100f));
            this.location.Y += (int)(movePoints * (direction.Y / 100f));
            this.lifeSpan -= movePoints;
        }
    }
}
