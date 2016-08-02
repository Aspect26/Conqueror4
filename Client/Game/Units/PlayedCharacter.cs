using System.Drawing;
using Shared;
using System;

namespace Client
{
    public sealed class PlayedCharacter : GenericUnit
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Spec { get { return UnitID; }  }
        private ServerConnection server;

        // TODO !!!!!! remove this -> mock data
        public int ManaPoints { get { return 25; } }
        public int MaxManaPoints { get { return 100; } }
        public int Experience { get; set; }

        // moving
        private bool movingUp = false;
        private bool movingRight = false;
        private bool movingBottom = false;
        private bool movingLeft = false;
        public MovingDirection MovingDirection { get; private set; }

        private bool moved;
        private const int SLOWING_CONSTANT = 4;
        private UnitAnimation animation;

        public PlayedCharacter(ServerConnection server, string name, int level, int spec, int uniqueId, BaseStats maxStats,
            BaseStats actualStats) 
            : base(null, spec, uniqueId, new Location(), maxStats, actualStats)
        {
            this.Name = name;
            this.Level = level;
            this.server = server;
            this.animation = new CentreUnitAnimation(this, GameData.GetCharacterBasePath(spec));
            this.moved = false;
            this.MovingDirection = MovingDirection.None;
        }

        public void SetMaxStats(BaseStats maxStats)
        {
            this.MaxStats = maxStats;
        }

        public void SetActualStats(BaseStats actualStats)
        {
            this.ActualStats = actualStats;
        }

        // RENDERING
        private int playerSize = 50;
        public override void DrawUnit(Graphics g)
        {
            animation.Render(g);
            g.DrawString(Name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2 - 20);
        }

        public override void PlayCycle(int timeSpan)
        {
            base.PlayCycle(timeSpan);

            animation.AnimateCycle(timeSpan);
            int movePoints = timeSpan / SLOWING_CONSTANT;

            bool movingHorizontally = (movingLeft && !movingRight) || (!movingLeft && movingRight);
            bool movingVertically = (movingUp && !movingBottom) || (!movingUp && movingBottom);

            if(movingHorizontally && movingVertically)
            {
                movePoints = (int)Math.Sqrt( (movePoints * movePoints)/2d );
                moved = true;

                if (movingUp)
                {
                    Location.Y -= movePoints;
                    MovingDirection = MovingDirection.Up;
                }
                else
                {
                    Location.Y += movePoints;
                    MovingDirection = MovingDirection.Bottom;
                }

                if (movingLeft)
                {
                    Location.X -= movePoints;
                    MovingDirection = MovingDirection | MovingDirection.Left;
                }
                else
                {
                    Location.X += movePoints;
                    MovingDirection = MovingDirection | MovingDirection.Right;
                }
            }
            else if (movingHorizontally)
            {
                moved = true;
                if (movingLeft)
                {
                    Location.X -= movePoints;
                    MovingDirection = MovingDirection.Left;
                }
                else
                {
                    Location.X += movePoints;
                    MovingDirection = MovingDirection.Right;
                }
            }
            else if (movingVertically)
            {
                moved = true;
                if (movingUp)
                {
                    Location.Y -= movePoints;
                    MovingDirection = MovingDirection.Up;
                }
                else
                {
                    Location.Y += movePoints;
                    MovingDirection = MovingDirection.Bottom;
                }
            }
            else
            {
                moved = false;
                MovingDirection = MovingDirection.None;
            }

            if (moved)
            {
                server.SendPlayerLocation(this);
                moved = false;
            }
        }

        // MOVING COMMANDS
        // Start
        public void StartMovingUp()
        {
            movingUp = true;
        }

        public void StartMovingRight()
        {
            movingRight = true;
        }

        public void StartMovingBottom()
        {
            movingBottom = true;
        }

        public void StartMovingLeft()
        {
            movingLeft = true;
        }

        // stop
        public void StopMovingUp()
        {
            movingUp = false;
        }

        public void StopMovingRight()
        {
            movingRight = false;
        }

        public void StopMovingBottom()
        {
            movingBottom = false;
        }

        public void StopMovingLeft()
        {
            movingLeft = false;
        }
    }
}
