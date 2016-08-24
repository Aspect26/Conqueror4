using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a missile shot by either a player or an NPC unit.
    /// Every missile has its image, current location, it's direction vector and life span.
    /// </summary>
    public class Missile
    {
        // direction vector
        private Point direction;

        private Point location;
        private Image image;
        private Game game;
        private int lifeSpan;

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The unit which shot the missile.</value>
        public IUnit Source { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dead.
        /// If a missile dies it is removed from the game later in that game's cycle.
        /// </summary>
        /// <value><c>true</c> if this instance is dead; otherwise, <c>false</c>.</value>
        public bool IsDead { get; private set; }

        // TODO: this shall be in the server message for every missile.
        private const int IMPLIICT_LIFE_SPAN = 333;

        /// <summary>
        /// Initializes a new instance of the <see cref="Missile"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="source">The source unit.</param>
        /// <param name="image">The image.</param>
        /// <param name="location">The location.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="lifeSpan">The life span.</param>
        public Missile(Game game, IUnit source, Image image, Point location, Point direction, 
            int lifeSpan = IMPLIICT_LIFE_SPAN)
        {
            this.game = game;
            this.Source = source;
            this.image = image;
            this.location = location;
            this.lifeSpan = lifeSpan;
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
        /// Plays one cycle.
        /// Moves the missile in the direction of it's direction vector. 
        /// </summary>
        /// <param name="timeSpan">The time span from the last update.</param>
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

        /// <summary>
        /// Hits the specified unit.
        /// The missile is informed that it hit a unit. For a generic missile (currently there are no other implemented)  is
        /// means that it dies.
        /// </summary>
        /// <param name="unit">The hitted unit.</param>
        public void HitUnit(GenericUnit unit)
        {
            this.IsDead = true;
        }

        /// <summary>
        /// Renders the missile.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public void Render(Graphics g)
        {
            Point position = game.MapPositionToScreenPosition(location);
            g.DrawImageAt(image, position);
        }
    }
}
