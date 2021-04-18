using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace InfiniteExplore
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        public Player Player { get; private set; }
        public Camera Camera { get; private set; }
        public Map Map { get; private set; }

        private int ufps;
        private int dfps;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Map = new Map();
            Player = new Player(0, 0, Drawing.Grid, Drawing.Grid * 2);
            Camera = new Camera();
        }

        protected override void Initialize()
        {
            // Initialize graphics
            Drawing.Initialize(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
            Drawing.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            // Get time delta and update FPS
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ufps = (int)Math.Round(1 / delta);

            // Get and process keyboard state
            KeyboardState = Keyboard.GetState();
            if (KeyboardState.IsKeyDown(Keys.Escape)) Exit();

            Player.Update(delta, this); // Update player
            Camera.Update(this); // Update camera

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Get time delta and draw FPS
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            dfps = (int)Math.Round(1 / delta);

            GraphicsDevice.Clear(Color.Black);

            // World sprite batch
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);
            Map.Draw(this); // Draw map
            Player.Draw(this); // Draw player
            SpriteBatch.End();

            // UI sprite batch
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Drawing.DrawText($"ufps: {ufps}", new Vector2(8, 8), Color.White, this);
            Drawing.DrawText($"dfps: {dfps}", new Vector2(8, 24), Color.White, this);
            Player.DrawUI(this);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
