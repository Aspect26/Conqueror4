using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// An implementation of special effect for Priest's Heal ability.
    /// It is made really simply, it draw a circle whose center is the player
    /// which used the ability and the radius is the time differenc (in milisecons) 
    /// between the time that the ability was used and actual time.
    /// </summary>
    /// <seealso cref="Client.SpecialEffect" />
    public class AbilityPriestHeal : SpecialEffect
    {
        private static Pen circlePen = Pens.Yellow;

        private Point center;
        private int currentDuration = 0;
        private const int duration = 300;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityPriestHeal"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="center">The center.</param>
        public AbilityPriestHeal(Game game, Point center)
        {
            this.center = game.MapPositionToScreenPosition(center);
        }

        /// <summary>
        /// Renders the effect.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            g.DrawCircleAt(circlePen, center, currentDuration);
        }

        /// <summary>
        /// Plays one cycle of the effect.
        /// </summary>
        /// <param name="timeSpan">The time span from the last cycle.</param>
        public override void PlayCycle(int timeSpan)
        {
            currentDuration += timeSpan;
            if (currentDuration >= duration)
                IsDead = true;
        }
    }
}
