using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents one tile in a map object.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The tile size
        /// </summary>
        public static readonly int TILE_SIZE = 50;
        private static Pen noImagePen = Pens.Black;

        /// <summary>
        /// Gets or sets the tile identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Tile(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Renders the tile on the specified location on the screen.
        /// </summary>
        /// <param name="graphics">The graphics object.</param>
        /// <param name="X">The x location.</param>
        /// <param name="Y">The y location.</param>
        public virtual void Render(Graphics graphics, int X, int Y)
        {
            graphics.DrawImage(GameData.GetTile(Id), X, Y, TILE_SIZE, TILE_SIZE);
        }
    }
}
