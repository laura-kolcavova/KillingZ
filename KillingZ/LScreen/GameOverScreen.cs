using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using KillingZ.GFX;

namespace KillingZ.LScreen
{
    class GameOverScreen : Screen
    {
        private string text;
        private Vector2 textPos;
        private Texture2D screen;
        private Rectangle screenRect;

        private string killedZombiesText;
        private Vector2 killedZombiesTextPos;

        public GameOverScreen(Handler handler) : base(handler)
        {
            screen = new Texture2D(handler.GetGraphicsDevice(), 1, 1);
            Color color = new Color(Color.Black, 0.7f);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            screen.SetData(data);
            screenRect = new Rectangle(0, 0, (int)handler.GetWidth(), (int)handler.GetHeight());
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void SetData(bool win, int killedZombies)
        {
            text = win ? "YOU SURVIVED!" : "GAME OVER";
            textPos = new Vector2(handler.GetWidth() / 2 - Assets.Arial18.MeasureString(text).Length() / 2,
                handler.GetHeight() / 3);

            killedZombiesText = "Killed zombies: " + killedZombies;
            killedZombiesTextPos = new Vector2(handler.GetWidth() / 2 - Assets.Arial15.MeasureString(killedZombiesText).Length() / 2,
                handler.GetHeight() / 2);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(screen, screenRect, Color.White);
            sb.DrawString(Assets.Arial18, text, textPos, Color.DarkOrange);
            sb.DrawString(Assets.Arial15, killedZombiesText, killedZombiesTextPos, Color.Orange);
        }
    }
}
