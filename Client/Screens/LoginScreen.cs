using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    class LoginScreen : EmptyScreen
    {
        public LoginScreen() : base(Game.GetLoginBackground())
        {
            int width = 300;
            LineInput nickInput = new LineInput(new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2), 
                width, Color.Black, Color.Wheat);

            PasswordInput passInput = new PasswordInput(
                new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2 + nickInput.HEIGHT + 5), 
                width, Color.Black, Color.Wheat);

            Button loginButton = new Button("Login",
                new Rectangle(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));

            Button registerButton = new Button("Register",
                new Rectangle(Game.WIDTH / 2 - width / 2 + loginButton.WIDTH + 10, Game.HEIGHT / 2 + 2 * nickInput.HEIGHT + 2 * 5,
                width / 2 - 5, 30));

            LineText loginText = new LineText("Username:", Color.Black, new Point(
                nickInput.X - 150, nickInput.Y + 5), 20);

            LineText passText = new LineText("Password:", Color.Black, new Point(
                passInput.X - 150, passInput.Y + 5), 20);

            userInterface.AddComponent(nickInput);
            userInterface.AddComponent(passInput);
            userInterface.AddComponent(loginButton);
            userInterface.AddComponent(registerButton);
            userInterface.AddComponent(loginText);
            userInterface.AddComponent(passText);

            userInterface.SetFocusedComponent(nickInput);
        }
    }
}
