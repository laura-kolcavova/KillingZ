using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillingZ.LScreen
{
    abstract class Screen
    {
        protected Handler handler;

        public Screen(Handler handler)
        {
            this.handler = handler;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch sb);
    }
}
