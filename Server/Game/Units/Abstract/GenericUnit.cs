using Shared;

namespace Server
{
    public class GenericUnit : IUnit
    {
        public Location Location { get; set; }
        public MovingDirection Direction { get; private set; }

        protected int movingSpeed;
        protected const int SLOWING_CONSTANT = 5;
        protected int id;

        public string Name { get; set; }

        public GenericUnit(int id, Location location)
        {
            this.Location = location;
            this.id = id;
            this.Name = "Unknown";

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public GenericUnit(string name, int id, Location location): this(id, location)
        {
            this.Name = name;
        }

        public virtual void PlayCycle(int timeSpan)
        {
            int movePoints = timeSpan / SLOWING_CONSTANT;

            if (Direction == MovingDirection.Right)
                System.Console.WriteLine("TS: " + timeSpan + ", MP: " + movePoints);

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

        public void StartMovingUp()
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

        public void StartMovingRight()
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
                case MovingDirection.Up:
                    Direction = MovingDirection.UpLeft; break;
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
                case MovingDirection.UpLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.UpRight:
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
                case MovingDirection.UpRight:
                    Direction = MovingDirection.Up; break;

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
                case MovingDirection.UpLeft:
                    Direction = MovingDirection.Left; break;
                case MovingDirection.BottomLeft:
                    Direction = MovingDirection.Bottom; break;

                default:
                    Direction = MovingDirection.None; break;
            }
        }

        public int GetId()
        {
            return id;
        }

        public Location GetLocation()
        {
            return Location;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public virtual bool IsPlayer()
        {
            return false;
        }
    }
}
