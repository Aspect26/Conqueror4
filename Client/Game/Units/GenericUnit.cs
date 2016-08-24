using System.Drawing;
using Shared;

namespace Client
{
    /// <summary>
    /// A base class for every unit in the game. The implementation is sufficient enough
    /// for NPC units.
    /// </summary>
    /// <seealso cref="Client.IUnit" />
    public class GenericUnit : IUnit
    {
        /// <summary>
        /// Gets the unit identifier.
        /// This identifier identifies what type of a unit it is (e.g.: 3 is priest,
        /// 7 is  Wolf...)
        /// </summary>
        /// <value>The unit identifier.</value>
        public int UnitID { get; protected set; }

        /// <summary>
        /// Gets the unique identifier.
        /// This identifier is unique for every unit in the game.
        /// Actually it is unique for every unit in a MapInstance (see server side).
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; protected set; }

        /// <summary>
        /// Gets the maximum possible stats of this unit.
        /// </summary>
        /// <value>The maximum stats.</value>
        public BaseStats MaxStats { get; protected set; }

        /// <summary>
        /// Gets the actual stats of the unit.
        /// </summary>
        /// <value>The actual stats.</value>
        public BaseStats ActualStats { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this unit is dead.
        /// Units marked as dead are remove latter in the game cycle.
        /// </summary>
        /// <value><c>true</c> if this unit is dead; otherwise, <c>false</c>.</value>
        public bool IsDead { get; protected set; }

        /// <summary>
        /// Gets the hit range.
        /// </summary>
        /// <value>The hit range.</value>
        public int HitRange { get { return 30; } }

        /// <summary>
        /// Gets the unit's fraction.
        /// </summary>
        /// <value>The fraction.</value>
        public int Fraction { get; protected set; }

        /// <summary>
        /// Gets or sets the unit's location.
        /// </summary>
        /// <value>The unit's location.</value>
        public Location Location { get; set; }

        protected Image unitImage;
        protected Brush nameBrush = Brushes.Black;
        private Game game;
        private Font font;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericUnit"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="unitID">The unit identifier.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="maxStats">The maximum stats.</param>
        /// <param name="actualStats">The actual stats.</param>
        /// <param name="fraction">The fraction.</param>
        public GenericUnit(Game game, int unitID, int uniqueId, Location location, BaseStats maxStats, 
            BaseStats actualStats, int fraction)
        {
            this.Location = location;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.game = game;
            this.MaxStats = maxStats;
            this.ActualStats = actualStats;
            this.Fraction = fraction;
            this.IsDead = false;
            this.unitImage = GameData.GetUnitImage(unitID);
            this.font = GameData.GetFont(8);
        }

        /// <summary>
        /// Determines whether this unit is a player.
        /// It always returns false for a generic unit because a player needs a more powerful
        /// implementation of the IUnit interface.
        /// </summary>
        /// <returns><c>true</c> if this instance is a player; otherwise, <c>false</c>.</returns>
        public virtual bool IsPlayer()
        {
            return false;
        }

        /// <summary>
        /// Sets the unique identifier.
        /// </summary>
        /// <param name="uniqueId">The unique identifier.</param>
        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
        }

        /// <summary>
        /// Kills this unit.
        /// Marks this unit as dead so it is then removed from the game later in the 
        /// game cycle.
        /// </summary>
        public void Kill()
        {
            this.IsDead = true;
        }

        /// <summary>
        /// Sets the unit's location.
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        public void SetLocation(int x, int y)
        {
            this.Location.X = x;
            this.Location.Y = y;
        }

        /// <summary>
        /// Plays one game cycle.
        /// Implicitly it does nothing because there is no hardcore simulation for the unit.
        /// If a unit changes state somehow (e.g.: its location or hitpoints) it is immediatelly
        /// send by a server to client so there is no need to simulate anything. 
        /// </summary>
        /// <param name="timeSpan">The time span between last played cycle and now.</param>
        public virtual void PlayCycle(int timeSpan)
        {
        }

