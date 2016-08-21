using System;
using System.Drawing;

namespace Client
{
    public class AbilityPriestHeal : SpecialEffect
    {
        private static Pen circlePen = Pens.Yellow;

        private Point center;
        private int currentDuration = 0;
        private const int duration = 300;

        public AbilityPriestHeal(Game game, Point center)
        {
            this.center = game.MapPositionToScreenPosition(center);
        }

        public override void Render(Graphics g)
        {
            g.DrawCircleAt(circlePen, center, currentDuration);
        }

        public override void PlayCycle(int timeSpan)
        {
            currentDuration += timeSpan;
            if (currentDuration >= duration)
                IsDead = true;
        }
    }
}
