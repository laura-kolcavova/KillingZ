using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using KillingZ.LWorld;

namespace KillingZ.LEntity
{
    abstract class Creature : Entity
    {
        public static Creature NOBODY = null;

        public bool alive { get; protected set; }

        protected int maxHealth;
        public int health { get; protected set; }
        public bool face_right { get; protected set; }
        protected float speed;

        protected float jump_height;
        protected float gravity;
        protected float fallspeed;
        protected bool falling;

        protected float nx, ny;
        protected float moveX, moveY;

        //Tile collision
        protected bool tc_topLeft;
        protected bool tc_topRight;
        protected bool tc_bottomLeft;
        protected bool tc_bottomRight;

        public Creature()
        {

        }

        public Creature(Handler handler, Vector2 position) : base(handler, position)
        {
            fallspeed = 0f;
            falling = false;
            alive = true;
        }

        public void Move()
        {
            Fall();

            nx = position.X + moveX;
            ny = position.Y + moveY;

            //Check tiles collision
            MoveY();
            MoveX();

            position = new Vector2(nx, ny);
        }

        private void Fall()
        {
            if (falling)
            {
                fallspeed += gravity;
                moveY = fallspeed;
            }
        }

        private void MoveX()
        {
            //ny - kvuli fallingu
            int tileTop = GetMapY(ny + bounds.Y);
            int tileBott = GetMapY(ny + bounds.Y + bounds.Height - 1);

            if (moveX > 0) //Right
            {
                int mx = GetMapX(nx + bounds.X + bounds.Width - 1);

                if (TileCollision(mx, tileTop) || TileCollision(mx, tileBott))
                {
                    nx = mx * Tile.TILE_WIDTH - bounds.X - bounds.Width;
                }
            }

            if (moveX < 0) //Left
            {
                int mx = GetMapX(nx + bounds.X);

                if (TileCollision(mx, tileTop) || TileCollision(mx, tileBott))
                {
                    nx = (mx + 1) * Tile.TILE_WIDTH + bounds.X;
                }
            }

        }

        private void MoveY()
        {
            int tileLeft = GetMapX(position.X + bounds.X);
            int tileRight = GetMapX(position.X + bounds.X + bounds.Width - 1);

            if (moveY > 0) //Down
            {
                int my = GetMapY(ny + bounds.Y + bounds.Height - 1);

                if (TileCollision(tileLeft, my) || TileCollision(tileRight, my))
                {
                    ny = my * Tile.TILE_HEIGHT - bounds.Y - bounds.Height;

                    if (falling)
                    {
                        fallspeed = 0f;
                        falling = false;
                    }
                }
            }

            if (moveY < 0) //Up
            {
                int my = GetMapY(ny + bounds.Y);

                if (TileCollision(tileLeft, my) || TileCollision(tileRight, my))
                {
                    ny = (my + 1) * Tile.TILE_HEIGHT + bounds.Y;

                    if (falling)
                    {
                        fallspeed = 0;
                    }
                }
            }

            if(!falling)
            {
                int my = GetMapY(ny + bounds.Y + bounds.Height);

                if(!TileCollision(tileLeft, my) && !TileCollision(tileRight, my))
                {
                    falling = true;
                }
            }
        }

        protected virtual void Jump()
        { 
            fallspeed = -jump_height;
            falling = true;
        }

    }
}