        /// <summary>
        /// Draws the unit.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public virtual void DrawUnit(Graphics g)
        {
            // unit
            Point mapLocation = game.MapPositionToScreenPosition(Location.X, Location.Y);
            g.DrawImageAt(unitImage, mapLocation);

            Point helpPoint = mapLocation; helpPoint.X++;
            g.DrawLine(Pens.Red, mapLocation, helpPoint);

            // HP bar
            Point barLocation = new Point(mapLocation.X - unitImage.Width/2, mapLocation.Y - unitImage.Height / 2 - 11);
            float hpRatio = (float)GetActualHitPoints() / GetMaxHitPoints();
            Brush hpBrush = (hpRatio < 0.25f) ? Brushes.Red : Brushes.LightGreen;

            g.FillRectangle(Brushes.DarkGreen, barLocation.X, barLocation.Y, unitImage.Width, 6);
            g.FillRectangle(hpBrush, barLocation.X, barLocation.Y, unitImage.Width * hpRatio, 6);

            // name
            Point p = new Point(mapLocation.X - (int)g.MeasureString(SharedData.GetUnitName(UnitID), font).Width / 2, 
                mapLocation.Y - unitImage.Height / 2 - 25);
            g.DrawString(SharedData.GetUnitName(UnitID), font, nameBrush, p);
        }

        /// <summary>
        /// Gets the current image of the unit.
        /// </summary>
        /// <returns>The image.</returns>
        public Image GetCurrentImage()
        {
            return this.unitImage;
        }

        /// <summary>
        /// Tries to hit the unit with a missile.
        /// </summary>
        /// <param name="missile">The missile.</param>
        public void TryHitByMissile(Missile missile)
        {
            if (missile.Source.Fraction == this.Fraction)
                return;

            if (missile.IsDead)
                return;

            Point missilePoint = missile.GetLocation();
            Point myPoint = new Point(this.Location.X, this.Location.Y);
            int distance = myPoint.DistanceFrom(missilePoint);

            if (distance <= HitRange)
            {
                missile.HitUnit(this);
            }
        }

        /// <summary>
        /// Updates the actual stats.
        /// </summary>
        /// <param name="stats">The actual stats.</param>
        public void UpdateActualStats(BaseStats stats)
        {
            this.ActualStats.HitPoints = stats.HitPoints;
            this.ActualStats.ManaPoints = stats.ManaPoints;
            this.ActualStats.Armor = stats.Armor;
            this.ActualStats.Damage = stats.Damage;
            this.ActualStats.SpellBonus = stats.SpellBonus;
        }

        /// <summary>
        /// Updates the maximum stats.
        /// </summary>
        /// <param name="stats">The maximum stats.</param>
        public void UpdateMaxStats(BaseStats stats)
        {
            this.MaxStats.HitPoints = stats.HitPoints;
            this.MaxStats.ManaPoints = stats.ManaPoints;
            this.MaxStats.Armor = stats.Armor;
            this.MaxStats.Damage = stats.Damage;
            this.MaxStats.SpellBonus = stats.SpellBonus;
        }

        /// <summary>
        /// Gets the actual hit points of the unit.
        /// </summary>
        /// <returns>The actual hit points.</returns>
        public int GetActualHitPoints()
        {
            return this.ActualStats.HitPoints;
        }

        /// <summary>
        /// Gets the actual mana points of the unit.
        /// </summary>
        /// <returns>The actual mana points.</returns>
        public int GetActualManaPoints()
        {
            return this.ActualStats.ManaPoints;
        }

        /// <summary>
        /// Gets the maximum possible hit points.
        /// The maximal value is already summed on the server side so the client shall not
        /// summ it with the equip bonuses.
        /// </summary>
        /// <returns>The maximal possible hit points with regards to unit's level and equip.</returns>
        public int GetMaxHitPoints()
        {
            return this.MaxStats.HitPoints;
        }

        /// <summary>
        /// Gets the maximum possible mana points.
        /// The maximal value is already summed on the server side so the client shall not
        /// summ it with the equip bonuses.
        /// </summary>
        /// <returns>The maximal possible mana points with regards to unit's level and equip.</returns>
        public int GetMaxManaPoints()
        {
            return this.MaxStats.ManaPoints;
        }
    }
}
