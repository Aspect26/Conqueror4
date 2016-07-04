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
        // X x Y
        private Tile[][] tiles;

        private const int VISIBILITY = 14;

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

        public void Render(Graphics graphics, Location playerLocation)
        {
            // "move" map accordingly to playe's location
            int xPlayerOffset = playerLocation.X;
            int yPlayerOffset = playerLocation.Y;

            // "move" map so it's relative to middle of the screen, not topleft corner
            int xScreenOffset = Game.WIDTH / 2;
            int yScreenOffset = Game.HEIGHT / 2;

            for(int y = yPlayerOffset/Tile.TILE_SIZE - VISIBILITY/2; y < yPlayerOffset/Tile.TILE_SIZE + VISIBILITY / 2; y++)
            {
                if (y >= 0 && y < tiles.Length)
                {
                    for (int x = xPlayerOffset / Tile.TILE_SIZE - VISIBILITY / 2; x < xPlayerOffset / Tile.TILE_SIZE + VISIBILITY / 2; x++)
                    {
                        if (x >= 0 && x <= tiles[y].Length)
                        {
                            tiles[y][x].Render(graphics,
                                x * Tile.TILE_SIZE - xPlayerOffset + xScreenOffset, 
                                y * Tile.TILE_SIZE - yPlayerOffset + yScreenOffset);
                        }
                    }
                }
            }
        }
    }
}
