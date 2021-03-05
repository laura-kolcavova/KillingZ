using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KillingZ.LEntity
{
    class ZombieDog : Zombie
    {
        private float sp_jumpTimer; //special jump timer
        private float nextJumpTime;

        public ZombieDog(Zombie prototype, Handler handler, Vector2 position): base(prototype, handler, position)
        {
            width = 40;
            height = 32;
            bounds = new Rectangle(0, 1, width, height - 2);

            sp_jumpTimer = 0;
            jump_height = 10;

            SetNextJumpTime();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //jumping
            if (!falling)
            {
                sp_jumpTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (sp_jumpTimer >= nextJumpTime)
                {
                    sp_jumpTimer = 0;
                    Jump();
                    SetNextJumpTime();
                }
            }
        }

        private void SetNextJumpTime()
        {
            nextJumpTime = handler.GetRandom(10, 30) * 100;
        }
    }
}
