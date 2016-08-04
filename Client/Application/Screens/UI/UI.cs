using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary>
    /// Represents the whole UI of one screen. Holds informtion about all the components in the current UI like 
    /// textboxes, buttons, etc. It also holds information about whether a component is selected and sends user
    /// inputs to the corresponding component. Also draws all the components everytime a render function is called. 
    /// </summary>
    public class UI
    {
        public const int DEFAULT_BORDER_HEIGHT = 4;

        private IComponent focusedComponent;
        private List<IComponent> components;
        private MessageBoxComponent messageBox;

        public UI()
        {
            this.components = new List<IComponent>();
            this.focusedComponent = null;
            this.messageBox = null;
        }

        public void SetFocusedComponent(IComponent focusedComponent)
        {
            if (messageBox == null)
            {
                if (this.focusedComponent != null)
                    this.focusedComponent.SetFocused(false);

                this.focusedComponent = focusedComponent;
                if(this.focusedComponent != null)
                    this.focusedComponent.SetFocused(true);
            }
        }

        public void AddComponent(IComponent component)
        {
            this.components.Add(component);
        }

        public void RemoveComponent(IComponent component)
        {
            this.components.Remove(component);
        }

        public IComponent GetFocusedComponent()
        {
            return this.focusedComponent;
        }

        public void MessageBoxShow(string text)
        {
            SetFocusedComponent(null);
            messageBox = new MessageBoxComponent(text);
            messageBox.Closed += OnMessageBoxClosed;
        }

        public void Render(Graphics g)
        {
            foreach(IComponent c in components)
            {
                if (c.Shown)
                {
                    c.Render(g);
                }
            }

            if (messageBox != null)
                messageBox.Render(g);
        }

        private IComponent findComponentAt(Point location)
        {
            foreach (IComponent c in components)
            {
                if (c.IsAt(location))
                    return c;
            }

            return null;
        }

        // **********************************************
        // EVENTS
        // **********************************************
        public void OnMessageBoxClosed(MessageBoxComponent mBox)
        {
            if(mBox != messageBox)
            {
                Console.WriteLine("FATAL ERROR IN MESAGEBOX!!!!!!!!!!!");
            }
            this.messageBox = null;
        }

        public void OnKeyDown(int key)
        {
            if(messageBox != null)
            {
                messageBox.OnKeyDown(key);
                return;
            }

            if(focusedComponent != null)
            {
                focusedComponent.OnKeyDown(key);
            }
        }

        public void OnKeyUp(int key)
        {
            if(messageBox != null)
            {
                messageBox.OnKeyUp(key);
                return;
            }

            if (focusedComponent != null)
            {
                if (key == 9)
                    SetFocusedComponent(focusedComponent.GetNeighbour());
                else
                    focusedComponent.OnKeyUp(key);
            }
        }

        public void OnMouseLeftDown(Point location)
        {
            if(messageBox != null)
            {
                messageBox.OnMouseLeftDown(location);
                return;
            }

            if (focusedComponent != null)
            {
                focusedComponent.SetFocused(false);
            }

            focusedComponent = findComponentAt(location);

            if (focusedComponent != null)
            {
                focusedComponent.SetFocused(true);
                focusedComponent.OnMouseLeftDown(location);
            }
        }

        public void OnMouseLeftUp(Point location)
        {
            if (messageBox != null)
            {
                messageBox.OnMouseLeftUp(location);
                return;
            }

            if (focusedComponent != null)
            {
                focusedComponent.OnMouseLeftUp(location);
            }
        }

        public void OnMouseRightDown(Point location)
        {
            if (messageBox == null && focusedComponent != null)
            {
                focusedComponent.OnMouseRightDown(location);
            }
        }

        public void OnMouseRightUp(Point location)
        {
            if (messageBox == null && focusedComponent != null)
            {
                focusedComponent.OnMouseRightUp(location);
            }
        }
    }
}
