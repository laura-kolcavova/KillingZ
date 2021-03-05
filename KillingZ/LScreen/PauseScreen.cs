using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KillingZ.GFX;
using KillingZ.LState;

namespace KillingZ.LScreen
{
    class PauseScreen : Screen
    {
        private Texture2D screen;
        private Rectangle screenRect;
        private Menu menu;

        public PauseScreen(Handler handler) : base(handler)
        {
            screen = new Texture2D(handler.GetGraphicsDevice(), 1, 1);
            Color color = new Color(Color.Black, 0.7f);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            screen.SetData(data);
            screenRect = new Rectangle(0, 0, (int)handler.GetWidth(), (int)handler.GetHeight());

            string[] items = { "Resume", "Quit" };
            menu = new Menu(items, Menu.Dir.Vertical, new Vector2(370, 150),
                true, true, Color.Orange, Color.DarkRed, Assets.Arial18);
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
            Select(menu.GetSelectedItem());
        }

        private void Select(int option)
        {
            switch(option)
            {
                case 0:
                    ((GameState)handler.GetState()).SetScreen(GameState.Screens.Game);
                    break;

                case 1: handler.SetState(Game1.STATE_MENU); break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(screen, screenRect, Color.White);
            menu.Draw(sb);
        }
    }
}
