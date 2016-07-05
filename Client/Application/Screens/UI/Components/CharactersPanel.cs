using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class CharactersPanel : Panel
    {
        public CharactersPanel(Point offsetPosition, Rectangle position, int borderSize = UI.DEFAULT_BORDER_HEIGHT,
            IComponent neighbour = null)
        :base(offsetPosition, position, borderSize, neighbour)
        {

        }

        public Character GetSelectedCharacter()
        {
            CharacterButton btn = (CharacterButton)container.GetFocusedComponent();
            if (btn == null)
                return null;

            return btn.GetCharacter();
        }
    }
}
