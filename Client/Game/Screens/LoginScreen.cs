using System;
using System.Drawing;

namespace Client
{
    class LoginScreen : EmptyScreen
    {
        static Point position = new Point(0, 0);
        LineInput nickInput;
        PasswordInput passInput;
        Button loginButton;
        Button registerButton;
        LineText loginText;
        LineText passText;

        public LoginScreen(Game game) : base(game, Game.GetLoginBackground())
        {
            int width = 300;
            nickInput = new LineInput(position, new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2),
                width, Color.Black, Color.Wheat, passInput);

            passInput = new PasswordInput(position,
                new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2 + nickInput.HEIGHT + 5),
                width, Color.Black, Color.Wheat);

            loginButton = new Button(position, "Login",
                new Rectangle(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));
            loginButton.Click += OnLoginClicked;

            registerButton = new Button(position, "Register",
                new Rectangle(Game.WIDTH / 2 - width / 2 + loginButton.WIDTH + 10, Game.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));
            registerButton.Click += OnRegisterClicked;

            loginText = new LineText(position, "Username:", Color.Black, new Point(
                nickInput.X - 150, nickInput.Y + 5), 20);

            passText = new LineText(position, "Password:", Color.Black, new Point(
                passInput.X - 150, passInput.Y + 5), 20);

            nickInput.SetNeighbour(passInput);
            passInput.SetNeighbour(loginButton);
            loginButton.SetNeighbour(registerButton);
            registerButton.SetNeighbour(nickInput);

            userInterface.AddComponent(nickInput);
            userInterface.AddComponent(passInput);
            userInterface.AddComponent(loginButton);
            userInterface.AddComponent(registerButton);
            userInterface.AddComponent(loginText);
            userInterface.AddComponent(passText);

            userInterface.SetFocusedComponent(nickInput);
        }

        private void OnLoginClicked(Button button, EventArgs e)
        {
            userInterface.MessageBoxShow("Uncompleted section!");
        }

        private void OnRegisterClicked(Button button, EventArgs e)
        {
            string nick = nickInput.getValue();
            if (!Game.IsValidUsername(nick))
            {
                userInterface.MessageBoxShow("Invalid user name!");
                return;
            }

            string pass = passInput.getValue();
            if (pass.Length < 5)
            {
                userInterface.MessageBoxShow("The password must contain at least 5 characters!");
                return;
            }

            int result = game.server.RegisterAccount(nick, pass);
            if (result == ServerConnection.RESULT_OK)
            {
                userInterface.MessageBoxShow("Account created successfully!");
            }
            else
            {
                switch (result)
                {
                    case ServerConnection.RESULT_CANTCONNECT:
                        userInterface.MessageBoxShow("Can't connect to the server!"); break;
                    case ServerConnection.RESULT_CANTSEND:
                    case ServerConnection.RESULT_EMPTY:
                        userInterface.MessageBoxShow("Can't communicate with the server!"); break;
                    case ServerConnection.RESULT_FALSE:
                        userInterface.MessageBoxShow("Could not create specified account!"); break;
                }
            }
        }
    }
}
