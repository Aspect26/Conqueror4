using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    class LoginScreen : EmptyScreen
    {
        private Color background = Color.Black;

        public LoginScreen()
        {
            int width = 300;
            LineInput nickInput = new LineInput(new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2), width, Color.Wheat, Color.Black);
            LineInput passInput = new PasswordInput(new Point(Game.WIDTH / 2 - width / 2, Game.HEIGHT / 2 + nickInput.HEIGHT + 5), width, Color.Wheat, Color.Black);

            userInterface.AddComponent(nickInput);
            userInterface.AddComponent(passInput);

            userInterface.SetFocusedComponent(nickInput);
        }

        public override void Render(Graphics g)
        {
            g.Clear(background);
            userInterface.Render(g);
        }
    }
}
