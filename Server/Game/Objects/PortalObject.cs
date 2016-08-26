using System.Drawing;

namespace Server
{
    /// <summary>
    /// A game object implementation of a portal. A portal transfers a unit (if it
    /// comes close enough - to collision range) to another map instance.
    /// This shall be reimplemented so it also holds information of x and y coordinate
    /// of the destination location.
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Server.GenericObject" />
    public sealed class PortalObject : GenericObject
    {
        private const int COLLISION_RANGE = 64;

        private int mapId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalObject"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="location">The location.</param>
        /// <param name="mapId">The map identifier.</param>
        public PortalObject(int uid, Point location, int mapId)
            :base(uid, location, COLLISION_RANGE)
        {
            this.mapId = mapId;
        }

        /// <summary>
        /// Gets the server message mark. Each object type (like portal) shall have
        /// its own mark. It is send to a client so it knows which game object this
        /// is. This game object's mark is 'P'. 
        /// </summary>
        /// <value>The server message mark.</value>
        public override string ServerMessageMark { get { return "P"; } }

        /// <summary>
        /// Gets the coded data for server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetCodedData()
        {
            return base.GetCodedData() + "|" + mapId;
        }
    }
}
