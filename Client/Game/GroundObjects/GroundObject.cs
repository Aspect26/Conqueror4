using System;
using System.Drawing;

namespace Client
{
    public abstract class GroundObject : IGroundObject
    {
        protected int collisionDistance;
        protected Point location;
        protected Image image;
        protected Game game;

        public GroundObject(Game game, int collisionDistance = 16) 
            : this(game, collisionDistance, GameData.GetUnknownGroundObjectImage(), new Point())
        {
        }

        public GroundObject(Game game, int collisionDistance, Image image, Point location)
        {
            this.image = image;
            this.location = location;
            this.collisionDistance = collisionDistance;
            this.game = game;
        }

        public abstract void Collide();

        public int GetCollisionDistance()
        {
            return collisionDistance;
        }

        public virtual void Render(Graphics g)
        {
            g.DrawImageAt(image, game.MapPositionToScreenPosition(location));
        }
    }
}
