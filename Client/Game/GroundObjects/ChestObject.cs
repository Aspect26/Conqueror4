using System;
using System.Drawing;

namespace Client
{
    public class ChestObject : GroundObject
    {
        private IItem containingItem;

        public ChestObject(Game game, IItem containingItem, Point location) :
            base(game, 16, GameData.GetChestImage(), location)
        {
            this.containingItem = containingItem;
        }

        public override void Collide()
        {
            throw new NotImplementedException();
        }
    }
}
