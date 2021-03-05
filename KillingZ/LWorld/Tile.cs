using KillingZ.GFX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KillingZ.LWorld
{
    class Tile
    {
        private static Tile[] tiles;

        public static int TILE_WIDTH { get { return 32; } }
        public static int TILE_HEIGHT { get { return 32; } }
       
        public static void InitTiles()
        {
            tiles = new Tile[255];

            new Tile(Assets.tile_empty, false, 1);
            new Tile(Assets.tile_sand, true, 35);
            new Tile(Assets.tile_stone, true, 68);
            new Tile(Assets.tile_upGrass_sand, true, 92);
            new Tile(Assets.tile_leftGrass_sand, true, 112);
            new Tile(Assets.tile_rightGrass_sand, true, 114);
            new Tile(Assets.tile_downGrass_sand, true, 134);
            new Tile(Assets.tile_sand_leftUpGrass, false, 137);
            new Tile(Assets.tile_sand_rightUpGrass, false, 136);
            new Tile(Assets.tile_sand_leftDownGrass, false, 115);
            new Tile(Assets.tile_sand_rightDownGrass, false, 116);
        }

        public static Tile GetTile(int id)
        {
            return tiles[id];
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// </summary>
        private Sprite sprite;
        public int id { get; private set; }
        public bool block { get; private set; }

        private Tile(Sprite sprite, bool block, int id)
        {
            this.sprite = sprite;
            this.block = block;
            this.id = id;

            tiles[id] = this;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch sb, int x, int y)
        {
            if (id == 0) return;
            sprite.Draw(sb, new Rectangle(x, y, TILE_WIDTH, TILE_HEIGHT));
        }
    }
}
