using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Repersents a portal in the game.
    /// </summary>
    /// <seealso cref="Client.GroundObject" />
    public class PortalObject : GroundObject
    {
        private int mapId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalObject"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="uid">The uid.</param>
        /// <param name="location">The location.</param>
        /// <param name="collisionDistance">The collision distance.</param>
        public PortalObject(Game game, int uid, Point location, int collisionDistance,
            int mapId)
            : base(game, collisionDistance, GameData.GetPortalImage(), location, uid)
        {
            this.mapId = mapId;
        }

        /// <summary>
        /// Implements what the object should do if it collides with a player.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Collide()
        {
            game.TryChangeMap(mapId);
        }

        /// <summary>
        /// If the unit enters a portal ther is no way back!
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Leave()
        {
        }
    }
}
