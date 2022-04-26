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

    class Frogger
    {

        /********************* Private variables *******************/
        //4 Private Variables
        private Texture2D frogTexture;
        private Rectangle frogPosition;
        private bool isVisible;
        private Rectangle Screen;

        /********************* Constructor ********************** */
        /// <summary>
        /// Creates a frog
        /// </summary>
        /// <param name="theImage">The frog image. </param>
        /// <param name="thePosition">The frog's position. </param>
        /// <param name="isVisible">Whether to draw the frog or not. </param>
        /// <param name="theScreen">The bounds within which the frog must remain.</param>
        public Frogger(Texture2D theImage, Rectangle thePosition, bool isVisible, Rectangle theScreen)
        {
            frogTexture = theImage;
            frogPosition = thePosition;
            Screen = theScreen;
            isVisible = true;
        }

        /********************* Property methods ********************/
        public Texture2D theImage
        {
            get { return frogTexture; }
            set { frogTexture = value; }
        }

        public Rectangle thePosition
        {
            get { return frogPosition; }
            set
            {
                frogPosition = value;
            }
        }

        public bool Visible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }



        /********************** Methods ****************************/
        /// <summary>
        /// Move in the given direction, WITHOUT going off of the screen.
        /// </summary>
        public void Jump(Direction frogJump)
        {
            //Move Up
            if (frogJump == Direction.North)
            {
                frogPosition.Y = frogPosition.Y - frogPosition.Height;
            }

            //Keep Frog From Moving Past the Top
            if (frogPosition.Top < Screen.Top)
            {
                frogPosition.Y = Screen.Top;
            }

            //Move Down
            if (frogJump == Direction.South)
            {
                frogPosition.Y = frogPosition.Y + frogPosition.Height;
            }

            //Keep Frog From Moving Past the Bottom
            if (frogPosition.Bottom > Screen.Bottom)
            {
                frogPosition.Y = Screen.Bottom - frogPosition.Height;
            }

            //Move Right
            if (frogJump == Direction.East)
            {
                frogPosition.X = frogPosition.X + frogPosition.Width;
            }

            //Keep Frog From Moving Past the Right
            if (frogPosition.Right > Screen.Right)
            {
                frogPosition.X = Screen.Right - frogPosition.Width;
            }

            //Move Left
            if (frogJump == Direction.West)
            {
                frogPosition.X = frogPosition.X - frogPosition.Width;
            }

            //Keep Frog From Moving Past the Left
            if (frogPosition.X < Screen.Left)
            {
                frogPosition.X = Screen.Left;
            }
        }


        /// <summary>
        /// Return true if the frog is in the top row of the screen.
        /// </summary>
        /// <returns></returns>
        //public bool OnTopRow()
        //{
        //    if (frogPosition.Y == Screen.Top)
        //        return true;
        //}

        /// <summary>
        /// Draw the frog using the spriteBatch, IF the frog is visible.
        /// </summary>
        /// <param name="spriteBatch"></param>
        // Pass in the spriteBatch object so the sprite can draw itself.
        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(frogTexture, frogPosition, Color.White);
        }
    }
}
