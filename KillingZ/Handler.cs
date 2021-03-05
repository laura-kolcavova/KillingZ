using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using KillingZ.LEntity;
using KillingZ.LWorld;
using KillingZ.GFX;
using KillingZ.LState;

namespace KillingZ
{
    class Handler
    {
        private Game1 game;
        private World world;
        private EntityManager entityManager;
        private AnimationManager animManager;
        private ZombieWaveManager zombieWaveManager;
        private GameCamera gameCamera;
        private Random r;

        private string worldName;
        private Game1.GameType gameType;
        private int killedZombies;
        private Player.PlayerType playerType;
        //private bool tokenOn;

        public Handler(Game1 game)
        {
            this.game = game;
            r = new Random();

            //tokenOn = false;
        }


        //Get
        //////////////////////////////
        public int GetWidth() => game.GetWidth();

        public int GetHeight() => game.GetHeight();

        public GraphicsDevice GetGraphicsDevice() => game.GetGraphicdDevice();

        public ContentManager GetContent() => game.Content;

        public State GetState() => game.GetState();


        public int GetRandom(int min, int max) => r.Next(min, max + 1);


        public World GetWorld() => world;

        public ZombieWaveManager GetZombieWaveManager() => zombieWaveManager;

        public EntityManager GetEntityManager() => entityManager;

        public AnimationManager GetAnimationManager() => animManager;

        public GameCamera GetCamera() => gameCamera;

        public string GetWorldName() => worldName;

        public Game1.GameType GetGameType() => gameType;

        public int GetKilledZombies() => killedZombies;

        public Player.PlayerType GetPlayerType() => playerType;

        //public bool GetTokenOn() { return tokenOn; }


        //Set
        /// //////////////////
        public void SetState(int state)
        {
            game.SetState(state);
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void SetZombieWaveManager(ZombieWaveManager zwm)
        {
            zombieWaveManager = zwm;
        }

        public void SetEntityManager(EntityManager em)
        {
            entityManager = em;
        }

        public void SetAnimationManager(AnimationManager am)
        {
            animManager = am;
        }

        public void SetGameCamera(GameCamera camera)
        {
            gameCamera = camera;
        }

        public void SetWorldName(string worldName)
        {
            this.worldName = worldName;
        }

        public void SetGameType(Game1.GameType gameType)
        {
            this.gameType = gameType;
        }

        public void SetKilledZombies(int count)
        {
            killedZombies = count;
        }

        public void SetPlayerType(Player.PlayerType type)
        {
            playerType = type;
        }
       /* public void SetTokenOn(bool state)
        {
            tokenOn = state;
        }*/
    }
}
