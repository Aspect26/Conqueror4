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
        public bool IsDead { get; protected set; }

        // OTHER STATS
        public int HitRange { get { return 30; } }
        public int Fraction { get; protected set; }

        protected Image unitImage;
        public Location Location { get; set; }

        private Game game;
        private Font font;
        protected Brush nameBrush = Brushes.Black;

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

        public virtual bool Isplayer()
        {
            return false;
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

        public Image GetCurrentImage()
        {
            return this.unitImage;
        }

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

        public void UpdateActualStats(BaseStats stats)
        {
            this.ActualStats.HitPoints = stats.HitPoints;
            this.ActualStats.ManaPoints = stats.ManaPoints;
            this.ActualStats.Armor = stats.Armor;
            this.ActualStats.Damage = stats.Damage;
            this.ActualStats.SpellBonus = stats.SpellBonus;
        }

        public void UpdateMaxStats(BaseStats stats)
        {
            this.MaxStats.HitPoints = stats.HitPoints;
            this.MaxStats.ManaPoints = stats.ManaPoints;
            this.MaxStats.Armor = stats.Armor;
            this.MaxStats.Damage = stats.Damage;
            this.MaxStats.SpellBonus = stats.SpellBonus;
        }

        public int GetActualHitPoints()
        {
            return this.ActualStats.HitPoints;
        }

        public int GetActualManaPoints()
        {
            return this.ActualStats.ManaPoints;
        }

        public int GetMaxHitPoints()
        {
            return this.MaxStats.HitPoints;
        }

        public int GetMaxManaPoints()
        {
            return this.MaxStats.ManaPoints;
        }
    }
}
