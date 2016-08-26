namespace Server
{
    /// <summary>
    /// A basic information about an NPC (non player controlled) unit. This is loaded 
    /// from the SQL database and later an actual unit is created based on the informations
    /// from this instance.
    /// </summary>
    public class UnitInfo
    {
        public UnitInfo(int ID, int X, int Y)
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the unit's location.
        /// </summary>
        /// <value>The x coordinate.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the unit's location.
        /// </summary>
        /// <value>The y coordinate.</value>
        public int Y { get; set; }
    }
}
