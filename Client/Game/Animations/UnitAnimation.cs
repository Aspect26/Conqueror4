using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class UnitAnimation : IAnimation
    {
        private string baseImagePath;
        private SimpleUnit unit;

        private const int FRONT = 0;
        private const int RIGHT = 1;
        private const int BACK = 2;
        private const int LEFT = 3;

        private Image[][] images;
        private Image noAnimationImage;
        private bool moving;

        private int currentImageSide;
        private int currentImageIndex;

        private const int CHANGE_SPEED_MS = 400;
        private int lastChangeBefore = 499;
        private MovingDirection lastDirection = MovingDirection.None;

        public UnitAnimation(SimpleUnit unit, string baseImagePath)
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

        public void Render(Graphics g)
        {
            int size = unit.UnitSize;

            if (!moving)
            {
                g.DrawImage(noAnimationImage,
                    Application.WIDTH / 2 - size / 2, Application.HEIGHT / 2 - size / 2, size, size);
            }
            else
            {
                g.DrawImage(images[currentImageSide][currentImageIndex],
                    Application.WIDTH / 2 - size / 2, Application.HEIGHT / 2 - size / 2, size, size);
            }
        }

        public void AnimateCycle(int timeSpan)
        {
            lastChangeBefore += timeSpan;

            if(unit.Direction != lastDirection)
            {
                Console.WriteLine("CHANGING DIRECTION");
                changeSide();
                return;
            }

            if (unit.Direction == MovingDirection.None)
            {
                Console.WriteLine("STOPPED MOVING");
                moving = false;
                return;
            }

            if (lastChangeBefore > CHANGE_SPEED_MS)
            {
                // increase index
                Console.WriteLine("INCREASING INDEX");
                lastChangeBefore = lastChangeBefore % CHANGE_SPEED_MS;
                currentImageIndex = (currentImageIndex + 1) % images[currentImageSide].Length;
            }
        }

        private void changeSide()
        {
            lastChangeBefore = 499;
            currentImageIndex = 0;
            moving = true;

            switch (unit.Direction)
            {
                case MovingDirection.Right:
                    currentImageSide = RIGHT; break;

                case MovingDirection.Bottom:
                    currentImageSide = FRONT; break;

                case MovingDirection.Left:
                    currentImageSide = LEFT; break;

                case MovingDirection.Top:
                    currentImageSide = BACK; break;

                default:
                    moving = false; break;
            }
            lastDirection = unit.Direction;
        }

        public Image GetCurrentImage()
        {
            return images[currentImageSide][currentImageIndex];
        }
    }
}
