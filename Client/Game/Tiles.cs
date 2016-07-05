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

        public int Id { get; set; }

        public Tile(int id)
        {
            this.Id = id;
        }

        public virtual void Render(Graphics graphics, int X, int Y)
        {
            graphics.DrawImage(GameData.GetTile(Id), X, Y, TILE_SIZE, TILE_SIZE);
        }
    }
}
