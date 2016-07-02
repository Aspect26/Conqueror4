using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Client
{
    public class Map
    {
        // HEIGHT x WIDTH
        private Tile[][] tiles;

        public void Create(string mapPath)
        {
            using (StreamReader input = new StreamReader(new FileStream(mapPath, FileMode.Open)))
            {
                try
                {
                    string[] meta = input.ReadLine().Split(new char[] { ' ' });
                    int height = Convert.ToInt32(meta[0]);
                    int width = Convert.ToInt32(meta[1]);

                    tiles = new Tile[height][];
                    for (int y = 0; y < height; y++)
                    {
                        tiles[y] = new Tile[width];
                        string[] lineParts = input.ReadLine().Split(' ');

                        for (int x = 0; x < width; x++)
                        {
                            tiles[y][x] = new Tile(Convert.ToInt32(lineParts[x]));
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Missing file: " + e.Message + "!");
                }
            }
        }

        public void Render(Graphics graphics, int xOffset, int yOffset)
        {
            for(int y = 0; y<tiles.Length; y++)
            {
                for(int x = 0; x<tiles[y].Length; x++)
                {
                    tiles[y][x].Render(graphics, x*Tile.TILE_SIZE + xOffset, y*Tile.TILE_SIZE + yOffset);
                }
            }
        }
    }
}
