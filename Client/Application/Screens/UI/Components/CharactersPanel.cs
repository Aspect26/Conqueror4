using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a characters panel in the CharactersScreen.
    /// </summary>
    /// <seealso cref="Client.Panel" />
    /// <seealso cref="Client.CharactersScreen"/>
    public class CharactersPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharactersPanel"/> class.
        /// </summary>
        /// <param name="offsetPosition">The offset position.</param>
        /// <param name="position">The position.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="neighbour">The neighbour.</param>
        public CharactersPanel(Point offsetPosition, Rectangle position, int borderSize = UserInterface.DEFAULT_BORDER_HEIGHT,
            IComponent neighbour = null)
        :base(offsetPosition, position, borderSize, neighbour)
        {

        }

        /// <summary>
        /// Gets the selected character.
        /// </summary>
        /// <returns>The character.</returns>
        public Character GetSelectedCharacter()
        {
            if (!(container.GetFocusedComponent() is CharacterButton))
                return null;

            CharacterButton btn = (CharacterButton)container.GetFocusedComponent();
            if (btn == null)
                return null;

            return btn.GetCharacter();
        }
    }
}
