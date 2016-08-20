using System;
using System.Drawing;

namespace Client
{
    public abstract class GroundObject : IGroundObject
    {
        public Point Location { get; protected set; }

        protected int collisionDistance;
        protected Image image;
        protected Game game;

        public GroundObject(Game game, int collisionDistance = 16) 
            : this(game, collisionDistance, GameData.GetUnknownGroundObjectImage(), new Point())
        {
        }

        public GroundObject(Game game, int collisionDistance, Image image, Point location)
        {
            this.image = image;
            this.Location = location;
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
            g.DrawImageAt(image, game.MapPositionToScreenPosition(Location));
        }
    }
}
