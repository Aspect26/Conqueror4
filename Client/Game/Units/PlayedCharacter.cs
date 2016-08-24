using System.Drawing;
using Shared;
using System;

namespace Client
{
    /// <summary>
    /// This class represents the actual character played by the player.
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Client.GenericUnit" />
    public sealed class PlayedCharacter : GenericUnit
    {
        /// <summary>
        /// Gets or sets the name.
        /// This should be also in the IUnit interface. The name is now taken from 
        /// GameData.GetUnitName() function (which is wrong!).
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the character's level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets the character's specialization.
        /// </summary>
        /// <value>The specialization.</value>
        public int Spec { get { return UnitID; }  }

        /// <summary>
        /// Gets the character's quest.
        /// </summary>
        /// <value>The quest.</value>
        public IQuest Quest { get; private set; }

        /// <summary>
        /// Gets the character's equip.
        /// </summary>
        /// <value>The equip.</value>
        public Equip Equip { get; private set; }

        private ServerConnection server;

        /// <summary>
        /// Gets or sets the character's experience.
        /// </summary>
        /// <value>The experience.</value>
        public int Experience { get; set; }

        /// <summary>
        /// Gets or sets the experience required for the character to 
        /// increase its level.
        /// </summary>
        /// <value>The required experience.</value>
        public int ExperienceRequired { get; set; }

        // moving
        private bool movingUp = false;
        private bool movingRight = false;
        private bool movingBottom = false;
        private bool movingLeft = false;

        /// <summary>
        /// Gets the character's moving direction.
        /// </summary>
        /// <value>The moving direction.</value>
        public MovingDirection MovingDirection { get; private set; }

        private bool moved;
        private const int SLOWING_CONSTANT = 4;
        private UnitAnimation animation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayedCharacter"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="name">The name.</param>
        /// <param name="level">The level.</param>
        /// <param name="spec">The spec.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="maxStats">The maximum stats.</param>
        /// <param name="actualStats">The actual stats.</param>
        /// <param name="fraction">The fraction.</param>
        public PlayedCharacter(ServerConnection server, string name, int level, int spec, int uniqueId, BaseStats maxStats,
            BaseStats actualStats, int fraction)
            : base(null, spec, uniqueId, new Location(), maxStats, actualStats, fraction)
        {
            this.Name = name;
            this.Level = level;
            this.server = server;
            this.animation = new CentreUnitAnimation(this, GameData.GetCharacterBasePath(spec));
            this.moved = false;
            this.MovingDirection = MovingDirection.None;
            this.Equip = new Equip();
            this.ExperienceRequired = 42;
        }

        /// <summary>
        /// Delegate ChangeMap
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public delegate void ChangeMap(int mapId);
        /// <summary>
        /// Occurs when the character enters another map then the one in which it he
        /// currently is.
        /// </summary>
        public event ChangeMap MapChanged;

        /// <summary>
        /// Changes the character's location.
        /// </summary>
        /// <param name="l">The l.</param>
        public void ChangeLocation(Location l)
        {
            int oldMapId = this.Location.MapID;

            this.Location.X = l.X;
            this.Location.Y = l.Y;
            this.Location.MapID = l.MapID;

            if (oldMapId != this.Location.MapID)
                MapChanged(this.Location.MapID);
        }

        /// <summary>
        /// Determines whether this unit is a player.
        /// It always returns true becaus the obvious reasons.
        /// </summary>
        /// <returns><c>true</c> if this instance is a player; otherwise, <c>false</c>.</returns>
        public override bool IsPlayer()
        {
            return true;
        }

        /// <summary>
        /// Sets the current quest.
        /// </summary>
        /// <param name="quest">The quest.</param>
        public void SetCurrentQuest(IQuest quest)
        {
            this.Quest = quest;
        }

        /// <summary>
        /// Sets the maximum stats.
        /// </summary>
        /// <param name="maxStats">The maximum stats.</param>
        public void SetMaxStats(BaseStats maxStats)
        {
            this.MaxStats = maxStats;
        }

        /// <summary>
        /// Sets the actual stats.
        /// </summary>
        /// <param name="actualStats">The actual stats.</param>
        public void SetActualStats(BaseStats actualStats)
        {
            this.ActualStats = actualStats;
        }

        /// <summary>
        /// Sets the fraction.
        /// </summary>
        /// <param name="fraction">The fraction.</param>
        public void SetFraction(int fraction)
        {
            this.Fraction = fraction;
        }

        /// <summary>
        /// Sets the equip.
        /// </summary>
        /// <param name="equip">The equip.</param>
        public void SetEquip(Equip equip)
        {
            this.Equip = equip;
        }

        private int playerSize = 50;

        /// <summary>
        /// Draws the unit + the characters name above its head.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void DrawUnit(Graphics g)
        {
            animation.Render(g);
            g.DrawString(Name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2 - 20);
        }

        /// <summary>
        /// Plays one game cycle.
        /// Gets in which direction is the character moving and moves it in that direction.
        /// The distance by which it is moved is counted with regard to the time span between
        /// the last game cycle and now. 
        /// WARNING: THE NEW CHARACTER'S LOCATION IS IMMEDIATELLY SEND TO THE SERVER WHERE
        /// IT IS NOT CHECKED IF IT IS VALID OR POSSIBLE TO MOVE THAT DISTANCE -> THIS
        /// IS POSSIBLY THE MOST VULNERABLE PLACE IN THE SOFTWARE PROJECT.
        /// It is also horribly long........
        /// </summary>
        /// <param name="timeSpan">The time span between last played cycle and now.</param>
        public override void PlayCycle(int timeSpan)
        {
            base.PlayCycle(timeSpan);

            animation.AnimateCycle(timeSpan);
            int movePoints = timeSpan / SharedData.SlowingConstant;

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

        /// <summary>
        /// Sets the corresponding moving flag.
        /// </summary>
        public void StartMovingUp()
        {
            movingUp = true;
        }

        /// <summary>
        /// Sets the corresponding moving flag.
        /// </summary>
        public void StartMovingRight()
        {
            movingRight = true;
        }

        /// <summary>
        /// Sets the corresponding moving flag.
        /// </summary>
        public void StartMovingBottom()
        {
            movingBottom = true;
        }

        /// <summary>
        /// Sets the corresponding moving flag.
        /// </summary>
        public void StartMovingLeft()
        {
            movingLeft = true;
        }

        // stop
        /// <summary>
        /// Unsets the corresponding moving flag.
        /// </summary>
        public void StopMovingUp()
        {
            movingUp = false;
        }

        /// <summary>
        /// Unsets the corresponding moving flag.
        /// </summary>
        public void StopMovingRight()
        {
            movingRight = false;
        }

        /// <summary>
        /// Unsets the corresponding moving flag.
        /// </summary>
        public void StopMovingBottom()
        {
            movingBottom = false;
        }

        /// <summary>
        /// Unsets the corresponding moving flag.
        /// </summary>
        public void StopMovingLeft()
        {
            movingLeft = false;
        }

        /// <summary>
        /// Counts the direction vector and send server a message that the player
        /// wants to shoot. If it succeeds, the server sends a message about new missile
        /// in the game.
        /// </summary>
        /// <param name="location">The location on the screen where player clicked.</param>
        public void TryShoot(Point location)
        {
            int x = location.X - Application.MIDDLE.X;
            int y = location.Y - Application.MIDDLE.Y;
            double length = Math.Sqrt(x * x + y * y);

            int dirX = (int)((x / length) * 100);
            int dirY = (int)((y / length) * 100);
            server.SendPlayerShoot(dirX, dirY);
        }
    }
}
