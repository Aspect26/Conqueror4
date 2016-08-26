using System;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Repesents the login screen. It's the first screen shown to the use when the application is executed.
    /// </summary>
    /// <seealso cref="Client.EmptyScreen" />
    class LoginScreen : EmptyScreen
    {
        private const int BUILD_NUMBER = 97;
        private static Point position = new Point(0, 0);
        private LineInput nickInput;
        private PasswordInput passInput;
        private Button loginButton;
        private Button registerButton;
        private LineText loginText;
        private LineText passText;
        private LineText buildText;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginScreen"/> class.
        /// Initializes it's interface components.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="server">The server connection.</param>
        public LoginScreen(Application application, ServerConnection server) : base(application, server, GameData.GetLoginBackground())
        {
            initializeComponents();
            setNeighbours();
            addComponentsToInterface();

            userInterface.SetFocusedComponent(nickInput);
        }

        /// <summary>
        /// Initializes the interface components.
        /// </summary>
        private void initializeComponents()
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
            loginButton.OnClick += OnLoginClicked;

            registerButton = new Button(position, "Register",
                new Rectangle(Application.WIDTH / 2 - width / 2 + loginButton.WIDTH + 10, Application.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));
            registerButton.OnClick += OnRegisterClicked;

            loginText = new LineText(position, "Username:", Color.Yellow, new Point(
                nickInput.X - 150, nickInput.Y + 5), 20);

            passText = new LineText(position, "Password:", Color.Yellow, new Point(
                passInput.X - 150, passInput.Y + 5), 20);

            buildText = new LineText(position, "Build: " + BUILD_NUMBER, Color.Wheat, new Point(
                700, 600), 15);
        }

        private void setNeighbours()
        {
            nickInput.SetNeighbour(passInput);
            passInput.SetNeighbour(loginButton);
            loginButton.SetNeighbour(registerButton);
            registerButton.SetNeighbour(nickInput);
        }

        /// <summary>
        /// Adds the components to the interface.
        /// </summary>
        private void addComponentsToInterface()
        {
            userInterface.AddComponent(nickInput);
            userInterface.AddComponent(passInput);
            userInterface.AddComponent(loginButton);
            userInterface.AddComponent(registerButton);
            userInterface.AddComponent(loginText);
            userInterface.AddComponent(passText);
            userInterface.AddComponent(buildText);
        }

        /// <summary>
        /// Handles the <see cref="E:LoginClicked" /> event. 
        /// Sends a message to the server with logging information and waits for the server's response.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
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
                        userInterface.ShowMessageBox("Can't connect to the server!"); break;
                    case ServerConnection.RESULT_CANTSEND:
                    case ServerConnection.RESULT_EMPTY:
                        userInterface.ShowMessageBox("Can't communicate with the server!"); break;
                    case ServerConnection.RESULT_FALSE:
                        userInterface.ShowMessageBox("Wrong username or nicknamen or account already loggedd in."); break;
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="E:RegisterClicked" /> event.
        /// Sends a message to the server with new account's information and waits for the server's response.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRegisterClicked(Button button, EventArgs e)
        {
            string nick = nickInput.getValue();
            string pass = passInput.getValue();
            if (!checkUsername(nick) || !checkPassword(pass))
                return;

            int result = application.server.RegisterAccount(nick, pass);
            if (result == ServerConnection.RESULT_OK)
            {
                userInterface.ShowMessageBox("Account created successfully!");
            }
            else
            {
                switch (result)
                {
                    case ServerConnection.RESULT_CANTCONNECT:
                        userInterface.ShowMessageBox("Can't connect to the server!"); break;
                    case ServerConnection.RESULT_CANTSEND:
                    case ServerConnection.RESULT_EMPTY:
                        userInterface.ShowMessageBox("Can't communicate with the server!"); break;
                    case ServerConnection.RESULT_FALSE:
                        userInterface.ShowMessageBox("Could not create specified account!"); break;
                }
            }
        }

        /// <summary>
        /// Checks whether the username is valid. This should be on the server's side but for the sake of simplicity it is
        /// here.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns><c>true</c> if the username is valid, <c>false</c> otherwise.</returns>
        private bool checkUsername(string username)
        {
            if (!GameData.IsValidUsername(username))
            {
                userInterface.ShowMessageBox("Invalid user name!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the passoword is valid. This should be on the server's side but for the sake of simplicity it is
        /// here.
        /// </summary>
        /// <param name="pass">The password.</param>
        /// <returns><c>true</c> if the password is valid, <c>false</c> otherwise.</returns>
        private bool checkPassword(string pass)
        {
            if (pass.Length < 5)
            {
                userInterface.ShowMessageBox("The password must contain at least 5 characters!");
                return false;
            }

            return true;
        }
    }
}
