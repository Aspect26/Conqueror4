using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class CharactersScreen : EmptyScreen
    {
        private CharactersPanel charactersPanel;

        public CharactersScreen(Application game, ServerConnection server): base(game, server, GameData.GetCharactersBackground())
        {
            int result = game.server.GetCharacters(game.Account);
            if (result != ServerConnection.RESULT_OK)
                userInterface.MessageBoxShow("Error in communication with the server.");

            // CHARACTERS PANEL
            LineText usernameText = new LineText(new Point(0, 0), game.Account.Username + ":", Color.Yellow,
                new Point(10, 76), 14);

            charactersPanel = new CharactersPanel(new Point(0, 0), new Rectangle(10, 100, 256, Application.HEIGHT - 105));

            int currentY = 4;
            int btn_height = 50;
            foreach(Character character in game.Account.GetCharacters())
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
            createCharacterButton.Click += OnNewCharacterClicked;
            charactersPanel.AddComponent(createCharacterButton);

            // ENTER BUTTON
            int btn_width = 100;
            btn_height = 30;
            Button enterButton = new Button(new Point(0, 0), "ENTER WORLD",
                new Rectangle(((Application.WIDTH - charactersPanel.WIDTH-10) / 2 - btn_width / 2) + charactersPanel.WIDTH+10, 
                Application.HEIGHT - 10 - btn_height, btn_width, btn_height));
            enterButton.Click += OnEnterWorldClicked;

            // APPLY ALL
            userInterface.AddComponent(usernameText);
            userInterface.AddComponent(charactersPanel);
            userInterface.AddComponent(enterButton);
        }

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
            userInterface.MessageBoxShow("Unfinished section!");
        }

        private void enterWorld(Character character)
        {
            application.Account.SetPlayedCharacter(character.Name, server);
            application.ChangeWindow(new PlayScreen(application, server));
        }
    }
}
