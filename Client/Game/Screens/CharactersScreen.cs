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

            int currentY = 0;
            int btn_height = 50;
            int ofs = UI.DEFAULT_BORDER_HEIGHT;
            foreach(Character character in game.Account.GetCharacters())
            {
                Button btn = new Button(new Point(0, ofs), character.Name + ", " + character.Level + ", " + Game.GetSpecName(character.Spec), 
                    new Rectangle(0, currentY, charactersPanel.WIDTH, btn_height));

                charactersPanel.AddComponent(btn);
                currentY += btn_height;
            }

            Button createCharacterButton = new Button(new Point(0, 0), "NEW CHARACTER", 
                new Rectangle(charactersPanel.X, charactersPanel.HEIGHT - btn_height - UI.DEFAULT_BORDER_HEIGHT,
                    charactersPanel.WIDTH, btn_height));
            createCharacterButton.Click += OnNewCharacterClicked;

            charactersPanel.AddComponent(createCharacterButton);

            userInterface.AddComponent(charactersPanel);
        }

        public void OnNewCharacterClicked(Button b, EventArgs e)
        {
            userInterface.MessageBoxShow("Unfinished section!");
        }
    }
}
