using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlayerUnit : SimpleUnit
    {
        private string name;
        private Game game;

        public PlayerUnit(Game game, string name, int id, int x, int y)
            : base(game, GameData.GetCharacterBasePath(id), new Location(x, y))
        {
            this.name = name;
            this.game = game;
        }

        public override void DrawUnit(Graphics g)
        {
            base.DrawUnit(g);

            Point p = game.MapPositionToScreenPosition(this.Location.X, this.Location.Y);
            g.DrawString(name, GameData.GetFont(8), Brushes.Black, p.X - UnitSize/2, p.Y - UnitSize/2 - 20);
        }
    }
}
