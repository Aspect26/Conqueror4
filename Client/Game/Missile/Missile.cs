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
        public bool IsDead { get; private set; }

        private const int IMPLIICT_LIFE_SPAN = 333;

        public Missile(Game game, Image image, Point location, Point direction, int lifeSpan = IMPLIICT_LIFE_SPAN)
        {
            this.game = game;
            this.image = image;
            this.location = location;
            this.lifeSpan = lifeSpan;
            this.direction = direction;
            this.IsDead = false;
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

        public void Render(Graphics g)
        {
            Point position = game.MapPositionToScreenPosition(location);
            g.DrawImageAt(image, position);
        }
    }
}
