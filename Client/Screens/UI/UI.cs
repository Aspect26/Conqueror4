﻿using System;
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
        public const int DEFAULT_BORDER_HEIGHT = 10;

        private IComponent focusedComponent;
        private List<IComponent> components;

        public UI()
        {
            this.components = new List<IComponent>();
            this.focusedComponent = null;
        }

        public void SetFocusedComponent(IComponent focusedComponent)
        {
            if(this.focusedComponent != null)
                this.focusedComponent.SetFocused(false);

            this.focusedComponent = focusedComponent;
            this.focusedComponent.SetFocused(true);
        }

        public void AddComponent(IComponent component)
        {
            this.components.Add(component);
        }

        public void RemoveComponent(IComponent component)
        {
            this.components.Remove(component);
        }

        public void Render(Graphics g)
        {
            foreach(IComponent c in components)
            {
                c.Render(g);
            }
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

        public void OnKeyDown(int key)
        {
            if(focusedComponent != null)
            {
                focusedComponent.OnKeyDown(key);
            }
        }

        public void OnKeyUp(int key)
        {
            if (focusedComponent != null)
            {
                focusedComponent.OnKeyUp(key);
            }
        }

        public void OnMouseLeftDown(Point location)
        {
            if (focusedComponent != null)
            {
                focusedComponent.SetFocused(false);
            }

            focusedComponent = findComponentAt(location);

            if (focusedComponent != null)
            {
                focusedComponent.SetFocused(true);
                focusedComponent.OnLeftMouseDown(location);
            }
        }

        public void OnMouseLeftUp(Point location)
        {
            if (focusedComponent != null)
            {
                focusedComponent.OnLeftMouseUp(location);
            }
        }

        public void OnMouseRightDown(Point location)
        {
            if (focusedComponent != null)
            {
                focusedComponent.OnRightMouseDown(location);
            }
        }

        public void OnMouseRightUp(Point location)
        {
            if (focusedComponent != null)
            {
                focusedComponent.OnRightMouseUp(location);
            }
        }
    }
}
