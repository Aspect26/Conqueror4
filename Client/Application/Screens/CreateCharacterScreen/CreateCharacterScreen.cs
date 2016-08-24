using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the screen that is shown when the user presses create character button in the CharactersScreen
    /// </summary>
    /// <seealso cref="Client.EmptyScreen" />
    /// <seealso cref="Client.CharactersScreen" />
    public class CreateCharacterScreen : EmptyScreen
    {
        private const int CHARACTER_IMAGE_PADDING = 80;

        private CreateCharacterSelection characterSelection;
        private Button createButton;
        private Button cancelButton;
        private LineInput nickname;

        private Font infoFont;
        private Rectangle screenRect = new Rectangle(0, 0, Application.WIDTH, Application.HEIGHT);

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterScreen"/> class.
        /// Initializes the user interface.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="server">The server connection.</param>
        public CreateCharacterScreen(Application application, ServerConnection server)
            : base(application, server, GameData.GetCharactersBackground())
        {
            this.infoFont = GameData.GetFont(11);

            this.characterSelection = new CreateCharacterSelection();
            this.userInterface.AddComponent(characterSelection);

            // create button
            int yPosition = 450;
            int buttonWidth = 70;
            this.createButton = new Button(new Point(0, 0), "Create", new Rectangle(
                Application.WIDTH / 2 - buttonWidth / 2, yPosition + 70, buttonWidth, 40));
            this.createButton.OnClick += OnCreateButtonClick;
            this.userInterface.AddComponent(createButton);

            // cancel button
            this.cancelButton = new Button(new Point(0, 0), "Cancel", new Rectangle(
                Application.WIDTH - buttonWidth - 20, yPosition + 70, buttonWidth, 40));
            this.cancelButton.OnClick += OnCancelButtonClick;
            this.userInterface.AddComponent(cancelButton);

            // nickname
            int xPosition = 150;
            LineText nickText = new LineText(new Point(0, 0), "Nickname:", Color.Yellow,
                new Point(xPosition, yPosition + 115), 15);
            userInterface.AddComponent(nickText);

            this.nickname = new LineInput(new Point(0, 0), new Point(xPosition + 135, yPosition + 115), 
                220, Color.Black);
            userInterface.AddComponent(nickname);
        }

        /// <summary>
        /// Handles the <see cref="E:CreateButtonClick" /> event.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnCreateButtonClick(Button button, System.EventArgs e)
        {
            int spec = characterSelection.GetSelectedSpec();
            createCharacter(spec, nickname.getValue());
        }

        /// <summary>
        /// Handles the <see cref="E:CancelButtonClick" /> event. Changes screen back to CharactersScreen.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <seealso cref="Client.CharactersScreen" />
        private void OnCancelButtonClick(Button button, System.EventArgs e)
        {
            application.ChangeWindow(new CharactersScreen(application, server));
        }

        /// <summary>
        /// Renders the user interface, background and selected character's image and info.
        /// </summary>
        /// <param name="g">Graphics object.</param>
        public override void Render(Graphics g)
        {
            g.DrawImage(background, screenRect);

            // draw selected character + text
            int spec = characterSelection.GetSelectedSpec();
            int width = Application.WIDTH / 2 - 2 * CHARACTER_IMAGE_PADDING;
            int height = Application.HEIGHT / 2;
            int y = CHARACTER_IMAGE_PADDING;
            Rectangle leftPosition = new Rectangle(CHARACTER_IMAGE_PADDING, y, width, height);
            Rectangle rightPosition = new Rectangle(Application.WIDTH - CHARACTER_IMAGE_PADDING - width,
                y, width, height);

            g.DrawImage(GameData.GetUnitImage(spec), leftPosition);
            g.DrawString(GameData.GetSpecName(spec) + "\r\n\r\n" + GameData.GetCharacterInfo(spec),
                infoFont, Brushes.Yellow, rightPosition);

            userInterface.Render(g);
        }

        /// <summary>
        /// Creates the character with specified specialization and name. The name must be at least 4 letters long.
        /// </summary>
        /// <param name="spec">The specialization.</param>
        /// <param name="name">The name.</param>
        private void createCharacter(int spec, string name)
        {
            if(name.Length < 4)
            {
                userInterface.ShowMessageBox("The nickname must have at least 4 letters.");
                return;
            }

            int result = server.CreateCharacter(application.Account.Username, name, spec);
            if (result != ServerConnection.RESULT_OK)
            {
                userInterface.ShowMessageBox("The nickname is already taken.");
            }
            else
            {
                application.ChangeWindow(new CharactersScreen(application, server));
            }
        }
    }
}
