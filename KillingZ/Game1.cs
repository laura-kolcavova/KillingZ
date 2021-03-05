using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using KillingZ.GFX;
using KillingZ.LState;

namespace KillingZ
{
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int WIDTH = 800;
        private const int HEIGHT = 500;

        public static int STATE_COUNT = 3;

        public static int STATE_MENU = 0;
        public static int STATE_GAME = 1;
        public static int STATE_SELECT_WORLD = 2;

        public enum GameType { Campaign, Classic, Survival };

        private Handler handler;

        private State[] states;
        private int currentState;

        private Color mainColor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            /*graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;*/
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            handler = new Handler(this);

            Assets.SetContentManager(Content);
            Assets.SetHandler(handler);
            Assets.LoadContent();

            states = new State[STATE_COUNT];
            currentState = STATE_MENU;
            SetState(currentState);

            mainColor = new Color(220, 224, 224);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SetState(int state)
        {
            UnloadState(currentState);
            currentState = state;
            LoadState(state);
        }

        private void UnloadState(int state)
        {
            states[state] = null;
        }

        private void LoadState(int state)
        {
            if (state == STATE_MENU) states[state] = new MenuState(handler);
            else if (state == STATE_GAME) states[state] = new GameState(handler);
            else if (state == STATE_SELECT_WORLD) states[state] = new SelectWorldState(handler);
        }

        protected override void Update(GameTime gameTime)
        {

            if (states[currentState] != null)
            {
                states[currentState].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(mainColor);

            spriteBatch.Begin();
            ///////////////////////////////////////

            if(states[currentState] != null)
            {
                states[currentState].Draw(spriteBatch);
            }

            ///////////////////////////////////////
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public int GetWidth()
        {
            return graphics.GraphicsDevice.Viewport.Width;
        }

        public int GetHeight()
        {
            return graphics.GraphicsDevice.Viewport.Height;
        }

        public GraphicsDevice GetGraphicdDevice()
        {
            return graphics.GraphicsDevice;
        }

        public State GetState()
        {
            return states[currentState];
        }
    }
}