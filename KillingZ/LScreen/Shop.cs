using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using KillingZ.LEntity;
using KillingZ.GFX;
using System.Diagnostics;

namespace KillingZ.LScreen
{
    class Shop : Screen
    {
        private static ISellable EMPTY = null;

        private Texture2D background;
        private Rectangle backgroundRect;

        private Texture2D screen;
        private Rectangle screenRect;

        private ISellable[] playerGear;
        private ISellable[] tier1;
        private ISellable[] tier2;
        private ISellable[] tier3;

        private string[] menu;
        private string[][] lists;

        private int mainCurrentOption;
        private int listCurrentOption;
        private float timer;

        private ISellable currentItem;

        private Player customer;
        
        private enum Action { Buy, Sell, Nothing };
        private Action action;

        //action text
        private Vector2 actionTextPos;
        private string actionText;

        private string customerCashText;
        private Vector2 customerCashTextPos;

        public Shop(Handler handler) : base(handler)
        {
            LoadContent();
            Init();
        }

        private void LoadContent()
        {
            background = Assets.t_canvasBackground;
            backgroundRect = new Rectangle(0, 0, handler.GetWidth(), handler.GetHeight());

            screen = new Texture2D(handler.GetGraphicsDevice(), 1, 1);
            Color color = new Color(Color.Black, 0.6f);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; i++) data[i] = color;
            screen.SetData(data);
            screenRect = new Rectangle(0, 0, (int)handler.GetWidth(), (int)handler.GetHeight());
        }

