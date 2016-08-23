using System.Drawing;

namespace Client
{
    public class CreateCharacterSelection : RectangleComponent
    {
        private CreateCharacterButton[] characterButtons;

        public CreateCharacterSelection()
            : base(new Point(0, 0), new Rectangle(Application.WIDTH / 2 - 3 * 64, 450, 64 * 6, 64), Color.Black)
        {
            // character buttons
            this.characterButtons = new CreateCharacterButton[6];
            for (int i = 0; i < 6; i++)
            {
                this.characterButtons[i] = new CreateCharacterButton(new Point(0, 0),
                    new Rectangle(this.X + (i*64), this.Y, 64, 64), i + 1);
            }
            this.characterButtons[0].SetFocused(true);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            foreach(CreateCharacterButton btn in characterButtons)
            {
                btn.Render(g);
            }
        }

        public override void OnMouseLeftDown(Point position)
        {
            foreach(CreateCharacterButton btn in characterButtons)
            {
                btn.SetFocused(btn.IsAt(position));
            }
        }

        public int GetSelectedSpec()
        {
            foreach (CreateCharacterButton btn in characterButtons)
            {
                if (btn.IsFocused())
                {
                    return btn.GetSpec();
                }
            }

            return -1;
        }
    }
}
