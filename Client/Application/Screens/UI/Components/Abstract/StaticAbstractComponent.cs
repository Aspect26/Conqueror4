using System.Drawing;

namespace Client
{
    /// <summary>
    /// An abstract class for all the components that shall have a custom abstract shape and won't react to user's inputs.
    /// </summary>
    /// <seealso cref="Client.IComponent" />
    public abstract class StaticAbstractComponent : BaseComponent
    {
        /// <summary>
        /// Returns 1 because it shall not be needed anywhere.
        /// </summary>
        /// <value>The width.</value>
        public override int WIDTH { get { return 1; } }

        /// <summary>
        /// Returns 1 because it shall not be needed anywhere.
        /// </summary>
        /// <value>The height.</value>
        public override int HEIGHT { get { return 1; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticAbstractComponent"/> class.
        /// </summary>
        /// <param name="shown">if set to <c>true</c> the component shall be shown.
        public StaticAbstractComponent(bool shown = true)
        {
            this.Shown = shown;
        }

        /// <summary>
        /// Does nothing becaus height for this type of component is irelevant.
        /// </summary>
        /// <param name="height">The new height.</param>
        public override void ChangeHeight(int height)
        {
            return;
        }

        /// <summary>
        /// Does nothing becaus height for this type of component is irelevant.
        /// </summary>
        /// <param name="width">The new width.</param>
        public override void ChangeWidth(int width)
        {
            return;
        }

        /// <summary>
        /// Returns an empty rectangle because client area for this kind of component is irelevant.
        /// </summary>
        /// <returns>The client area rectangle.</returns>
        public override Rectangle GetClientArea()
        {
            return new Rectangle(X, Y, WIDTH, HEIGHT);
        }

        /// <summary>
        /// Always returns false so the UserInterace class won't redirect user inputs to this component.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns><c>false</c>.</returns>
        public override bool IsAt(Point location)
        {
            return false;
        }
    }
}
