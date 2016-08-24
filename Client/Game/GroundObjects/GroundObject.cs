using System.Drawing;

namespace Client
{
    /// <summary>
    /// A basic abstract class for all ground objects.
    /// It takes care of the object rendering, getters and fields.
    /// </summary>
    /// <seealso cref="Client.IGroundObject" />
    public abstract class GroundObject : IGroundObject
    {
        /// <summary>
        /// Gets the object's location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location { get; protected set; }

        /// <summary>
        /// Gets the object's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; protected set; }

        protected int collisionDistance;
        protected Image image;
        protected Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundObject"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="uid">The uid.</param>
        /// <param name="collisionDistance">The collision distance.</param>
        public GroundObject(Game game, int uid, int collisionDistance = 16) 
            : this(game, collisionDistance, GameData.GetUnknownGroundObjectImage(), new Point(), uid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundObject"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="collisionDistance">The collision distance.</param>
        /// <param name="image">The image.</param>
        /// <param name="location">The location.</param>
        /// <param name="uid">The unique identifier.</param>
        public GroundObject(Game game, int collisionDistance, Image image, Point location, int uid)
        {
            this.image = image;
            this.Location = location;
            this.collisionDistance = collisionDistance;
            this.game = game;
            this.UniqueID = uid;
        }

        /// <summary>
        /// Gets the collision distance.
        /// </summary>
        /// <returns>The distance</returns>
        public int GetCollisionDistance()
        {
            return collisionDistance;
        }

        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="g">The g.</param>
        public virtual void Render(Graphics g)
        {
            g.DrawImageAt(image, game.MapPositionToScreenPosition(Location));
        }

        /// <summary>
        /// Implements what the object should do when it is no longer in a collision distance with the player.
        /// </summary>
        public abstract void Leave();

        /// <summary>
        /// Implements what the object should do if it collides with a player.
        /// </summary>
        public abstract void Collide();
    }
}
