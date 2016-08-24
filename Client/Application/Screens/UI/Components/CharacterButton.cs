using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a button of a specific character in the CharactersScreen. This button contains image of the character and
    /// some basic information about it.
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Client.Button" />
    /// <seealso cref="Client.CharactersScreen"/>
    public sealed class CharacterButton : Button
    {
        /// <summary>
        /// Delegate OnCharacterClickHandler
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="character">The character.</param>
        public delegate void OnCharacterClickHandler(Button m, Character character);
        /// <summary>
        /// Occurs when [character click].
        /// </summary>
        public event OnCharacterClickHandler CharacterClick;

        private Character character;

        private Font mainFont = GameData.GetFont(10);
        private Font lesserFont = GameData.GetFont(8);
        private Image characterImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterButton"/> class.
        /// </summary>
        /// <param name="parentPosition">The parent position.</param>
        /// <param name="position">The position.</param>
        /// <param name="character">The character.</param>
        public CharacterButton(Point parentPosition, Rectangle position, Character character)
            : base(parentPosition, character.Name + ", " + character.Level + ", " + GameData.GetSpecName(character.Spec), position)
        {
            this.character = character;
            font = GameData.GetFont(position.Height - 20);

            this.backgroundImage = GameData.GetCharacterButtonBackground();
            this.characterImage = GameData.GetUnitImage(character.Spec);
        }

        /// <summary>
        /// Sets the component focused or not focused and sets the corresponding background image.
        /// </summary>
        /// <param name="focused">if set to <c>true</c> then the component becomes focused.</param>
        public override void SetFocused(bool focused)
        {
            this.focused = focused;
            if(this.focused)
                this.backgroundImage = GameData.GetCharacterButtonBackgroundSelected();
            else
                this.backgroundImage = GameData.GetCharacterButtonBackground();
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);
            RenderCharacterImage(g);
        }

        protected override void RenderText(Graphics g)
        {
            int marginLeft = 60;
            int marginTop = 6;
            g.DrawString(character.Name, mainFont, Brushes.Black, position.X + marginLeft, position.Y + marginTop);
            g.DrawString("Level " + character.Level, lesserFont, Brushes.Gray, position.X + marginLeft, position.Y + marginTop + mainFont.Size + 2);
            g.DrawString(GameData.GetSpecName(character.Spec), lesserFont, Brushes.Gray, position.X + marginLeft, position.Y + marginTop + mainFont.Size + 2 + lesserFont.Size + 1);
        }

        private void RenderCharacterImage(Graphics g)
        {
            g.DrawImage(characterImage, position.X + 17, position.Y + 9, 35, 30);
        }

        /// <summary>
        /// Gets the character this button reffers to.
        /// </summary>
        /// <returns>The character.</returns>
        public Character GetCharacter()
        {
            return character;
        }

        // ******************************************
        // EVENTS
        // ******************************************

        /// <summary>
        /// Called on [mouse left up] event.
        /// Does nothing because the button needs to be until a next character button is clicked.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftUp(Point position) { }

        /// <summary>
        /// Called on [mouse left down] event.
        /// Does nothing because the behaviour is handled somewhere else (little bit of hack here).
        /// </summary>
        /// <param name="position">The position.</param>
        /// <seealso cref="Client.CharactersPanel" />
        public override void OnMouseLeftDown(Point position) { }

        /// <summary>
        /// Called on [key up] event.
        /// Acts as if it was clicked if the key is enter.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void OnKeyUp(int key)
        {
            if (key == 13)
            {
                SetFocused(true);
                CharacterClick(this, character);
            }
        }
    }
}
