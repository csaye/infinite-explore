using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            // Get time delta
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Get and process keyboard state
            KeyboardState = Keyboard.GetState();
            if (KeyboardState.IsKeyDown(Keys.Escape)) Exit();

            Player.Update(delta, this); // Update player
            Camera.Update(this); // Update camera

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // World sprite batch
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.Transform);
            Map.Draw(this); // Draw map
            Player.Draw(this); // Draw player
            SpriteBatch.End();

            // UI sprite batch
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
