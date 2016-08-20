using System;
using System.Drawing;

namespace Client
{
    public abstract class GroundObject : IGroundObject
    {
        public Point Location { get; protected set; }
        public int UniqueID { get; protected set; }

        protected int collisionDistance;
        protected Image image;
        protected Game game;

        public GroundObject(Game game, int uid, int collisionDistance = 16) 
            : this(game, collisionDistance, GameData.GetUnknownGroundObjectImage(), new Point(), uid)
        {
        }

        public GroundObject(Game game, int collisionDistance, Image image, Point location, int uid)
        {
            this.image = image;
            this.Location = location;
            this.collisionDistance = collisionDistance;
            this.game = game;
            this.UniqueID = uid;
        }

        public int GetCollisionDistance()
        {
            return collisionDistance;
        }

        public virtual void Render(Graphics g)
        {
            g.DrawImageAt(image, game.MapPositionToScreenPosition(Location));
        }

        public abstract void Leave();
        public abstract void Collide();
    }
}
