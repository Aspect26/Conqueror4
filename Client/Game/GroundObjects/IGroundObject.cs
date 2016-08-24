using System.Drawing;

namespace Client
{
    /// <summary>
    /// Interface for all ground objects.
    /// It shall take care of rendering the object and colliding with the player's character.
    /// </summary>
    public interface IGroundObject
    {
        /// <summary>
        /// Renders the object.
        /// </summary>
        /// <param name="g">The g.</param>
        void Render(Graphics g);

        /// <summary>
        /// Gets the object's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }

        /// <summary>
        /// Gets the object's location.
        /// </summary>
        /// <value>The location.</value>
        Point Location { get; }

        /// <summary>
        /// Gets the collision distance.
        /// </summary>
        /// <returns>The distance</returns>
        int GetCollisionDistance();

        /// <summary>
        /// Implements what the object should do if it collides with a player.
        /// </summary>
        void Collide();

        /// <summary>
        /// Implements what the object should do when it is no longer in a collision distance with the player.
        /// </summary>
        void Leave();
    }
}
