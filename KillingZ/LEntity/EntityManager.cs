using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillingZ.LEntity
{
    class EntityManager
    {
        public Player player { get; private set; }

        private Handler handler;
        private List<Entity> entities;

        public EntityManager(Handler handler, Player player)
        {
            this.handler = handler;
            this.player = player;

            entities = new List<Entity>();
            Add(player);
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);

                if(!entities[i].visible)
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(sb);
            }
        }

        public void Add(Entity e)
        {
            entities.Add(e);
        }

        public Player GetPlayer()
        {
            return player;
        }

        public List<Entity> GetEntities()
        {
            return entities;
        }
    }
}
