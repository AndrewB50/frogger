using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Frogger
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }
    class MovingItemList
    {
        private MovingItem[] car;
        private int carCount = 50;          // number of cars
        private Direction direction;    // direction they are moving
        private Rectangle screen;       // screen they are drawn on
        private float speed;

        static Random rnd = new Random();

        /// <summary>
        /// Create a list of moving items
        /// </summary>
        /// <param name="image">The image for the cars. All cars use the same image.</param>
        /// <param name="position">The location of the first car in the list.</param>
        /// <param name="theSpeed">The number of pixels per tick to move.</param>
        /// <param name="theDirection">The direction the cars are moving.</param>
        /// <param name="screen">The screen on which the cars will be drawn.</param>
        public MovingItemList(Texture2D image, Rectangle position, float theSpeed, Direction theDirection, Rectangle theScreen)
        {
            // Console.WriteLine(screen.Right);
            speed = theSpeed;
            direction = theDirection;
            screen = theScreen;
            car = new MovingItem[carCount];
            // Cars moving West (left) will start with car[0] on the left edge of the screen.
            int gap = rnd.Next(3, 7);
            if (direction == Direction.West)
                for (int i = 0; i < carCount; i++)
                {
                    position.X = i * position.Width;
                    car[i] = new MovingItem(image, position, -speed);
                    car[i].Visible = false;
                    gap--;
                    if (gap == 0)
                    {
                        car[i].Visible = true;
                        gap = rnd.Next(3, 9);
                    }
                    //Console.WriteLine(i + " " + position + " " + theSpeed);
                }
            // Cars moving East (right) will start with car[0] on the right edge of the screen.
            else if (direction == Direction.East)
                for (int i = 0; i < carCount; i++)
                {
                    position.X = screen.Right - i * position.Width;
                    car[i] = new MovingItem(image, position, speed);
                    car[i].Visible = false;
                    gap--;
                    if (gap == 0)
                    {
                        car[i].Visible = true;
                        gap = rnd.Next(3, 9);
                    }
                }

        }

        /// <summary>
        /// Return the speed of the items in the list.
        /// </summary>
        public float Speed
        {
            get{ return speed;}
        }

        public Direction Direction
        {
            get { return direction; }
        }

        /// <summary>
        /// Return true if the frog has collided with any visible object on this list.
        /// </summary>
        /// <param name="frog">The sprite that we are testing to see if it has collided with anything on the list.</param>
        /// <returns></returns>
        public bool HasCollision(Frogger frog)
        {
            for (int i = 0; i < carCount; i++)
                if (car[i].Visible)
                    if (car[i].Position.Intersects(frog.thePosition))
                        return true;

            return false;
        }


        //public bool LandsOnLog(Frog frog)
        //{
        //    for (int i = 0; i < carCount; i++)
        //        if (car[i].Visible)
        //            if (car[i].Position.Intersects(frog.Position))
        //            {
        //                frog.Speed = car[i].Speed;
        //                return true;
        //            }

        //    return false;
        //}


        //public bool FallsInRiver(Frog frog)
        //{
        //    // The frog isn't even on this row!
        //    if (car[0].Position.Y != frog.Position.Y)
        //        return false;

        //    for (int i = 0; i < carCount; i++)
        //        if (car[i].Visible == false) // we're between logs/turtles
        //            if (car[i].Position.Intersects(frog.Position))
        //                return true;

        //    return false;
        //}

        /// <summary>
        /// Return true if the frog has landed on any visible object on this list.
        /// </summary>
        /// <param name="frog">The sprite that we are testing to see if it has landed on anything on this list.</param>
        /// <returns></returns>
        //public bool HasLandedOn(Frog frog)
        //{
        //    return HasCollision(frog);
        //}

        // Update the list by moving each list element
        public void Update()
        {
            Rectangle s;
            for (int i = 0; i < carCount; i++)
            {
                car[i].Move();
                s = car[i].Position;
                if (direction == Direction.West)
                {
                    if (car[i].Position.Right < screen.Left)
                    {   // Move it around to the east end
                        //Console.WriteLine("{0} right edge is {1}", i, s.Right);
                        if (i != 0)
                            s.X = car[i - 1].Position.Left + car[i - 1].Position.Width;
                        else
                            s.X = car[carCount - 1].Position.Left + car[carCount - 1].Position.Width;
                        car[i].Position = s;
                        //Console.WriteLine("moved {0} to {1}.", i, car[i].Position);
                    }
                }

                if (direction == Direction.East)
                {
                    if (car[i].Position.Left > screen.Right)
                    {
                        if (i != 0)
                            s.X = car[i - 1].Position.Left - car[i - 1].Position.Width;
                        else
                            s.X = car[carCount - 1].Position.Left - car[carCount - 1].Position.Width;
                        car[i].Position = s;
                    }
                }
                //Console.WriteLine("Moving " + car[i].Position);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < carCount; i++)
                car[i].Draw(spriteBatch);

            //Console.WriteLine(car[0].Position);
        }

    }
}
