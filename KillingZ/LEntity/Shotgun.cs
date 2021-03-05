using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KillingZ.LEntity
{
    class Shotgun : Gun
    {
        private bool fillingToFull;
        
        public Shotgun(Gun prototype, Handler handler, Vector2 position, Creature owner, bool face_right)
        : base(prototype, handler, position, owner, face_right)
        {
            fillingToFull = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (fillingToFull && !reloading) Reload();
        }

        public override void Fire(GameTime gameTime)
        {
            if (bulletsCount > 0)
            {
                fillingToFull = false;
                StopReload();
            }
            base.Fire(gameTime);
        }

        public override void Reload()
        {
            if (!reloading && !fire)
            {
                if (currentPlayerAmmo == 0) return;
                reload_sfx.Play();
                reloading = true;
                fillingToFull = true;
                timer = 0;
            }

            if (reloadingFinished)
            {
                reloading = false;
                reloadingFinished = false;

                int gotBullets = Math.Min(1, currentPlayerAmmo);
                bulletsCount += gotBullets;
                currentPlayerAmmo -= gotBullets;

                if(bulletsCount == Math.Min(maxBulletsInStack, currentPlayerAmmo))
                {
                    fillingToFull = false;
                }
            }
        }

    }
}
