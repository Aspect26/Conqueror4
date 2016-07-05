using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class MessageBoxComponent : BorderedRectangleComponent
    {
        public delegate void OnCloseEvent(MessageBoxComponent messageBox);
        public event OnCloseEvent Closed;

        private const int MARGIN = 5;

        private UI container;

        public MessageBoxComponent(string text)
            : base(new Point(0,0), new Rectangle((Application.WIDTH - (Application.WIDTH / 5) * 4) / 2, (Application.HEIGHT - 96) / 2,
                (Application.WIDTH / 5) * 4, 96), Color.White)
        {
            Point thisPoisition = new Point(this.position.X, this.position.Y);
            container = new UI();
            container.AddComponent(
                new LineText(thisPoisition, text, Color.Black,
                    new Point(borderSize + MARGIN, borderSize + MARGIN), 12));

            int btn_width = 50;
            int btn_height = 30;
            Button OKButton = new Button(thisPoisition, "Ok", new Rectangle(
                this.WIDTH / 2 - btn_width / 2, this.HEIGHT - borderSize - MARGIN - btn_height, btn_width, btn_height));
            OKButton.Click += OnOKClicked;
            container.AddComponent(OKButton);

            container.SetFocusedComponent(OKButton);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            container.Render(g);
        }

        private void OnOKClicked(Button b, EventArgs args)
        {
            Closed(this);
        }

        // EVENTS
        public override void OnKeyDown(int key)
        {
            container.OnKeyDown(key);
        }

        public override void OnKeyUp(int key)
        {
            container.OnKeyUp(key);
        }

        public override void OnMouseLeftDown(Point position)
        {
            container.OnMouseLeftDown(position);
        }

        public override void OnMouseLeftUp(Point position)
        {
            container.OnMouseLeftUp(position);
        }
    }
}
