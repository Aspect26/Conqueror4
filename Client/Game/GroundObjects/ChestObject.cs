using System.Drawing;

namespace Client
{
    public class ChestObject : GroundObject
    {
        private const int COLLIDING_RANGE = 24;
        private IItem containingItem;

        public ChestObject(Game game, IItem containingItem, Point location) :
            base(game, COLLIDING_RANGE, GameData.GetChestImage(), location, containingItem.UniqueID)
        {
            this.containingItem = containingItem;
        }

        public override void Collide()
        {
            game.SetDroppedItem(containingItem);
        }

        public override void Leave()
        {
            game.TryRemoveDroppedItem(containingItem);
        }
    }
}
