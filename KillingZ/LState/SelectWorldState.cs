using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KillingZ.GFX;
using KillingZ.LScreen;

namespace KillingZ.LState
{
    class SelectWorldState : State
    {
        private Texture2D background;
        private Rectangle backgrounRect;

        private Menu menu;
        private bool tokenOn;

        private string title;
        private Vector2 titlePos;

        public SelectWorldState(Handler handler):base(handler)
        {
            LoadContent(handler.GetContent());
            Init();
        }

        public override void Init()
        {
            string[] menuItems = { "World 1", "Mario", "World 3", "Plain" };
            menu = new Menu(menuItems, Menu.Dir.Vertical, new Vector2(370, 150),
                true, true, Color.Black, Color.DarkRed, Assets.Arial18);

            tokenOn = false;

            title = "";
            if(handler.GetGameType() == Game1.GameType.Campaign)
            {
                title = "Campaign";
            }
            else if(handler.GetGameType() == Game1.GameType.Classic)
            {
                title = "Classic";
            }
            else if (handler.GetGameType() == Game1.GameType.Survival)
            {
                title = "Survival";
            }

            titlePos = new Vector2(handler.GetWidth() / 2 - (Assets.Arial18.MeasureString(title).Length() / 2)
                , 20);
        }

        public override void LoadContent(ContentManager content)
        {
            background = Assets.t_canvasBackground;
            backgrounRect = new Rectangle(0, 0, handler.GetWidth(), handler.GetHeight());
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            if (!tokenOn)
            {
                if (keystate.IsKeyDown(Keys.Enter)) tokenOn = false;
                if (keystate.IsKeyUp(Keys.Enter)) tokenOn = true;
                return;
            }

            if (keystate.IsKeyDown(Keys.Escape))
            {
                handler.SetState(Game1.STATE_MENU);
            }

            menu.Update(gameTime);

            Select(menu.GetSelectedItem());
        }

        private void Select(int option)
        {
            if (option == -1) return;

            switch(option)
            {
                case 0: SelectWorld("world1"); break;
                case 1: SelectWorld("world2"); break;
                case 2: SelectWorld("World3"); break;
                case 3: SelectWorld("World4"); break;
            }
        }

        private void SelectWorld(string wordlName)
        {
            handler.SetWorldName(wordlName);
            handler.SetState(Game1.STATE_GAME);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(background, backgrounRect, Color.White);
            sb.DrawString(Assets.Arial18, title, titlePos, Color.Black);
            menu.Draw(sb);
        }
    }
}
