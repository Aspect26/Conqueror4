using Shared;
using System.Drawing;

namespace Client
{
    public class PlayerUnit : GenericUnit
    {
        private string name;
        private Game game;

        public PlayerUnit(Game game, string name, int unitId, int uniqueId, int x, int y, BaseStats maxStats,
            BaseStats actualStats, int fraction)
            : base(game, unitId, uniqueId, new Location(x, y), maxStats, actualStats, fraction)
        {
            this.name = name;
            this.game = game;
            this.nameBrush = Brushes.Blue;
        }

        public override bool Isplayer()
        {
            return true;
        }
    }
}
