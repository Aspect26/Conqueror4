using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the screen that is shown when user logs in. It contains list of characters that user have created on
    /// the account logged in.
    /// </summary>
    /// <seealso cref="Client.EmptyScreen" />
    public class CharactersScreen : EmptyScreen
    {
        private CharactersPanel charactersPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharactersScreen"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="server">The server connection.</param>
        public CharactersScreen(Application application, ServerConnection server): base(application, server, GameData.GetCharactersBackground())
        {
            int result = application.server.GetCharacters(application.Account);
            if (result != ServerConnection.RESULT_OK)
                userInterface.ShowMessageBox("Error in communication with the server.");

            initCharactersPanel();
            initEnterWorldButton();
        }

        private void initCharactersPanel()
        {
            LineText usernameText = new LineText(new Point(0, 0), application.Account.Username + ":", Color.Yellow,
                new Point(10, 76), 14);

            charactersPanel = new CharactersPanel(new Point(0, 0), new Rectangle(10, 100, 256, Application.HEIGHT - 105));

            int currentY = 4;
            int btn_height = 50;
            foreach (Character character in application.Account.GetCharacters())
            {
                CharacterButton btn = new CharacterButton(new Point(0, 0),
                    new Rectangle(0, currentY, charactersPanel.GetClientArea().Width, btn_height), character);
                btn.CharacterClick += OnCharacterClicked;

                charactersPanel.AddComponent(btn);
                currentY += btn_height;
            }

            Button createCharacterButton = new Button(new Point(0, 0), "NEW CHARACTER",
                new Rectangle(0, charactersPanel.HEIGHT - btn_height,
                    charactersPanel.GetClientArea().Width, btn_height));
            createCharacterButton.OnClick += OnNewCharacterClicked;
            charactersPanel.AddComponent(createCharacterButton);

            userInterface.AddComponent(usernameText);
            userInterface.AddComponent(charactersPanel);
        }

        private void initEnterWorldButton()
        {
            int btn_width = 100;
            int btn_height = 30;
            Button enterButton = new Button(new Point(0, 0), "ENTER WORLD",
                new Rectangle(((Application.WIDTH - charactersPanel.WIDTH - 10) / 2 - btn_width / 2) + charactersPanel.WIDTH + 10,
                Application.HEIGHT - 10 - btn_height, btn_width, btn_height));
            enterButton.OnClick += OnEnterWorldClicked;

            userInterface.AddComponent(enterButton);
        }

        /// <summary>
        /// Renders the user interface, background and the image of selected character.
        /// </summary>
        /// <param name="g">Graphics object.</param>
        public override void Render(Graphics g)
        {
            base.Render(g);

            Character chr = charactersPanel.GetSelectedCharacter();

            if (chr == null)
                return;

            Image img = GameData.GetUnitImage(chr.Spec);
            g.DrawImage(img, Application.WIDTH / 2, charactersPanel.Y, 
                Application.WIDTH / 2 - 50, charactersPanel.HEIGHT - 50);
        }

        /// <summary>
        /// Handles the <see cref="E:EnterWorldClicked" /> event.
        /// </summary>
        /// <param name="button">The button clicked.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnEnterWorldClicked(Button button, EventArgs e)
        {
            Character selected = charactersPanel.GetSelectedCharacter();
            if (selected != null)
                enterWorld(selected);
        }

        private void OnCharacterClicked(Button button, Character character)
        {
            enterWorld(character);
        }

        private void OnNewCharacterClicked(Button b, EventArgs e)
        {
            application.ChangeWindow(new CreateCharacterScreen(application, server));
        }

        /// <summary>
        /// Sets the characters playing character and changes window to PlayScreen
        /// </summary>
        /// <param name="character">The character.</param>
        private void enterWorld(Character character)
        {
            application.Account.SetPlayedCharacter(character.Name, server);
            application.ChangeWindow(new PlayScreen(application, server));
        }
    }
}
