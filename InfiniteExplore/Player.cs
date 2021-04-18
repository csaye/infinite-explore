using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InfiniteExplore
{
    public class Player
    {
        private Vector2 position;
        private Vector2 size;

        private Vector2 direction;

        private Rectangle Bounds
        {
            get { return new Rectangle(position.ToPoint(), size.ToPoint()); }
        }

        private const float Speed = 100;
        private readonly Vector2 SpriteSize = new Vector2(16, 32);

        public Vector2 Position => position;
        public Vector2 Size => size;

        public Player(int x, int y, int w, int h)
        {
            position = new Vector2(x, y);
            size = new Vector2(w, h);
        }

        public void Update(float delta, Game1 game)
        {
            // Get keyboard state
            KeyboardState state = game.KeyboardState;

            // Get movement direction
            direction.X = state.IsKeyDown(Keys.A) ? -1 : state.IsKeyDown(Keys.D) ? 1 : 0;
            direction.Y = state.IsKeyDown(Keys.W) ? -1 : state.IsKeyDown(Keys.S) ? 1 : 0;
            if (direction.Length() > 1) direction.Normalize();

            // Update position by movement direction
            position += direction * Speed * delta;
        }

        public void Draw(Game1 game)
        {
            Drawing.DrawSprite(Drawing.PlayerTexture, Bounds, SpriteSize, 0, game);
        }

        public void DrawUI(Game1 game)
        {
            Vector2 pos = position / Drawing.Grid;
            pos.Round();
            Drawing.DrawText($"pos: {pos}", new Vector2(8, 40), Color.White, game);
        }
    }
}
