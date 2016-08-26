namespace Server
{
    /// <summary>
    /// A basic information about a game object. This is loaded from the SQL 
    /// database and later an actual game object is created based on the informations
    /// from this instance.
    /// </summary>
    public class ObjectInfo
    {
        public ObjectInfo(char mark, int X, int Y, string[] specialArguments)
        {
            this.Mark = mark;
            this.X = X;
            this.Y = Y;
            this.SpecialArguments = specialArguments;
        }

        /// <summary>
        /// Gets or sets the mark. The mark specifies which type of the game 
        /// object this is (e.g.: 'P' is for portal, currently there are no other objects,
        /// items shall be later integrated with game objects)
        /// </summary>
        /// <value>The mark.</value>
        public char Mark { get; set; }

        /// <summary>
        /// Gets or sets the X coordination of the object's location.
        /// </summary>
        /// <value>The x coordinate.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordination of the object's location.
        /// </summary>
        /// <value>The y coordinate.</value>
        public int Y { get; set; }

        /// <summary>
        /// Specific game objects can have additional specific information (e.g.:
        /// the portal have information about the map where they are teleporting 
        /// units). 
        /// </summary>
        /// <value>The special properties.</value>
        public string[] SpecialArguments { get; set; }
    }
}
