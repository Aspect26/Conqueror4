using System.Drawing;
using Shared;

namespace Client
{
    public class SimpleUnit : IUnit
    {
        // IDs
        public int UnitID { get; protected set; }
        public int UniqueID { get; protected set; }

        // STATUS
        public int HitPoints { get; protected set; }
        public int MaxHitPoints { get; protected set; }
        public int ManaPoints { get; protected set; }
        public int MaxManaPoints { get; protected set; }

        public int UnitSize { get; private set; }
        protected Image unitImage;
        public Location Location { get; set; }
        private Game game;

        public SimpleUnit(Game game, int unitID, int uniqueId, Location location)
        {
            this.Location = location;
            this.UnitSize = 50;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.game = game;
            this.unitImage = GameData.GetUnitImage(unitID);
        }

        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
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
            g.DrawImageAt(unitImage, game.MapPositionToScreenPosition(Location.X, Location.Y), 50, 50);
        }

        public Image GetCurrentImage()
        {
            return this.unitImage;
        }
    }
}
