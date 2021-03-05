using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KillingZ.LScreen
{
    class Menu
    {
        public enum Dir { Horizontal, Vertical };

        private string[] items;
        private Dir direction;
        private Vector2 position;
        private bool visible;
        private bool active;
        private Color defaultColor;
        private Color selectedColor;
        private SpriteFont font;

        private int currentItem;
        private float timer;

        private bool k_enter;

        private Keys key1;
        private Keys key2;

        public Menu(string[] items, Dir direction, Vector2 position, bool visible, bool active, Color defaultColor, Color selectedColor, SpriteFont font)
        {
            this.items = items;
            this.direction = direction;
            this.position = position;
            this.visible = visible;
            this.active = active;
            this.defaultColor = defaultColor;
            this.selectedColor = selectedColor;
            this.font = font;

            currentItem = 0;
            timer = 0;
            k_enter = true;

            if(direction == Dir.Horizontal)
            {
                key1 = Keys.Left;
                key2 = Keys.Right;
            }
            else if (direction == Dir.Vertical)
            {
                key1 = Keys.Up;
                key2 = Keys.Down;
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            if (!active) return;

            if (timer == 0)
            {
                if (keystate.IsKeyDown(key1) || keystate.IsKeyDown(key2))
                {
                    timer = 100;
                }

                if (keystate.IsKeyDown(key1))
                {
                    if (currentItem == 0) currentItem = items.Length - 1;
                    else currentItem--;
                }
                else if (keystate.IsKeyDown(key2))
                {
                    if (currentItem == items.Length - 1) currentItem = 0;
                    else currentItem++;
                }
                else if (keystate.IsKeyDown(Keys.Enter))
                {
                    k_enter = true;
                }
                else if (keystate.IsKeyUp(Keys.Enter))
                {
                    k_enter = false;
                }
            }
            else
            {
                timer -= gameTime.ElapsedGameTime.Milliseconds;

                if (timer <= 0)
                {
                    timer = 0;
                }

                if (keystate.IsKeyUp(key1) && keystate.IsKeyUp(key2))
                {
                    timer = 0;
                }
            }
        }

        public int GetSelectedItem()
        {
            if (k_enter) return currentItem;
            else return -1;
        }

        public int GetCurrentItem()
        {
            return currentItem;
        }

        public void Draw(SpriteBatch sb)
        {
            if (!visible) return;

            if(direction == Dir.Vertical)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Color color = i == currentItem ? selectedColor : defaultColor;

                    Vector2 pos = Vector2.Zero;

                    if(direction == Dir.Vertical)
                    {
                        pos = new Vector2(position.X, position.Y + (i * 50));
                    }
                    else if(direction == Dir.Horizontal)
                    {
                        pos = new Vector2(position.X + i * 160, position.Y);
                    }

                    sb.DrawString(font, items[i], pos, color);
                }
            }
        }

    }

}
