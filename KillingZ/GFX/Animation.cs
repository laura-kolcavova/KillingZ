using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KillingZ.GFX
{
    class Animation
    {
        private Sprite[] sprites;
        private float speed;
        private int index,timer;
        private bool active, looping;
        private bool faceLeft;

        private Rectangle a_DestRect;
        private Handler handler;

        public Animation(Sprite[] sprites, float speed, bool looping)
        {
            this.sprites = sprites;
            this.speed = speed;
            this.looping = looping;

            active = true;

            index = 0;
            timer = 0;
        }

        public Animation(Animation prototype, Handler handler, bool faceLeft)
        {
            this.sprites = prototype.sprites;
            this.speed = prototype.speed;
            this.looping = prototype.looping;
            this.handler = handler;
            this.faceLeft = faceLeft;
            active = true;
            index = 0;
            timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!active) return;

            timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(timer > speed)
            {
                index++;

                if (index == sprites.Length)
                {
                    index = 0;
                    if (!looping) active = false;
                }

                timer = 0;
            }
        }

        public bool IsActive() { return active; }

        public void SetActive(bool state)
        {
            active = state;
        }

        public void SetRectangle(Rectangle rect)
        {
            a_DestRect = rect;
        }

        public Sprite GetSprite()
        {
            return sprites[index];
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle destRect = new Rectangle((int)(a_DestRect.X - handler.GetCamera().GetX()),
                (int)(a_DestRect.Y - handler.GetCamera().GetY()), a_DestRect.Width, a_DestRect.Height);

            if(faceLeft)
            {
                sprites[index].Draw(sb, destRect, SpriteEffects.FlipHorizontally);
            }
            else
            {
                sprites[index].Draw(sb, destRect, SpriteEffects.None);
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sprites[index].Draw(sb, position);
        }

        public void Draw(SpriteBatch sb, Rectangle destRect)
        {
            sprites[index].Draw(sb, destRect);
        }
    }
}
