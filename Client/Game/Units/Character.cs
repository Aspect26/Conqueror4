using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class PlayerCharacter : SimpleUnit
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Spec { get; set; }

        public PlayerCharacter(string name, int level, int spec) 
            : base(GameData.GetCharacterBasePath(spec), new Location())
        {
            this.Name = name;
            this.Level = level;
            this.Spec = spec;
        }

        // RENDERING
        private int playerSize = 50;
        public override void DrawUnit(Graphics g)
        {
            g.DrawImage(GetCurrentImage(), Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2,
                playerSize, playerSize);

            g.DrawString(Name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2 - 10);
        }
    }
}
