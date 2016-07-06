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

        public PlayerUnit(string name, int id, int x, int y)
            :base(GameData.GetCharacterBasePath(id), new Location())
        {
            this.name = name;
            this.Location.X = x;
            this.Location.Y = y;
        }
        public override void DrawUnit(Graphics g)
        {
            animation.Render(g);

            g.DrawString(name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - UnitSize / 2, Application.HEIGHT / 2 - UnitSize / 2 - 10);
        }
    }
}
