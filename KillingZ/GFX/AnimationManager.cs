using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillingZ.GFX
{
    class AnimationManager
    {
        private List<Animation> animations;
        private Handler handler;

        public AnimationManager(Handler handler)
        {
            this.handler = handler;
            animations = new List<Animation>();
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < animations.Count; i++)
            {
                animations[i].Update(gameTime);

                if(!animations[i].IsActive())
                {
                    animations.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < animations.Count; i++)
            {
                animations[i].Draw(sb);
            }
        }

        public void Add(Animation animation)
        {
            animations.Add(animation);
        }

    }
}
