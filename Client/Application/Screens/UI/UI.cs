using System;
using System.Collections.Generic;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the whole UI of one screen. Holds informtion about all the components in the current UI like
    /// textboxes, buttons, etc. It also holds information about whether a component is selected and sends user
    /// inputs to the corresponding component. Also draws all the components everytime a render function is called.
    /// </summary>
    public class UserInterface
    {
        /// <summary>
        /// The default border height for components
        /// </summary>
        public const int DEFAULT_BORDER_HEIGHT = 4;

        private IComponent focusedComponent;
        private List<IComponent> components;
        private MessageBoxComponent messageBox;

        private Point mousePosition;
        private object mousePositionLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterface"/> class.
        /// </summary>
        public UserInterface()
        {
            this.components = new List<IComponent>();
            this.focusedComponent = null;
            this.messageBox = null;
            this.mousePosition = new Point(0, 0);
        }

        /// <summary>
        /// Sets the focused component.
        /// </summary>
        /// <param name="focusedComponent">The focused component.</param>
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

        /// <summary>
        /// Adds a component to the interface.
        /// </summary>
        /// <param name="component">The component.</param>
        public void AddComponent(IComponent component)
        {
            this.components.Add(component);
        }

        /// <summary>
        /// Removes a component from the interface.
        /// </summary>
        /// <param name="component">The component.</param>
        public void RemoveComponent(IComponent component)
        {
            this.components.Remove(component);
        }

        /// <summary>
        /// Gets the focused component.
        /// </summary>
        /// <returns>IComponent.</returns>
        public IComponent GetFocusedComponent()
        {
            return this.focusedComponent;
        }


        /// <summary>
        /// Shows the message box. Only one message box can be shown at a time. If there is a message box shown,
        /// all the user inputs are redirected to the message box instead of ui components. 
        /// </summary>
        /// <param name="text">Text of the message box.</param>
        public void ShowMessageBox(string text)
        {
            SetFocusedComponent(null);
            messageBox = new MessageBoxComponent(text);
            messageBox.Closed += OnMessageBoxClosed;
        }

        /// <summary>
        /// Renders all the shown ui components and if there is a message box, it draws it above all of them.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public void Render(Graphics g)
        {
            foreach(IComponent c in components)
            {
                if (c.Shown)
                {
                    c.Render(g);
                }
                lock(mousePositionLock) {
                    if (c.IsAt(mousePosition) && c.HasTooltip())
                    {
                        c.RenderTooltip(g, mousePosition);
                    }
                }

            }

            if (messageBox != null)
                messageBox.Render(g);
        }

        /// <summary>
        /// Finds a component on the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The found compoenent or null, if there is no component on the specified location..</returns>
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

        /// <summary>
        /// Called when message box is closed.
        /// </summary>
        /// <param name="mBox">The message box.</param>
        public void OnMessageBoxClosed(MessageBoxComponent mBox)
        {
            if(mBox != messageBox)
            {
                Console.WriteLine("FATAL ERROR IN MESAGEBOX!!!!!!!!!!!");
            }
            this.messageBox = null;
        }

        /// <summary>
        /// Called when [key down].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="key">The key.</param>
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

        /// <summary>
        /// Called when [key up].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="key">The key.</param>
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

        /// <summary>
        /// Called when [mouse left down].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="location">The location.</param>
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

        /// <summary>
        /// Called when [mouse left up].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="location">The location.</param>
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

        /// <summary>
        /// Called when [mouse right down].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="location">The location.</param>
        public void OnMouseRightDown(Point location)
        {
            if (messageBox == null && focusedComponent != null)
            {
                focusedComponent.OnMouseRightDown(location);
            }
        }

        /// <summary>
        /// Called when [mouse right up].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="location">The location.</param>
        public void OnMouseRightUp(Point location)
        {
            if (messageBox == null && focusedComponent != null)
            {
                focusedComponent.OnMouseRightUp(location);
            }
        }

        /// <summary>
        /// Called when [mouse move].
        /// Redirects the event to the focused componenr or message box if shown.
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        public void OnMouseMove(int x, int y)
        {
            lock (mousePositionLock)
            {
                this.mousePosition.X = x;
                this.mousePosition.Y = y;
            }
        }
    }
}
