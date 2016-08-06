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

        public LoginScreen(Application game, ServerConnection server) : base(game, server, GameData.GetLoginBackground())
        {
            int width = 300;
            nickInput = new LineInput(position, new Point(Application.WIDTH / 2 - width / 2, Application.HEIGHT / 2),
                width, Color.Black);

            passInput = new PasswordInput(position,
                new Point(Application.WIDTH / 2 - width / 2, Application.HEIGHT / 2 + nickInput.HEIGHT + 5),
                width, Color.Black);

            loginButton = new Button(position, "Login",
                new Rectangle(Application.WIDTH / 2 - width / 2, Application.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));
            loginButton.Click += OnLoginClicked;

            registerButton = new Button(position, "Register",
                new Rectangle(Application.WIDTH / 2 - width / 2 + loginButton.WIDTH + 10, Application.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));
            registerButton.Click += OnRegisterClicked;

            loginText = new LineText(position, "Username:", Color.Yellow, new Point(
                nickInput.X - 150, nickInput.Y + 5), 20);

            passText = new LineText(position, "Password:", Color.Yellow, new Point(
                passInput.X - 150, passInput.Y + 5), 20);

            LineText buildText = new LineText(position, "Build: 68", Color.Wheat, new Point(
                700, 600), 15);

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
            userInterface.AddComponent(buildText);

            userInterface.SetFocusedComponent(nickInput);
        }

        private void OnLoginClicked(Button button, EventArgs args)
        {
            string nick = nickInput.getValue();
            string pass = passInput.getValue();
            if (!checkUsername(nick) || !checkPassword(pass))
                return;

            int result = application.server.LoginAccount(nick, pass);
            if (result == ServerConnection.RESULT_OK)
            {
                application.Account = new Account();
                application.Account.Username = nick;
                application.ChangeWindow(new CharactersScreen(application, this.server));
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
                        userInterface.MessageBoxShow("Wrong username or nicknamen or account already loggedd in."); break;
                }
            }
        }

        private void OnRegisterClicked(Button button, EventArgs e)
        {
            string nick = nickInput.getValue();
            string pass = passInput.getValue();
            if (!checkUsername(nick) || !checkPassword(pass))
                return;

            int result = application.server.RegisterAccount(nick, pass);
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

        private bool checkUsername(string username)
        {
            if (!GameData.IsValidUsername(username))
            {
                userInterface.MessageBoxShow("Invalid user name!");
                return false;
            }

            return true;
        }

        private bool checkPassword(string pass)
        {
            if (pass.Length < 5)
            {
                userInterface.MessageBoxShow("The password must contain at least 5 characters!");
                return false;
            }

            return true;
        }
    }
}
