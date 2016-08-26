using System.Drawing;

namespace Server
{
    /// <summary>
    /// Represents a game object (currently only a portal is available). The objects
    /// mechanism was implemented in a hurry and most probably could have been done better.
    /// The worst problem with the objects is that items shall also be classified as an
    /// objects but they were implemented much sooner than the objects.
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// Gets the object's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        int UniqueID { get; }

        /// <summary>
        /// Gets the object's current location.
        /// </summary>
        /// <value>The location.</value>
        Point Location { get; }

        /// <summary>
        /// Gets the collision range of the object.
        /// </summary>
        /// <value>The collision range.</value>
        int CollisionRange { get; }

        /// <summary>
        /// Gets the server message mark. Each object type (like portal) shall have 
        /// its own mark. It is send to a client so it knows which game object this
        /// is. Only a one character long marks are valid (why it is not a char then?)
        /// </summary>
        /// <value>The server message mark.</value>
        string ServerMessageMark { get; }

        /// <summary>
        /// Gets the coded data for server message.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetCodedData();
    }
}
