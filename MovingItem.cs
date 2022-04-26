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
    /* A movingItem has: an image (Texture2D).
     * A location and size on the screen in pixels (a Rectangle).
     * A location (x, y).
     * A speed (pixels per tick). 
     * Positive speed moves right, negative moves left.
     * A Move method.
     */
    class MovingItem
    {
        private Texture2D texture;
        private Rectangle position;
        private Vector2 location; // precise to a fraction of a pixel 
        private float speed; // pixels per tick
        private bool visible;

        /// <summary>
        /// Create a moving item 
        /// </summary>
        /// <param name="texture">The image</param>
        /// <param name="position">The location and size (rectangle)</param>
        /// <param name="secondsToCross">Seconds to cross the screen width</param>
        /// <param name="speed"></param>
        public MovingItem(Texture2D texture, Rectangle position, float speed)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
            location.X = position.X;
            location.Y = position.Y;
            visible = true;
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle Position
        {
            get { return position; }
            set
            {
                position = value;
                location.X = position.X;
            }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public void Move()
        {
            location.X += speed;
            position.X = (int)location.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