        private void Init()
        {
            //Guns
            tier1 = new ISellable[]
            {
                Assets.g_SIG, Assets.g_MAGNUM, Assets.g_COLT
            };

            tier2 = new ISellable[]
            {
                Assets.g_SG553, Assets.g_P90, Assets.g_UZI, Assets.g_VECTOR,
                Assets.g_SHOTGUN_SHORT, Assets.g_SHOTGUN_LONG
            };

            tier3 = new ISellable[]
            {
                Assets.g_G36, Assets.g_M4A1, Assets.g_AK47, Assets.g_SCAR, Assets.g_FN_FAL,
                Assets.g_RIFLE, Assets.g_SNIPER, Assets.g_DRAGUNOV, Assets.g_DSR
            };

            //Menu
            menu = new string[] { "Weapons", "Tier 1", "Tier 2", "Tier 3" };

            lists = new string[4][];

            lists[0] = new string[0];
            lists[1] = LoadListItemsFromItems(tier1);
            lists[2] = LoadListItemsFromItems(tier2);
            lists[3] = LoadListItemsFromItems(tier3);

            mainCurrentOption = 0;
            listCurrentOption = 0;
            timer = 0;

            action = Action.Sell;
            actionTextPos = new Vector2(40, 300);
            actionText = "";

            customerCashTextPos = new Vector2(40, handler.GetHeight() - 50);
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (timer == 0)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.Right) ||
                    keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.Down))
                {
                    timer = 100;
                }

                if(keyState.IsKeyDown(Keys.Left))
                {
                    if (mainCurrentOption == 0) mainCurrentOption = menu.Length - 1;
                    else mainCurrentOption--;

                    listCurrentOption = 0;
                }
                else if(keyState.IsKeyDown(Keys.Right))
                {
                    if (mainCurrentOption == menu.Length - 1) mainCurrentOption = 0;
                    else mainCurrentOption++;

                    listCurrentOption = 0;
                }
                else if(keyState.IsKeyDown(Keys.Up))
                {
                    if(listCurrentOption == 0)
                    {
                        listCurrentOption = lists[mainCurrentOption].Length - 1;
                    }
                    else listCurrentOption--;
                }
                else if(keyState.IsKeyDown(Keys.Down))
                {
                    if (listCurrentOption == lists[mainCurrentOption].Length - 1)
                    {
                        listCurrentOption = 0;
                    }
                    else listCurrentOption++;
                }
                else if(keyState.IsKeyDown(Keys.Enter))
                {
                    DoAction();
                }


                currentItem = GetCurrentItem();

                if (currentItem == EMPTY)
                {
                    action = Action.Nothing;
                    actionText = "";
                }
                else if (mainCurrentOption == 0)
                {
                    action = Action.Sell;
                    actionText = "Sell: " + currentItem.cashValue / 2 + "$";
                }
                else
                {
                    action = Action.Buy;
                    actionText = "Buy: " + currentItem.cashValue + "$";
                }
            }
            else
            {
                timer -= gameTime.ElapsedGameTime.Milliseconds;

                if (timer < 0) timer = 0;

                if (keyState.IsKeyUp(Keys.Left) && keyState.IsKeyUp(Keys.Right) &&
                    keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down))
                {
                    timer = 0;
                }
            }
        }

        private string[] LoadListItemsFromItems(ISellable[] items)
        {
            string[] listItems = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == EMPTY) listItems[i] = "EMPTY";
                else listItems[i] = ((Entity)items[i]).name;
            }

            return listItems;
        }

        private ISellable GetCurrentItem()
        {
            switch(mainCurrentOption)
            {
                case 0: return playerGear[listCurrentOption];
                case 1: return tier1[listCurrentOption];
                case 2: return tier2[listCurrentOption];
                case 3: return tier3[listCurrentOption];
                default: return EMPTY;
            }
        }

        private ISellable GetItemAt(int menu, int item)
        {
            switch (menu)
            {
                case 0: return playerGear[item];
                case 1: return tier1[item];
                case 2: return tier2[item];
                case 3: return tier3[item];
                default: return EMPTY;
            }

        }

        public void SetCustomer(Player customer)
        {
            this.customer = customer;
            RefreshMenu();
            currentItem = GetCurrentItem();
        }

        private void RefreshMenu()
        {
            //set guns and menu
            playerGear = customer.guns;
            lists[0] = LoadListItemsFromItems(playerGear);
            customerCashText = customer.cash + "$";
        }

        private void DoAction()
        {
            if(action == Action.Buy)
            {
                if (!CanBeBought(currentItem)) return;

                customer.AddCash(-currentItem.cashValue);

                if (currentItem is Gun)
                {
                    Gun gunToSell = Gun.CreateGun((Gun)currentItem, handler, customer.position, customer, customer.face_right);

                    customer.GetGun(gunToSell);
                }
            }
            else if(action == Action.Sell)
            {
                customer.AddCash(currentItem.cashValue / 2);
                customer.RemoveGun((int)((Gun)currentItem).tier);
            }

            RefreshMenu();
        }

        private bool CanBeBought(ISellable item)
        {
            if (item == EMPTY) return false;

            //item in gear of player
            bool itemInGear = false;

            if (item is Gun)
            {
                if (playerGear[((int)((Gun)item).tier)] != EMPTY) itemInGear = true;
            }

            if (itemInGear)
            {
                for (int i = 0; i < playerGear.Length; i++)
                {
                    if (playerGear[i] == EMPTY) continue;

                    if (((Entity)playerGear[i]).name == ((Entity)item).name) itemInGear = true;
                }
            }

            return customer.cash >= item.cashValue && itemInGear == false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(background, backgroundRect, Color.White);
            sb.Draw(screen, screenRect, Color.White);

            //Draw menu
            for(int i = 0; i < menu.Length; i++)
            {
                Vector2 pos = new Vector2(40 + i * 160, 20);
                Color color;

                if (mainCurrentOption == i) color = Color.Red;
                else color = Color.Orange;

                sb.DrawString(Assets.Arial18, menu[i], pos, color);
            }

            //Draw list of the menu
            DrawListItems(sb, lists[mainCurrentOption], 40 + mainCurrentOption * 160, true);
            if (mainCurrentOption != 0)
            {
                DrawListItems(sb, lists[0], 40, false);
            }

            //Draw gun image
            if (action != Action.Nothing)
            {
                Sprite gunSprite = ((Entity)currentItem).texture;
                Rectangle gunDestRect = new Rectangle(40, 240,
                    (int)(gunSprite.GetWidth() * 1.3), (int)(gunSprite.GetHeight() * 1.3));
                gunSprite.Draw(sb, gunDestRect);
            }

            //Draw action
            sb.DrawString(Assets.Arial15, actionText, actionTextPos, Color.Orange);

            //Draw money
            sb.DrawString(Assets.Arial15, customerCashText, customerCashTextPos, Color.Orange);
        }

        private void DrawListItems(SpriteBatch sb, string[] list, float x, bool active)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Vector2 pos = new Vector2(x, 70 + i * 40);
                Color color = Color.Orange;

                if (listCurrentOption == i && active) color = Color.Red;
                else
                {
                    if(action == Action.Buy && active)
                    {
                        if(!CanBeBought(GetItemAt(mainCurrentOption, i))) color = Color.DarkGray;
                    }
                }


                sb.DrawString(Assets.Arial15, list[i], pos, color);
            }
        }

    }
}
