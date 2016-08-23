using System.Drawing;

namespace Client
{
    public class CreateCharacterScreen : EmptyScreen
    {
        private const int CHARACTER_IMAGE_PADDING = 80;

        private CreateCharacterSelection characterSelection;
        private Button createButton;
        private Button cancelButton;
        private LineInput nickname;

        private Font infoFont;
        private Rectangle screenRect = new Rectangle(0, 0, Application.WIDTH, Application.HEIGHT);

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
            this.createButton.OnClick += CreateButton_OnClick;
            this.userInterface.AddComponent(createButton);

            // cancel button
            this.cancelButton = new Button(new Point(0, 0), "Cancel", new Rectangle(
                Application.WIDTH - buttonWidth - 20, yPosition + 70, buttonWidth, 40));
            this.cancelButton.OnClick += CancelButton_OnClick;
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

        private void CreateButton_OnClick(Button m, System.EventArgs e)
        {
            int spec = characterSelection.GetSelectedSpec();
            createCharacter(spec, nickname.getValue());
        }

        private void CancelButton_OnClick(Button m, System.EventArgs e)
        {
            application.ChangeWindow(new CharactersScreen(application, server));
        }

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

        private void createCharacter(int spec, string name)
        {
            if(name.Length < 4)
            {
                userInterface.MessageBoxShow("The nickname must have at least 4 letters.");
                return;
            }

            int result = server.CreateCharacter(application.Account.Username, name, spec);
            if (result != ServerConnection.RESULT_OK)
            {
                userInterface.MessageBoxShow("The nickname is already taken.");
            }
            else
            {
                application.ChangeWindow(new CharactersScreen(application, server));
            }
        }
    }
}
