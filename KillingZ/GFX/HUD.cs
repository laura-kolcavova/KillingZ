using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using KillingZ.LEntity;
using KillingZ.LState;

namespace KillingZ.GFX
{
    class HUD
    {
        private Handler handler;
        private Player player;

        private Vector2 gunNameTextPos;
        private Vector2 bulletTextPos;
        private Vector2 ammoTextPos;
        private Vector2 hpTextPos;
        private Vector2 cashTextPos;
        private Vector2 waveTextpos;
        private Vector2 deadZombiesTextPos;

        private bool waveDefeated;

        public HUD(Handler handler, Player player)
        {
            this.handler = handler;
            this.player = player;

            gunNameTextPos = new Vector2(10, handler.GetHeight() - 80);
            bulletTextPos = new Vector2(10, handler.GetHeight() - 50);
            ammoTextPos = new Vector2(40, handler.GetHeight() - 50);
            hpTextPos = new Vector2(handler.GetWidth() - 80, handler.GetHeight() - 50);
            cashTextPos = new Vector2(5, 5);
            waveTextpos = new Vector2(handler.GetWidth() / 2 - 50, 5);
            deadZombiesTextPos = new Vector2(handler.GetWidth() - 100, 5);

            waveDefeated = false;
        }

        public void Draw(SpriteBatch sb)
        {
            if (player.currentGun != Gun.EMPTY)
            {
                //gun
                sb.DrawString(Assets.Arial15, player.currentGun.name, gunNameTextPos, Color.Black);
                //Bullets
                sb.DrawString(Assets.Arial15, player.currentGun.bulletsCount.ToString(), bulletTextPos, Color.Black);
                sb.DrawString(Assets.Arial15, "/ " + player.currentGun.currentPlayerAmmo.ToString(), ammoTextPos, Color.Black);
            }

            //hp
            sb.DrawString(Assets.Arial15, "HP: " + player.health.ToString(), hpTextPos, Color.Black);
            

            //cash
            sb.DrawString(Assets.Arial15, "$" + player.cash.ToString(), cashTextPos, Color.Black);

            //waves
            if (!waveDefeated)
            {
                sb.DrawString(Assets.Arial15, "Wave: " + handler.GetZombieWaveManager().wave.ToString(), waveTextpos, Color.Black);
            }
            else
            {
                string text1 = "You survived WAVE " + handler.GetZombieWaveManager().wave;

                string text2 = "IT`S TIME TO GO SHOPPING";
                sb.DrawString(Assets.Arial18, text1, new Vector2(
                    GetCenteredX(text1, handler.GetWidth() / 2, Assets.Arial18), handler.GetHeight() / 6),
                    Color.Black);

                sb.DrawString(Assets.Arial18, text2, new Vector2(
                    GetCenteredX(text2, handler.GetWidth() / 2, Assets.Arial18), handler.GetHeight() / 4),
                    Color.DarkRed);
            }

            sb.DrawString(Assets.Arial15, (handler.GetZombieWaveManager().maxZombies - handler.GetZombieWaveManager().deadZombies).ToString()
                + "/" + handler.GetZombieWaveManager().maxZombies.ToString(),
                deadZombiesTextPos, Color.Black);
        }

        private float GetCenteredX(string text,float x, SpriteFont font)
        {
            return x - (float)(font.MeasureString(text).Length()) / 2;
        }

        public void SetWaveDefeated(bool state)
        {
            waveDefeated = state;
        }
    }
}
