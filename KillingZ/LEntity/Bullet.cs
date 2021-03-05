using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using KillingZ.GFX;

namespace KillingZ.LEntity
{
    class Bullet : Entity
    {
        public static int DEFAULT_WIDTH = 10;
        public static int DEFAULT_HEIGHT = 1;
        public Gun.Weights weight;

        public int damage { get; private set; }

        public Vector2 forward { get; private set; }
        private float speed;

        public Bullet(Handler handler, Vector2 position, Vector2 forward, int damage, float speed, Gun.Weights weight): base(handler, position)
        {
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;

            this.forward = forward;
            this.speed = speed;
            this.damage = damage;
            this.weight = weight;

            bounds = new Rectangle(0, 0, width, height);

            texture = Assets.s_bullet;
        }

        public override void Update(GameTime gameTime)
        {
            float nx = position.X;
            float ny = position.Y;

            nx += speed * forward.X;
            ny += speed * forward.Y;

            if(nx > handler.GetWorld().widthPx || nx + width < 0 || ny > handler.GetWorld().heightPx || ny + height < 0)
            {
                visible = false;
            }

            if (CollisionWithTile(nx, ny)) visible = false;

            position = new Vector2(nx, ny);
        }

        private bool CollisionWithTile(float x, float y)
        {
            int topTile = GetMapY(y + bounds.Y);
            int bottomTile = GetMapY(y + bounds.Y + bounds.Height - 1);
            int mx = 0;

            if(forward.X > 0) //Right
            {
                mx = GetMapX(x + bounds.X + bounds.Width - 1);
            }

            if(forward.X < 0) //Left
            {
                mx = GetMapX(x + bounds.X);
            }

            return TileCollision(mx, topTile) || TileCollision(mx, bottomTile);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            texture.Draw(sb, destRect);
        }
    }
}
