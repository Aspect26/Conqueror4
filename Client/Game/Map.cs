using System;
using System.Drawing;
using System.IO;
using Shared;

namespace Client
{
    public class Map
    {
        // X x Y
        private Tile[][] tiles;
        private const int VISIBILITY = 16;

        public Map() { }

        public void Create(string mapPath)
        {
            Bitmap bitMap = new Bitmap(mapPath);

            int height = bitMap.Height;
            int width = bitMap.Width;
            tiles = new Tile[width][];

            for (int y = 0; y<height; y++)
            {
                for (int x = 0; x<width; x++)
                {
                    if (y == 0)
                        tiles[x] = new Tile[height];

                    tiles[x][y] = new Tile(GameData.GetTileId(bitMap.GetPixel(x, y)));
                }
            }
        }

        public void Render(Graphics graphics, Location playerLocation)
        {
            // "move" map accordingly to playe's location
            int xPlayerOffset = (int)playerLocation.X;
            int yPlayerOffset = (int)playerLocation.Y;

            // "move" map so it's relative to middle of the screen, not topleft corner
            int xScreenOffset = Application.WIDTH / 2;
            int yScreenOffset = Application.HEIGHT / 2;

            for(int y = yPlayerOffset/Tile.TILE_SIZE - VISIBILITY/2; y < yPlayerOffset/Tile.TILE_SIZE + VISIBILITY / 2; y++)
            {
                if (y >= 0 && y < tiles.Length)
                {
                    for (int x = xPlayerOffset / Tile.TILE_SIZE - VISIBILITY / 2; x < xPlayerOffset / Tile.TILE_SIZE + VISIBILITY / 2; x++)
                    {
                        if (x >= 0 && x < tiles[y].Length)
                        {
                            Point drawPoint = new Point(x * Tile.TILE_SIZE - xPlayerOffset + xScreenOffset,
                                y * Tile.TILE_SIZE - yPlayerOffset + yScreenOffset);

                            graphics.DrawImage(GameData.GetTile(tiles[x][y].Id),
                                drawPoint.X, drawPoint.Y,
                                Tile.TILE_SIZE, Tile.TILE_SIZE);

                            //BlendTile(graphics, x, y, drawPoint);
                        }
                    }
                }
            }
        }
    }
}
