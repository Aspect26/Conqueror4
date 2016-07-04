using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Client
{
    public class Tile
    {
        public static readonly int TILE_SIZE = 50;
        private static Pen noImagePen = Pens.Black;
        private int id;

        public Tile(int id)
        {
            this.id = id;
        }

        public virtual void Render(Graphics graphics, int X, int Y)
        {
            graphics.DrawImage(Game.GetTile(id), X, Y, TILE_SIZE, TILE_SIZE);
        }
    }
}
