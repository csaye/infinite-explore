using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfiniteExplore
{
    public static class Drawing
    {
        public const int Grid = 32;

        public const int ScreenWidth = 512;
        public const int ScreenHeight = 512;

        public static Texture2D PlayerTexture { get; private set; }
        public static Texture2D TilesetTexture { get; private set; }
        public static Texture2D GrassTexture { get; private set; }
        public static Texture2D TreeTexture { get; private set; }

        private static SpriteFont arialFont;

        public static void Initialize(Game1 game)
        {
            // Initialize graphics
            game.Graphics.PreferredBackBufferWidth = ScreenWidth;
            game.Graphics.PreferredBackBufferHeight = ScreenHeight;
            game.Graphics.ApplyChanges();
        }

        public static void LoadContent(Game1 game)
        {
            PlayerTexture = game.Content.Load<Texture2D>("Player");
            TilesetTexture = game.Content.Load<Texture2D>("Tileset");
            GrassTexture = game.Content.Load<Texture2D>("Grass");
            TreeTexture = game.Content.Load<Texture2D>("Tree");

            arialFont = game.Content.Load<SpriteFont>("Arial");
        }

        // Draws given sprite from given tileset position to given destination
        public static void DrawSprite(Texture2D texture, Rectangle destRect, Vector2 spriteSize, int tilesetIndex, Game1 game)
        {
            // Calculate source rect from source size and tileset index
            int spriteWidth = (int)spriteSize.X;
            int spriteHeight = (int)spriteSize.Y;
            int spritesPerRow = texture.Width / spriteWidth;
            int tilesetX = tilesetIndex % spritesPerRow * spriteWidth;
            int tilesetY = tilesetIndex / spritesPerRow * spriteHeight;
            Rectangle sourceRect = new Rectangle(tilesetX, tilesetY, spriteWidth, spriteHeight);

            // Draw texture to sprite batch
            game.SpriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }

        // Draws given text at given position with given color
        public static void DrawText(string text, Vector2 position, Color color, Game1 game)
        {
            // Draw string to sprite batch
            game.SpriteBatch.DrawString(arialFont, text, position, color);
        }
    }
}
