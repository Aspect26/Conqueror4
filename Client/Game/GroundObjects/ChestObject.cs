using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a chest object. 
    /// A chest object contains exactly one item which a player can take and equip it to it's corresponding slot.
    /// </summary>
    /// <seealso cref="Client.GroundObject" />
    public class ChestObject : GroundObject
    {
        private const int COLLIDING_RANGE = 24;
        private IItem containingItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChestObject"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="containingItem">The containing item.</param>
        /// <param name="location">The location.</param>
        public ChestObject(Game game, IItem containingItem, Point location) :
            base(game, COLLIDING_RANGE, GameData.GetChestImage(), location, containingItem.UniqueID)
        {
            this.containingItem = containingItem;
        }

        /// <summary>
        /// Informs the game that the player is near a dropped item so it can be shown in the right side overlay.
        /// </summary>
        public override void Collide()
        {
            game.SetDroppedItem(containingItem);
        }

        /// <summary>
        /// Informs the game that the player is no longer near the dropped item so it can be removed from the right side overlay.
        /// </summary>
        public override void Leave()
        {
            game.TryRemoveDroppedItem(containingItem);
        }
    }
}
