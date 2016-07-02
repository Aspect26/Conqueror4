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

            userInterface.AddComponent(nickInput);
            userInterface.AddComponent(passInput);

            userInterface.SetFocusedComponent(nickInput);
        }
    }
}
