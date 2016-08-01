using Shared;
using System.Drawing;

namespace Client
{
    public class Missile
    {
        // direction vector
        private Point direction;

        private Point location;
        private Image image;
        private Game game;
        private int lifeSpan;

        public IUnit Source { get; protected set; }
        public bool IsDead { get; private set; }

        private const int IMPLIICT_LIFE_SPAN = 333;

        public Missile(Game game, IUnit source, Image image, Point location, Point direction, 
            int lifeSpan = IMPLIICT_LIFE_SPAN)
        {
            this.game = game;
            this.Source = source;
            this.image = image;
            this.location = location;
            this.lifeSpan = lifeSpan;
            this.direction = direction;
            this.IsDead = false;
        }

        public Point GetLocation()
        {
            return this.location;
        }

        public void PlayCycle(long timeSpan)
        {
            if (lifeSpan < 1)
            {
                IsDead = true;
                return;
            }

            int movePoints = (int)timeSpan / 2;
            this.location.X += (int)(movePoints * (direction.X / 100f));
            this.location.Y += (int)(movePoints * (direction.Y / 100f));
            this.lifeSpan -= movePoints;
        }

        public void HitUnit(GenericUnit unit)
        {
            this.IsDead = true;
        }

        public void Render(Graphics g)
        {
            Point position = game.MapPositionToScreenPosition(location);
            g.DrawImageAt(image, position);
        }
    }
}
