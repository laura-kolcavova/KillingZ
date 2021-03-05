using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KillingZ.GFX
{
    class Sprite
    {
        private Texture2D sheet;
        private Rectangle sourceRect;

        public Sprite(Texture2D sheet, int x, int y, int width, int height)
        {
            this.sheet = sheet;
            sourceRect = new Rectangle(x, y, width, height);
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sb.Draw(sheet, position, sourceRect, Color.White);
        }

        public void Draw(SpriteBatch sb, Rectangle destRect)
        {
            sb.Draw(sheet, destRect, sourceRect, Color.White);
        }
        
        public void Draw(SpriteBatch sb, Rectangle destRect, SpriteEffects effects)
        {
            sb.Draw(sheet, destRect, sourceRect, Color.White, 0f, new Vector2(0, 0), effects, 0f);
        }

        public int GetWidth()
        {
            return sourceRect.Width;
        }

        public int GetHeight()
        {
            return sourceRect.Height;
        }
    }
}
