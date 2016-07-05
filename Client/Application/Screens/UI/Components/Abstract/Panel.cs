using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    public class Panel : BorderedRectangleComponent
    {
        protected UI container = new UI();

        public Panel(Point offsetPosition, Rectangle position, int borderSize = UI.DEFAULT_BORDER_HEIGHT, 
            IComponent neighbour = null)
            :base (offsetPosition, position, GameData.GetPanelBackground(), borderSize, neighbour)
        {
            
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
            g.DrawImage(backgroundImage, new Rectangle(position.X + borderSize, position.Y + borderSize,
                WIDTH - 2 * borderSize, HEIGHT - 2 * borderSize));
            container.Render(g);
        }

        public void AddComponent(IComponent component)
        {
            // top margin
            if (component.X < borderSize)
                component.MoveX(borderSize - component.X);

            // left margin
            if (component.Y < borderSize)
                component.MoveY(borderSize - component.Y);

            // right margin
            if(component.X + component.WIDTH > position.X + WIDTH - borderSize*2)
                component.MoveX( (borderSize + GetClientArea().Width) - (component.X + component.WIDTH));

            // bottom margin
            if (component.Y + component.HEIGHT > position.Y + HEIGHT - borderSize*2)
                component.MoveY((borderSize + GetClientArea().Height) - (component.Y + component.HEIGHT));

            component.MoveX(position.X);
            component.MoveY(position.Y);

            container.AddComponent(component);
        }

        public override void SetFocused(bool focused)
        {
            //this.focused = focused;
        }

        // ******************************************************
        // EVENTS - pass down to container
        // ******************************************************
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

        public override void OnMouseRightDown(Point position)
        {
            container.OnMouseRightDown(position);
        }

        public override void OnMouseRightUp(Point position)
        {
            container.OnMouseRightUp(position);
        }

        public override void OnMouseLeftUp(Point position)
        {
            container.OnMouseLeftUp(position);
        }
    }
}
