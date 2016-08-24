using System.Drawing;
using Shared;

namespace Client
{
    /// <summary>
    /// Represents the current map the player is located on. It is mostly just a 2D
    /// grid of tiles.
    /// </summary>
    public class Map
    {
        private Tile[][] tiles;

        /// <summary>
        /// Sets the visibility of the player so the map doesn't have to be rendered 
        /// whole every time, only those tiles which are close enough.
        /// </summary>
        private const int VISIBILITY = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map() { }

        /// <summary>
        /// Creates the map from specified file. The map is loaded from a bitmap image 
        /// where every pixel represents one tile and the pixel's color specifies which
        /// type of tile it is.
        /// </summary>
        /// <param name="mapPath">The map path.</param>
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

        /// <summary>
        /// Renders the map
        /// </summary>
        /// <param name="graphics">The graphics object.</param>
        /// <param name="playerLocation">The player's location.</param>
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
                                Tile.TILE_SIZE + 1, Tile.TILE_SIZE + 1);

                            // TODO: blend adjacent tiles
                            //BlendTile(graphics, x, y, drawPoint);
                        }
                    }
                }
            }
        }
    }
}
