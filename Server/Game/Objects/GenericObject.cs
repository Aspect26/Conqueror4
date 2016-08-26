using System;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Base abstract implementation that every game object shall inherit from.
    /// It takes care of fields and contains a parsing function (parsing from SQL
    /// database data into an actual object).
    /// </summary>
    /// <seealso cref="Server.IObject" />
    public abstract class GenericObject : IObject
    {
        /// <summary>
        /// Gets the object's unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; }

        /// <summary>
        /// Gets the object's current location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location { get; protected set; }

        /// <summary>
        /// Gets the collision range of the object.
        /// </summary>
        /// <value>The collision range.</value>
        public int CollisionRange { get; protected set; }

        /// <summary>
        /// Gets the server message mark. Each object type (like portal) shall have
        /// its own mark. It is send to a client so it knows which game object this
        /// is. Only a one character long marks are valid (why it is not a char then?)
        /// </summary>
        /// <value>The server message mark.</value>
        public abstract string ServerMessageMark { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="location">The location.</param>
        /// <param name="collisionRange">The collision range.</param>
        public GenericObject(int uid, Point location, int collisionRange)
        {
            this.UniqueID = uid;
            this.Location = location;
            this.CollisionRange = collisionRange;
        }

        /// <summary>
        /// Gets the coded data for server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetCodedData()
        {
            return ServerMessageMark + "|" + UniqueID + "|" + Location.X + "|" + Location.Y 
                + "|" + CollisionRange;
        }

        /// <summary>
        /// Creates a specific object based on the data from SQL database.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>GenericObject.</returns>
        /// <exception cref="NotImplementedException">Tries to create an unknown object of " +
        ///                         "type :" + info.Mark</exception>
        public static GenericObject Create(ObjectInfo info, int uid)
        {
            switch (info.Mark)
            {
                case 'P':
                    return new PortalObject(uid, new Point(info.X, info.Y),
                        Convert.ToInt32(info.SpecialArguments[0]));
                default:
                    throw new NotImplementedException("Tries to create an unknown object of " +
                        "type :" + info.Mark);
            }
        }
    }
}
