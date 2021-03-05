using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace KillingZ.LWorld
{
    class World
    {
        private Handler handler;
        private int[,] tiles; //Ground
        public int width { get; private set; }
        public int height { get; private set; }
        public int widthPx { get; private set; }
        public int heightPx { get; private set; }
  
        public World(Handler handler, string path)
        {
            this.handler = handler;

            LoadWorld(path);
        }

        private void LoadWorld(string path)
        {
            string[] lines;

            //read file as string
            try
            {
                lines = File.ReadAllLines(path);

                width = int.Parse(lines[0]);
                height = int.Parse(lines[1]);
                widthPx = width * Tile.TILE_WIDTH;
                heightPx = height * Tile.TILE_HEIGHT;

                tiles = new int[height, width];

                for(int i = 2; i < lines.Length; i++)
                {
                    int y = i - 2;
                    string[] tokens = lines[i].Split(',');
  
                    for (int x = 0; x < width; x++)
                    {
                        tiles[y, x] = int.Parse(tokens[x]);
                    }
                }
                
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public Tile GetTile(int x, int y)
        {
            if(x < 0 || x >= width || y < 0 || y >= height)
            {
                return Tile.GetTile(35);
            }
            return Tile.GetTile(tiles[y, x]);
        }

        public int GetNearestBlockRow(int col)
        {
            int highestRow = 0;

            for(int i = 1; i < height; i++)
            {
                if (GetTile(col, i).block) break;
                highestRow++;
            }

            return highestRow;
        }

        public void Draw(SpriteBatch sb)
        {
            float cameraX = handler.GetCamera().GetX();
            float cameraY = handler.GetCamera().GetY();

            int xStart = (int)Math.Max(0, handler.GetCamera().GetX() / Tile.TILE_WIDTH);
            int yStart = (int)Math.Max(0, handler.GetCamera().GetY() / Tile.TILE_HEIGHT);
            int xEnd = (int)Math.Min(width, ((cameraX + handler.GetWidth()) / Tile.TILE_WIDTH) + 1);
            int yEnd = (int)Math.Min(height, ((cameraY + handler.GetHeight()) / Tile.TILE_HEIGHT) + 1); 

            for(int x = xStart; x < xEnd; x++)
            {
                for(int y = yStart; y < yEnd; y++)
                {
                   GetTile(x, y).Draw(sb, (int)(x * Tile.TILE_WIDTH - cameraX),
                                          (int)(y * Tile.TILE_HEIGHT - cameraY));
                }
            }
        }

    }
}
