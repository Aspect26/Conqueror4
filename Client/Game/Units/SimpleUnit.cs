using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SimpleUnit : IUnit
    {
        public int UnitSize { get; private set; }

        protected UnitAnimation animation;
        public  Location Location { get; set; }

        public MovingDirection Direction { get; private set; }
        protected int movingSpeed;

        protected const int SLOWING_CONSTANT = 5;

        public SimpleUnit(Game game, string baseImagePath, Location location)
        {
            this.Location = location;
            this.UnitSize = 50;

            animation = new UnitAnimation(game, this, baseImagePath);

            Direction = MovingDirection.None;
            movingSpeed = 1;
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

            switch (Direction)
            {
                case MovingDirection.None:
                    break;

                case MovingDirection.Top:
                    Location.Y -= movePoints;
                    break;

                case MovingDirection.TopRight:
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

                case MovingDirection.TopLeft:
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

        public void StartMovingUp()
        {
            switch (Direction)
            {
                case MovingDirection.Right:
                    Direction = MovingDirection.TopRight; break;
                case MovingDirection.Left:
                    Direction = MovingDirection.TopLeft; break;

                default:
                    Direction = MovingDirection.Top; break;
            }
        }

        public void StartMovingRight()
        {
            switch (Direction)
            {
                case MovingDirection.Top:
                    Direction = MovingDirection.TopRight; break;
                case MovingDirection.Bottom:
                    Direction = MovingDirection.BottomRight; break;

                default:
                    Direction = MovingDirection.Right; break;
            }
        }

        public void StartMovingBottom()
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

        public void StartMovingLeft()
        {
            switch (Direction)
            {
                case MovingDirection.Top:
                    Direction = MovingDirection.TopLeft; break;
                case MovingDirection.Bottom:
                    Direction = MovingDirection.BottomLeft; break;

                default:
                    Direction = MovingDirection.Left; break;
            }
        }

        public void StopMovingUp()
        {
            switch (Direction)
            {
                case MovingDirection.TopLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.TopRight:
                    Direction = MovingDirection.Right; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public void StopMovingRight()
        {
            switch (Direction)
            {
                case MovingDirection.BottomRight:
                    Direction = MovingDirection.Bottom; break;
                case MovingDirection.TopRight:
                    Direction = MovingDirection.Top; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public void StopMovingBottom()
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

        public void StopMovingLeft()
        {
            switch (Direction)
            {
                case MovingDirection.TopLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.BottomLeft:
                    Direction = MovingDirection.Bottom; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }
    }
}
