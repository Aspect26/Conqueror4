using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a button in a character creation screen. 
    /// Button of this type represents one playable specialization in the game.
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Client.BorderedRectangleComponent" />
    public sealed class CreateCharacterButton : BorderedRectangleComponent
    {
        private int spec;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterButton"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The nutton's position.</param>
        /// <param name="spec">The specialization it represents.</param>
        public CreateCharacterButton(Point parentPosition, Rectangle position, int spec)
            : base(parentPosition, position, GameData.GetUnitImage(spec))
        {
            this.spec = spec;
        }

        /// <summary>
        /// Gets the specialization.
        /// </summary>
        /// <returns>The specialization.</returns>
        public int GetSpec()
        {
            return spec;
        }
    }
}
