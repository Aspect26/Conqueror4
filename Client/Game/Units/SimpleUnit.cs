using System.Drawing;
using Shared;

namespace Client
{
    public class SimpleUnit : IUnit
    {
        public int UnitSize { get; private set; }

        protected UnitAnimation animation;
        public Location Location { get; set; }

        public MovingDirection Direction { get; private set; }
        protected int movingSpeed;
        protected bool moved;

        protected const int SLOWING_CONSTANT = 5;

        public SimpleUnit(Game game, string baseImagePath, Location location)
        {
            this.Location = location;
            this.UnitSize = 50;
            this.moved = false;

            animation = new UnitAnimation(game, this, baseImagePath);

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public void SetLocation(int x, int y)
        {
            this.Location.X = x;
            this.Location.Y = y;
        }

        public SimpleUnit(Location location)
        {
            this.Location = location;
            this.UnitSize = 50;

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public virtual void PlayCycle(int timeSpan)
        {
            animation.AnimateCycle(timeSpan);
            int movePoints = timeSpan / SLOWING_CONSTANT;

            if (Direction != MovingDirection.None)
                moved = true;

            switch (Direction)
            {
                case MovingDirection.None:
                    break;

                case MovingDirection.Up:
                    Location.Y -= movePoints;
                    break;

                case MovingDirection.UpRight:
                    Location.X += movePoints;
                    Location.Y -= movePoints;
                    break;

                case MovingDirection.Right:
                    Location.X += movePoints;
                    break;

                case MovingDirection.BottomRight:
                    Location.X += movePoints;
                    Location.Y += movePoints;
                    break;

                case MovingDirection.Bottom:
                    Location.Y += movePoints;
                    break;

                case MovingDirection.BottomLeft:
                    Location.X -= movePoints;
                    Location.Y += movePoints;
                    break;

                case MovingDirection.Left:
                    Location.X -= movePoints;
                    break;

                case MovingDirection.UpLeft:
                    Location.X -= movePoints;
                    Location.Y -= movePoints;
                    break;
            }
        }

        public virtual void DrawUnit(Graphics g)
        {
            animation.Render(g);
        }

        public Image GetCurrentImage()
        {
            return animation.GetCurrentImage();
        }

        public virtual void StartMovingUp()
        {
            switch (Direction)
            {
                case MovingDirection.Right:
                    Direction = MovingDirection.UpRight; break;
                case MovingDirection.Left:
                    Direction = MovingDirection.UpLeft; break;

                default:
                    Direction = MovingDirection.Up; break;
            }
        }

        public virtual void StartMovingRight()
        {
            switch (Direction)
            {
                case MovingDirection.Up:
                    Direction = MovingDirection.UpRight; break;
                case MovingDirection.Bottom:
                    Direction = MovingDirection.BottomRight; break;

                default:
                    Direction = MovingDirection.Right; break;
            }
        }

        public virtual void StartMovingBottom()
        {
            switch (Direction)
            {
                case MovingDirection.Right:
                    Direction = MovingDirection.BottomRight; break;
                case MovingDirection.Left:
                    Direction = MovingDirection.BottomLeft; break;

                default:
                    Direction = MovingDirection.Bottom; break;
            }
        }

        public virtual void StartMovingLeft()
        {
            switch (Direction)
            {
                case MovingDirection.Up:
                    Direction = MovingDirection.UpLeft; break;
                case MovingDirection.Bottom:
                    Direction = MovingDirection.BottomLeft; break;

                default:
                    Direction = MovingDirection.Left; break;
            }
        }

        public virtual void StopMovingUp()
        {
            switch (Direction)
            {
                case MovingDirection.UpLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.UpRight:
                    Direction = MovingDirection.Right; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public virtual void StopMovingRight()
        {
            switch (Direction)
            {
                case MovingDirection.BottomRight:
                    Direction = MovingDirection.Bottom; break;
                case MovingDirection.UpRight:
                    Direction = MovingDirection.Up; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public virtual void StopMovingBottom()
        {
            switch (Direction)
            {
                case MovingDirection.BottomLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.BottomRight:
                    Direction = MovingDirection.Right; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public virtual void StopMovingLeft()
        {
            switch (Direction)
            {
                case MovingDirection.UpLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.BottomLeft:
                    Direction = MovingDirection.Bottom; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }
    }
}
