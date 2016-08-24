using System.Drawing;
using Shared;

namespace Client
{
    /// <summary>
    /// A basic animation for a unit.
    /// Unit animation is composed of 7 images. 2 images for moving up, 2 images for moving to a side (left or right),
    /// 2 images for moving down and one image for not moving.
    /// All the images are store in one folder. The animation gets path to this folder + initial name of the file.
    /// The files then have suffix: [ _front1, _front2, _back1, _back2, _side1, _side2, none ].png
    /// </summary>
    /// <seealso cref="Client.IAnimation" />
    public class UnitAnimation : IAnimation
    {
        protected string baseImagePath;
        protected PlayedCharacter unit;

        protected const int FRONT = 0;
        protected const int RIGHT = 1;
        protected const int BACK = 2;
        protected const int LEFT = 3;

        protected Image[][] images;
        protected Image noAnimationImage;
        protected bool moving;

        protected int currentImageSide;
        protected int currentImageIndex;

        protected const int CHANGE_SPEED_MS = 400;
        protected int lastChangeBefore = 499;
        protected MovingDirection lastDirection = MovingDirection.None;

        protected Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAnimation"/> class.
        /// Initializes the animation images. If the image doesn't exist the application crashes... Someone please add that try, catch block.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="baseImagePath">The base image path.</param>
        public UnitAnimation(Game game, PlayedCharacter unit, string baseImagePath)
        {
            this.unit = unit;
            this.baseImagePath = baseImagePath;
            this.game = game;

            createImages();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAnimation"/> class.
        /// Initializes the animation images. If the image doesn't exist the application crashes... Someone please add that try, catch block.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="baseImagePath">The base image path.</param>
        public UnitAnimation(PlayedCharacter unit, string baseImagePath)
        {
            this.unit = unit;
            this.baseImagePath = baseImagePath;

            createImages();
        }

        private void createImages()
        {
            noAnimationImage = Image.FromFile(baseImagePath + "_none.png");

            images = new Image[4][];

            images[FRONT] = new Image[2];
            images[FRONT][0] = Image.FromFile(baseImagePath + "_front1.png");
            images[FRONT][1] = Image.FromFile(baseImagePath + "_front2.png");

            images[LEFT] = new Image[2];
            images[LEFT][0] = Image.FromFile(baseImagePath + "_side1.png"); images[LEFT][0].RotateFlip(RotateFlipType.RotateNoneFlipX);
            images[LEFT][1] = Image.FromFile(baseImagePath + "_side2.png"); images[LEFT][1].RotateFlip(RotateFlipType.RotateNoneFlipX);

            images[RIGHT] = new Image[2];
            images[RIGHT][0] = Image.FromFile(baseImagePath + "_side1.png");
            images[RIGHT][1] = Image.FromFile(baseImagePath + "_side2.png");

            images[BACK] = new Image[2];
            images[BACK][0] = Image.FromFile(baseImagePath + "_back1.png");
            images[BACK][1] = Image.FromFile(baseImagePath + "_back2.png");

            currentImageSide = FRONT;
            currentImageIndex = 0;
            moving = false;
        }

        /// <summary>
        /// Renders the animated object.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public virtual void Render(Graphics g)
        {
            Point position = game.MapPositionToScreenPosition((int)unit.Location.X, (int)unit.Location.Y);

            if (!moving)
            {
                g.DrawImageAt(noAnimationImage, position.X, position.Y);
            }
            else
            {
                g.DrawImageAt(images[currentImageSide][currentImageIndex], position.X, position.Y);
            }
        }

        /// <summary>
        /// Animates one game cycle.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        public void AnimateCycle(int timeSpan)
        {
            lastChangeBefore += timeSpan;

            if(!AnimationEquals(lastDirection, unit.MovingDirection))
            {
                changeSide();
                return;
            }

            if (unit.MovingDirection == MovingDirection.None)
            {
                moving = false;
                return;
            }

            if (lastChangeBefore > CHANGE_SPEED_MS)
            {
                // increase index
                lastChangeBefore = lastChangeBefore % CHANGE_SPEED_MS;
                currentImageIndex = (currentImageIndex + 1) % images[currentImageSide].Length;
            }
        }

        /// <summary>
        /// Checks whether the two moving directions are the same.
        /// This is implemented as a binary operator, hence the parameter names right and left.
        /// </summary>
        /// <param name="left">The first operand.</param>
        /// <param name="right">The second operand.</param>
        /// <returns><c>true</c> if the directions equal, <c>false</c> otherwise.</returns>
        private bool AnimationEquals(MovingDirection left, MovingDirection right)
        {
            if (left == right)
                return true;

            // UP && UP_RIGHT
            if ( (left == MovingDirection.Up && right.HasFlag(MovingDirection.Up) && right.HasFlag(MovingDirection.Right)) ||
                 (right == MovingDirection.Up && left.HasFlag(MovingDirection.Up) && left.HasFlag(MovingDirection.Right)) )
                return true;

            // RIGHT && BOTTOM_RIGHT
            if ((left == MovingDirection.Right && right.HasFlag(MovingDirection.Right) && right.HasFlag(MovingDirection.Bottom)) ||
                (right == MovingDirection.Right && left.HasFlag(MovingDirection.Right) && left.HasFlag(MovingDirection.Bottom)))
                return true;

            // BOTTOM && BOTTOM_LEFT
            if ((left == MovingDirection.Bottom && right.HasFlag(MovingDirection.Bottom) && right.HasFlag(MovingDirection.Left)) ||
                (right == MovingDirection.Bottom && left.HasFlag(MovingDirection.Bottom) && left.HasFlag(MovingDirection.Left)))
                return true;

            // LEFT && UP_LEFT
            if ((left == MovingDirection.Left && right.HasFlag(MovingDirection.Left) && right.HasFlag(MovingDirection.Up)) ||
                (right == MovingDirection.Left && left.HasFlag(MovingDirection.Left) && left.HasFlag(MovingDirection.Up)))
                return true;

            return false;
        }

        private void changeSide()
        {
            lastChangeBefore = 499;
            currentImageIndex = 0;
            moving = true;

            if (unit.MovingDirection.HasFlag(MovingDirection.Up))
            {
                if (unit.MovingDirection.HasFlag(MovingDirection.Left))
                    currentImageSide = LEFT;
                else
                    currentImageSide = BACK;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Right))
            {
                currentImageSide = RIGHT;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Bottom))
            {
                currentImageSide = FRONT;
            }
            else if (unit.MovingDirection.HasFlag(MovingDirection.Left))
            {
                currentImageSide = LEFT;
            }
            else
            {
                moving = false;
            }

            lastDirection = unit.MovingDirection;
        }

        /// <summary>
        /// Gets the current image of the animation.
        /// </summary>
        /// <returns>Image.</returns>
        public Image GetCurrentImage()
        {
            if (!moving)
                return noAnimationImage;
            else
                return images[currentImageSide][currentImageIndex];
        }
    }
}
