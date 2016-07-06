using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Client
{
    public class Map
    {
        // X x Y
        private Tile[][] tiles;

        private const int VISIBILITY = 16;

        readonly float[][] matrixItems = {
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1, 0, 0, 0},
            new float[] {0, 0, 1, 0, 0},
            new float[] {0, 0, 0, 0.3f, 0},
            new float[] {0, 0, 0, 0, 1}};
        ColorMatrix blendMatrix;
        ImageAttributes blendAttribute = new ImageAttributes();


        public Map()
        {
            blendMatrix = new ColorMatrix(matrixItems);
            blendAttribute = new ImageAttributes();
            blendAttribute.SetColorMatrix(blendMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

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

                            graphics.DrawImage(GameData.GetTile(tiles[y][x].Id),
                                drawPoint.X, drawPoint.Y,
                                Tile.TILE_SIZE, Tile.TILE_SIZE);

                            //BlendTile(graphics, x, y, drawPoint);
                        }
                    }
                }
            }
        }

        private void BlendTile(Graphics g, int x, int y, Point position)
        {
            BlendLeft(g, x, y, position);
        }

        private void BlendLeft(Graphics g, int x, int y, Point position)
        {
            if (y - 1 >= 0 && y - 1 < tiles.Length)
            {
                if (tiles[y][x].Id != tiles[y - 1][x].Id)
                {
                    g.DrawImage(
                        GameData.GetTile(tiles[y][x].Id),
                        new Rectangle(position.X - Tile.TILE_SIZE, position.Y, Tile.TILE_SIZE, Tile.TILE_SIZE), 
                        0.0f, 
                        0.0f, 
                        Tile.TILE_SIZE, 
                        Tile.TILE_SIZE,
                        GraphicsUnit.Pixel, 
                        blendAttribute);
                }
            }
        }

        
    }
}
