using System.Drawing;

namespace Server
{
    /// <summary>
    /// Represents a missile. A missile is shot either by a player or and NPC unit.
    /// </summary>
    public class Missile
    {
        private Point direction;
        private int lifeSpan;
        private Point location;

        /// <summary>
        /// Gets or sets the unit that shot the missile.
        /// </summary>
        /// <value>The source.</value>
        public IUnit Source { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dead. In the game
        /// cycle, when the missile is moved to its new location it can hit a unit. If
        /// it happens, this flag is set, and it will hit no other unit in the cycle and
        /// will be removed from the list of active missile later in the cycle.
        /// </summary>
        /// <value><c>true</c> if the missile is dead; otherwise, <c>false</c>.</value>
        public bool IsDead { get; protected set; }

        /// <summary>
        /// Gets the damage of the missile.
        /// </summary>
        /// <value>The damage.</value>
        public int Damage { get { return Source.GetDamage(); } }

        // TODO: this is terribly wrong, this shall be get from a unit and unit shall
        // have it loaded from the database. The client also uses this magical constant.
        // Changing this constant in only one of the two application will cause many
        // weird effects in the game.
        private const int IMPLIICT_LIFE_SPAN = 333;

        /// <summary>
        /// Initializes a new instance of the <see cref="Missile"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="location">The location.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="lifeSpan">The life span.</param>
        public Missile(IUnit source, Point location, Point direction, int lifeSpan = IMPLIICT_LIFE_SPAN)
        {
            this.Source = source;
            this.lifeSpan = lifeSpan;
            this.location = location;
            this.direction = direction;
            this.IsDead = false;
        }

        /// <summary>
        /// Gets the missile's current location.
        /// </summary>
        /// <returns>Point.</returns>
        public Point GetLocation()
        {
            return this.location;
        }

        /// <summary>
        /// Hits the unit. Sets the dead flag and informs the unit about being hit.
        /// </summary>
        /// <param name="unit">The unit it hit.</param>
        public void HitUnit(IUnit unit)
        {
            this.IsDead = true;
            unit.HitByMissile(this);
        }

        /// <summary>
        /// Plays one cycle.
        /// </summary>
        /// <param name="timeSpan">The time span from last update.</param>
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
