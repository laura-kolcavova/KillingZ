using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using KillingZ.LEntity;
using KillingZ.GFX;

namespace KillingZ.LWorld
{
    class ZombieWaveManager
    {
        private Handler handler;

        public int maxWaves { get; private set; }
        public int wave { get; private set; }
        public int maxZombies { get; private set; }
        public int aliveZombies { get; private set; }
        public int deadZombies { get; private set; }
        public bool infinity { get; private set; }

        private enum Zombies
        {
            ZombieDog, Zombie1, Zombie2, Zombie3, Zombie4
        };

        private Zombies[] zombiePool;
        private float spawnTimer;
        private float spawnTime;

        public ZombieWaveManager(Handler handler, int maxWaves, bool infinity)
        {
            this.handler = handler;
            this.maxWaves = maxWaves;
            this.infinity = infinity;

            SetWave(1);
            spawnTimer = 0;
            spawnTime = 2000;
        }

        public void Update(GameTime gameTime)
        {
            if (aliveZombies + deadZombies < maxZombies)
            {
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (spawnTimer >= spawnTime)
                {
                    spawnTimer = 0;

                    Vector2 position = GetNewPosition();

                    int ranIndex = handler.GetRandom(0, zombiePool.Length - 1);
                    Zombies zombie = zombiePool[ranIndex];

                    AddZombie(zombie, position);
                }
            }

            /*//All zombies dead
            if(deadZombies == maxZombies)
            {
                if(!infinity && wave < maxWaves)
                {
                    SetWave(wave + 1);
                }
            }*/
        }

        public void SetWave(int wave)
        {
            this.wave = wave;

            deadZombies = aliveZombies = 0;

            if(wave == 1)
            {
                maxZombies = 20;

                zombiePool = new Zombies[] {
                    Zombies.Zombie1, Zombies.Zombie2
                };
            }
            else if(wave == 2)
            {
                maxZombies = 35;

                zombiePool = new Zombies[] {
                    Zombies.ZombieDog, Zombies.Zombie1, Zombies.Zombie2
                };
            }
            else if(wave == 3)
            {
                maxZombies = 52;

                zombiePool = new Zombies[] {
                    Zombies.ZombieDog, Zombies.Zombie1, Zombies.Zombie2, Zombies.Zombie3
                };
            }
            else if (wave == 4)
            {
                maxZombies = 84;

            }
            else if (wave == 5)
            {
                maxZombies = 105;

                zombiePool = new Zombies[] {
                    Zombies.ZombieDog, Zombies.Zombie1, Zombies.Zombie2, Zombies.Zombie3,
                    Zombies.Zombie4
                };
            }
            else if (wave == 6)
            {
                maxZombies = 125;
            }
            else if (wave == 7)
            {
                maxZombies = 145;
            }
            else if (wave == 8)
            {
                maxZombies = 160;
            }
            else if (wave == 9 || infinity)
            {
                maxZombies = 185;
            }
        }

        private Vector2 GetNewPosition()
        {
            float x;

            if (handler.GetCamera().GetX() <= 64)
            {
                x = handler.GetWidth()+ 32;
            }
            else if (handler.GetCamera().GetX() >= handler.GetWorld().widthPx - handler.GetWidth() - 64)
            {
                x = handler.GetCamera().GetX() - 32;
            }
            else
            {
                if(handler.GetRandom(0, 1) == 0)
                {
                    x = handler.GetCamera().GetX() - 32;
                }
                else
                {
                    x = handler.GetCamera().GetX() + handler.GetWidth() + 32;
                }
            }

            float y = handler.GetWorld().GetNearestBlockRow((int)(x / Tile.TILE_WIDTH)) * Tile.TILE_HEIGHT;

            return new Vector2(x, y);
        }

        private void AddZombie(Zombies zombie, Vector2 position)
        {
            aliveZombies++;

            switch(zombie)
            {
                case Zombies.ZombieDog:
                    handler.GetEntityManager().Add(new ZombieDog(Assets.zombieDog, handler, position));
                    break;

                case Zombies.Zombie1:
                    handler.GetEntityManager().Add(new Zombie(Assets.zombie1, handler, position));
                    break;

                case Zombies.Zombie2:
                    handler.GetEntityManager().Add(new Zombie(Assets.zombie2, handler, position));
                    break;

                case Zombies.Zombie3:
                    handler.GetEntityManager().Add(new Zombie(Assets.zombie3, handler, position));
                    break;

                case Zombies.Zombie4:
                    handler.GetEntityManager().Add(new Zombie(Assets.zombie4, handler, position));
                    break;
            }
        }

        public void DeadZombies(int deadZombies)
        {
            this.deadZombies += deadZombies;
            this.aliveZombies -= deadZombies;
        }
    }
}
