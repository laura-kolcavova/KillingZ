using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using KillingZ.LWorld;
using KillingZ.GFX;
using System.Diagnostics;

namespace KillingZ.LEntity
{
    abstract class Entity
    {
        protected Handler handler;

        public string name { get; protected set; }
        public Sprite texture { get; protected set; }
        public Vector2 position { get; protected set; }

        public int width { get; protected set; }
        public int height { get; protected set; }
        public bool visible { get; protected set; }

        protected Rectangle bounds;
        protected Rectangle destRect;

        //prototype
        public Entity()
        {

        }

        //entity constructor
        public Entity(Handler handler, Vector2 position)
        {
            this.handler = handler;

            this.position = position;

            visible = true;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch sb)
        {
            destRect = new Rectangle((int)(position.X - handler.GetCamera().GetX()),
                (int)(position.Y - handler.GetCamera().GetY()), width, height);
        }


        public bool Intersect(Entity e)
        {
            return getCollBounds().Intersects(e.getCollBounds());
        }

        private Rectangle getCollBounds()
        {
            return new Rectangle((int)position.X + bounds.X, (int)position.Y + bounds.Y, bounds.Width, bounds.Height);
        }

        protected int GetMapX(float x)
        {
            int n = (int)(x / Tile.TILE_WIDTH);
            if (x < 0 && n == 0) return -1;
            return n;
        }

        protected int GetMapY(float y)
        {
            int n = (int)(y / Tile.TILE_HEIGHT);
            if (y < 0 && n == 0) return -1;
            return n;
        }

        protected bool TileCollision(int x, int y)
        {
            return handler.GetWorld().GetTile(x, y).block;
        }

        public void SetVisible(bool state)
        {
            visible = state;
        }
    }
}
