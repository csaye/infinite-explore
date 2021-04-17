using Microsoft.Xna.Framework;

namespace InfiniteExplore
{
    public class Camera
    {
        public Matrix Transform { get; private set; } // Camera transform matrix
        public Vector2 MinPosition { get; private set; } // Minimum position camera can view

        private const int MidWidth = Drawing.ScreenWidth / 2; // Screen width midpoint
        private const int MidHeight = Drawing.ScreenHeight / 2; // Screen height midpoint

        private readonly Matrix Offset = Matrix.CreateTranslation(MidWidth, MidHeight, 0);

        public Camera() { }

        public void Update(Game1 game)
        {
            // Get player position and size
            Vector2 playerPosition = game.Player.Position;
            Vector2 playerSize = game.Player.Size;

            // Get camera position
            float cameraX = playerPosition.X + playerSize.X / 2;
            float cameraY = playerPosition.Y + playerSize.Y / 2;
            MinPosition = new Vector2(cameraX - MidWidth, cameraY - MidWidth);

            // Invert camera view
            cameraX *= -1;
            cameraY *= -1;

            // Get transform
            Matrix position = Matrix.CreateTranslation(cameraX, cameraY, 0);
            Transform = position * Offset;
        }
    }
}
