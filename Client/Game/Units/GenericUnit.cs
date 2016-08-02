using System.Drawing;
using Shared;

namespace Client
{
    public class GenericUnit : IUnit
    {
        // IDs
        public int UnitID { get; protected set; }
        public int UniqueID { get; protected set; }

        // STATUS
        public BaseStats MaxStats { get; protected set; }
        public BaseStats ActualStats { get; protected set; }
        public int ManaPoints { get; protected set; }
        public int MaxManaPoints { get; protected set; }
        public bool IsDead { get; protected set; }

        // OTHER STATS
        public int HitRange { get { return 30; } }

        public int UnitSize { get; private set; }
        protected Image unitImage;
        public Location Location { get; set; }
        private Game game;

        public GenericUnit(Game game, int unitID, int uniqueId, Location location, BaseStats maxStats, 
            BaseStats actualStats)
        {
            this.Location = location;
            this.UnitSize = 50;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.game = game;
            this.MaxStats = maxStats;
            this.ActualStats = actualStats;
            this.IsDead = false;
            this.unitImage = GameData.GetUnitImage(unitID);
        }

        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
        }

        public void Kill()
        {
            this.IsDead = true;
        }

        public void SetLocation(int x, int y)
        {
            this.Location.X = x;
            this.Location.Y = y;
        }

        public virtual void PlayCycle(int timeSpan)
        {
        }

        public virtual void DrawUnit(Graphics g)
        {
            // unit
            Point mapLocation = game.MapPositionToScreenPosition(Location.X, Location.Y);
            g.DrawImageAt(unitImage, mapLocation, UnitSize, UnitSize);

            // HP bar
            Point barLocation = new Point(mapLocation.X - UnitSize/2, mapLocation.Y - UnitSize/2);
            float hpRatio = (float)ActualStats.HitPoints / MaxStats.HitPoints;
            Brush hpBrush = (hpRatio < 0.25f) ? Brushes.Red : Brushes.LightGreen;

            g.FillRectangle(Brushes.DarkGreen, barLocation.X, barLocation.Y, UnitSize, 5);
            g.FillRectangle(hpBrush, barLocation.X, barLocation.Y, UnitSize * hpRatio, 5);
        }

        public Image GetCurrentImage()
        {
            return this.unitImage;
        }

        public void TryHitByMissile(Missile missile)
        {
            if (missile.Source == this)
                return;

            Point missilePoint = missile.GetLocation();
            Point myPoint = new Point(this.Location.X, this.Location.Y);
            int distance = myPoint.DistanceFrom(missilePoint);

            if (distance <= HitRange)
            {
                missile.HitUnit(this);
            }
        }
    }
}
