using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents a component in the character creation screen that contains buttons for all playable character
    /// specializations.
    /// </summary>
    /// <seealso cref="Client.RectangleComponent" />
    /// <seealso cref="Client.CreateCharacterButton" />
    public class CreateCharacterSelection : RectangleComponent
    {
        private CreateCharacterButton[] characterButtons;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterSelection"/> class.
        /// </summary>
        public CreateCharacterSelection()
            : base(new Point(0, 0), new Rectangle(Application.WIDTH / 2 - 3 * 64, 450, 64 * 6, 64), Color.Black)
        {
            // character buttons
            this.characterButtons = new CreateCharacterButton[6];
            for (int i = 0; i < 6; i++)
            {
                this.characterButtons[i] = new CreateCharacterButton(new Point(0, 0),
                    new Rectangle(this.X + (i*64), this.Y, 64, 64), i + 1);
            }
            this.characterButtons[0].SetFocused(true);
        }

        /// <summary>
        /// Renders the component on the graphics object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            foreach(CreateCharacterButton btn in characterButtons)
            {
                btn.Render(g);
            }
        }

        /// <summary>
        /// Called on [mouse left down] event.
        /// Sets a button which was cliekced to be selected.
        /// </summary>
        /// <param name="position">The position.</param>
        public override void OnMouseLeftDown(Point position)
        {
            foreach(CreateCharacterButton btn in characterButtons)
            {
                btn.SetFocused(btn.IsAt(position));
            }
        }

        /// <summary>
        /// Gets the selected specialization.
        /// </summary>
        /// <returns>The specialization.</returns>
        public int GetSelectedSpec()
        {
            foreach (CreateCharacterButton btn in characterButtons)
            {
                if (btn.IsFocused())
                {
                    return btn.GetSpec();
                }
            }

            return -1;
        }
    }
}
