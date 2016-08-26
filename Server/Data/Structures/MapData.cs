using System.Drawing;

namespace Server
{
    /// <summary>
    /// A simple class containing only a basic info about a one map.
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the table in which additional data to this
        /// map are stored.
        /// </summary>
        /// <value>The name of the data table.</value>
        public string DataTableName { get; set; }

        /// <summary>
        /// Gets or sets the revive location. This is a little unfinished becasue in
        /// shared maps the character's of both fractions will be revived at the same
        /// location.
        /// </summary>
        /// <value>The revive location.</value>
        public Point ReviveLocation { get; set; }
    }
}
