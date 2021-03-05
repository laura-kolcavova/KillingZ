using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using KillingZ.GFX;
using KillingZ.LScreen;
using KillingZ.LEntity;

namespace KillingZ.LState
{
    class MenuState : State
    {
        private Texture2D bgrImage;
        private Rectangle bgrImageRect;

        private SpriteFont titleFont;

        private string title;
        private Vector2 titlePos;

        private Menu menu;
        private bool tokenOn;

        private Menu playerTypeMenu;
        private Sprite[] playerSprites;
        private int playerIndex;


        public MenuState(Handler handler): base(handler)
        {
            LoadContent(handler.GetContent());
            Init();
        }

        public override void Init()
        {
            bgrImageRect = new Rectangle(0, 0, handler.GetWidth(), handler.GetHeight());

            title = "Killing Z";
            titlePos = new Vector2(50, 50);

            string[] items = { "Campaign", "Classic", "Survival", "Exit" };
            menu = new Menu(items, Menu.Dir.Vertical, new Vector2(600, 150),
                true, true, Color.Black, Color.DarkRed, Assets.Arial18);

            tokenOn = false;
            handler.SetPlayerType(Player.PlayerType.Type1);

            playerIndex = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            bgrImage = Assets.t_canvasBackground;

            titleFont = content.Load<SpriteFont>("Fonts/TitleFont");

            Texture2D wizard_sheet = content.Load<Texture2D>("Textures/player_sheet");
            Texture2D dwarf_sheet = content.Load<Texture2D>("Textures/dwarf_sheet");
            Texture2D spacece_sheet = content.Load<Texture2D>("Textures/spacece_sheet");

            playerSprites = new Sprite[]
            {
                new Sprite(wizard_sheet, 0, 0, 32, 32),
                new Sprite(dwarf_sheet, 32, 128, 32, 32),
                new Sprite(spacece_sheet, 320, 0, 32, 32)
            };

            playerTypeMenu = new Menu(new string[playerSprites.Length], Menu.Dir.Horizontal, Vector2.Zero,
                false, true, Color.Black, Color.Black, Assets.Arial13);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            if(!tokenOn)
            {
                if (keystate.IsKeyDown(Keys.Enter)) tokenOn = false;
                if (keystate.IsKeyUp(Keys.Enter)) tokenOn = true;
                return;
            }

            menu.Update(gameTime);
            Select(menu.GetSelectedItem());

            playerTypeMenu.Update(gameTime);
            playerIndex = playerTypeMenu.GetCurrentItem();
        }

        private void Select(int option)
        {
            switch(option)
            {
                case 0:
                    handler.SetGameType(Game1.GameType.Campaign);
                    handler.SetState(Game1.STATE_SELECT_WORLD);

                    handler.SetPlayerType((Player.PlayerType)playerIndex);
                    break;
                case 1:
                    handler.SetGameType(Game1.GameType.Classic);
                    handler.SetState(Game1.STATE_SELECT_WORLD);

                    handler.SetPlayerType((Player.PlayerType)playerIndex);
                    break;
                case 2:
                    handler.SetGameType(Game1.GameType.Survival);
                    handler.SetState(Game1.STATE_SELECT_WORLD);

                    handler.SetPlayerType((Player.PlayerType)playerIndex);
                    break;
                case 3: Environment.Exit(0); break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(bgrImage, bgrImageRect, Color.White);

            sb.DrawString(titleFont, title, titlePos, Color.DarkRed);

            playerSprites[playerIndex].Draw(sb, new Rectangle(280, 280, 40, 40));

            menu.Draw(sb);
        }
    }
}
