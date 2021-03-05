using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using KillingZ.LEntity;
using KillingZ.GFX;
using KillingZ.LWorld;
using KillingZ.LScreen;


namespace KillingZ.LState
{
    class GameState : State
    {
        //screens
        public enum Screens
        {
            Game, Pause, Shoping, GameOver
        };
        private Screens currentScreen;

        //game
        private World world;
        private ZombieWaveManager zombieWaveManager;
        private EntityManager entityManager;
        private AnimationManager animManager;
        private GameCamera gameCamera;
        private HUD hud;

        //screens
        private Screen pauseScreen;
        private Screen gameOverScreen;
        private Screen shop;

        private bool pauseTokenOn;

        private bool k_escape;

        private bool waited;
        private bool waiting;
        private float waitTime;
        private float waitTimer;

        public GameState(Handler handler): base(handler)
        {
            LoadContent(handler.GetContent());
            Init();
        }

        public override void LoadContent(ContentManager content)
        {
            Assets.LoadGameContent();
        }

        public override void UnloadContent()
        {
            Assets.UnloadContent();
        }

        public override void Init()
        {
            Tile.InitTiles();

            currentScreen = Screens.Game;

            world = new World(handler, @"Content/Worlds/" + handler.GetWorldName() + ".txt");
            handler.SetWorld(world);

            bool infinity = false;
            if (handler.GetGameType() == Game1.GameType.Survival) infinity = true;

            zombieWaveManager = new ZombieWaveManager(handler, 9, infinity);
            handler.SetZombieWaveManager(zombieWaveManager);
            handler.SetKilledZombies(0);

            Player player = new Player(handler, handler.GetPlayerType(), new Vector2(600, 50));

            entityManager = new EntityManager(handler, player);
            handler.SetEntityManager(entityManager);

            animManager = new AnimationManager(handler);
            handler.SetAnimationManager(animManager);

            gameCamera = new GameCamera(handler, new Vector2(0, 0));
            handler.SetGameCamera(gameCamera);

            hud = new HUD(handler, player);

            pauseScreen = new PauseScreen(handler);
            gameOverScreen = new GameOverScreen(handler);
            shop = new Shop(handler);

            pauseTokenOn = true;

            waited = false;
            waiting = false;
            waitTimer = 0;
            waitTime = 5000;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.Escape)) k_escape = true;
            if (keystate.IsKeyUp(Keys.Escape)) k_escape = false;

            if (currentScreen == Screens.Game)
            {
                if(waiting)
                {
                    waitTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (waitTimer > waitTime)
                    {
                        waitTimer = 0;
                        waiting = false;
                        waited = true;
                    }
                }    
                        
                //game
                world.Update(gameTime);

                zombieWaveManager.Update(gameTime);

                entityManager.Update(gameTime);
                animManager.Update(gameTime);

                //dead player
                if (!entityManager.GetPlayer().alive)
                {
                    ((GameOverScreen)gameOverScreen).SetData(false, handler.GetKilledZombies());
                    currentScreen = Screens.GameOver;
                }
                
                
                if(zombieWaveManager.deadZombies == zombieWaveManager.maxZombies)
                {
                    //win game
                    if(!zombieWaveManager.infinity && zombieWaveManager.wave == zombieWaveManager.maxWaves)
                    {
                        ((GameOverScreen)gameOverScreen).SetData(true, handler.GetKilledZombies());
                        currentScreen = Screens.GameOver;
                    }
                    else //wave dead - time to go shoping
                    {
                        if(!waited)
                        {
                            waiting = true;
                            hud.SetWaveDefeated(true);
                        }
                        else
                        {
                            waited = false;
                            currentScreen = Screens.Shoping;
                            ((Shop)shop).SetCustomer(entityManager.GetPlayer());

                            zombieWaveManager.SetWave(zombieWaveManager.wave + 1);
                            hud.SetWaveDefeated(false);
                        }
                    }
                }

                //pause
                CheckForPause(k_escape);
            }
            else if (currentScreen == Screens.Pause)
            {
                CheckForPause(k_escape);

                pauseScreen.Update(gameTime);
            }
            else if (currentScreen == Screens.Shoping)
            {
                if(k_escape)
                {
                    currentScreen = Screens.Game;
                    waited = false;
                }

                shop.Update(gameTime);
            }
            else if (currentScreen == Screens.GameOver)
            {
                if (keystate.IsKeyDown(Keys.Enter))
                {
                    handler.SetState(Game1.STATE_MENU);
                }
            }


        }

        private void CheckForPause(bool k_escape)
        {
            if (currentScreen == Screens.Pause)
            {
                if (k_escape)
                {
                    if (pauseTokenOn)
                    {
                        currentScreen = Screens.Game;
                        pauseTokenOn = false;
                    }
                }
                else
                {
                    pauseTokenOn = true;
                }
            }
            else if(currentScreen == Screens.Game)
            {
                if (k_escape)
                {
                    if (pauseTokenOn)
                    {
                        currentScreen = Screens.Pause;
                        pauseTokenOn = false;
                    }
                }
                else
                {
                    pauseTokenOn = true;
                }
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            
            if(currentScreen == Screens.Game)
            {
                world.Draw(sb);
                entityManager.Draw(sb);
                animManager.Draw(sb);
                hud.Draw(sb);
            }
            else if (currentScreen == Screens.Pause)
            {
                world.Draw(sb);
                entityManager.Draw(sb);
                animManager.Draw(sb);
                hud.Draw(sb);

                pauseScreen.Draw(sb);
            }
            else if (currentScreen == Screens.Shoping)
            {
                shop.Draw(sb);
            }
            else if (currentScreen == Screens.GameOver)
            {
                world.Draw(sb);
                entityManager.Draw(sb);
                animManager.Draw(sb);
                hud.Draw(sb);

                gameOverScreen.Draw(sb);
            }
        }

        public void SetScreen(Screens screen)
        {
            currentScreen = screen;
        }

    }
}
