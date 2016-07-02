using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Client
{
    public class Player
    {
        public int CurrentLocation { get; set; }
        public Point Location { get; set; }
        public int Character { get; set; }
        public Image PlayerImage { get; set; }

        public bool Logging { get; set; }
        public bool Logged { get; set; }
        public bool Loaded { get; set; }

        public Player()
        {
            Logged = false;
            Loaded = false;
            Logging = true;
        }

        public void Render(Graphics g)
        {
            g.DrawImage(PlayerImage, Game.WIDTH / 2, Game.HEIGHT / 2, Tile.TILE_SIZE, Tile.TILE_SIZE);
        }
    }
}
