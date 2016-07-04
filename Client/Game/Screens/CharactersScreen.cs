using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class CharactersScreen : EmptyScreen
    {
        private Panel charactersPanel;

        public CharactersScreen(Game game): base(game, Game.GetCharactersBackground())
        {
            int result = game.server.GetCharacters(game.Account);
            if (result != ServerConnection.RESULT_OK)
                userInterface.MessageBoxShow("Error in communication with the server.");

            charactersPanel = new Panel(new Point(0, 0), new Rectangle(10, 100, 256, Game.HEIGHT - 105));

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

            userInterface.AddComponent(charactersPanel);
        }

        private void OnCharacterClicked(Button button, Character character)
        {
            userInterface.MessageBoxShow("Unfinished section!");
        }

        private void OnNewCharacterClicked(Button b, EventArgs e)
        {
            userInterface.MessageBoxShow("Unfinished section!");
        }
    }
}
