using Microsoft.Xna.Framework;
using System;

namespace InfiniteExplore
{
    public class Map
    {
        private const float Pi = (float)Math.PI;
        private const int Grid = Drawing.Grid;

        private readonly Vector2 TileSpriteSize = new Vector2(16, 16);

        // Noise variables
        private const float Frequency = Pi / 32;
        private const float GrassFrequency = Pi / 16;
        private const float BiomeFrequency = Pi / 128;

        private const float Offset = 0;

        private const float GrassThreshold = 0.5f;

        private const float OceanThreshold = 0.35f; // 0.35
        private const float DesertThreshold = 0.45f; // 0.1
        private const float PlainsThreshold = 0.55f; // 0.1
        private const float RocksThreshold = 0.65f; // 0.1
        //private const float TundraThreshold = 1; // 0.35

        private enum Biome
        {
            Ocean,
            Desert,
            Plains,
            Rocks,
            Tundra
        }

        public Map() { }

        public void Draw(Game1 game)
        {
            // Get camera position
            Vector2 cameraPosition = game.Camera.MinPosition / Grid;

            // Get range of positions visible from camera
            int minX = (int)Math.Floor(cameraPosition.X);
            int minY = (int)Math.Floor(cameraPosition.Y);
            int maxX = minX + Drawing.ScreenWidth / Grid;
            int maxY = minY + Drawing.ScreenHeight / Grid;

            // Draw all tiles visible from camera
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    // Get base noise for point
                    float baseX = x * Frequency + Offset;
                    float baseY = y * Frequency + Offset;
                    float noise = PerlinNoise.Get(baseX, baseY);

                    // Calculate biome noise for point
                    float biomeX = x * BiomeFrequency + Offset;
                    float biomeY = y * BiomeFrequency + Offset;
                    float biomeNoise = PerlinNoise.Get(biomeX, biomeY);

                    // Calculate biome
                    Biome biome;
                    if (biomeNoise < OceanThreshold) biome = Biome.Ocean; // 0 - 0.3 (0.3)
                    else if (biomeNoise < DesertThreshold) biome = Biome.Desert; // 0.3 - 0.45 (0.15)
                    else if (biomeNoise < PlainsThreshold) biome = Biome.Plains; // 0.45 - 0.55 (0.1)
                    else if (biomeNoise < RocksThreshold) biome = Biome.Rocks; // 0.55 - 0.7 (0.15)
                    else biome = Biome.Tundra; // 0.7 - 1.0 (0.3)

                    // Get tileset index for position
                    int tilesetIndex = GetTilesetIndex(biome, noise, biomeNoise);

                    // Draw tile
                    Rectangle destRect = new Rectangle(x * Grid, y * Grid, Grid, Grid);
                    Drawing.DrawSprite(Drawing.TilesetTexture, destRect, TileSpriteSize, tilesetIndex, game);

                    // Get grass noise for point
                    float grassX = x * GrassFrequency + Offset;
                    float grassY = y * GrassFrequency + Offset;
                    float grassNoise = PerlinNoise.Get(grassX, grassY);

                    // If plains and grass, draw grass sprite
                    if (biome == Biome.Plains && grassNoise > GrassThreshold)
                    {
                        Rectangle grassRect = new Rectangle(
                            x * Grid - Grid / 8, y * Grid - (int)(Grid * 0.375f), (int)(Grid * 1.25f), (int)(Grid * 1.375f)
                        );
                        Drawing.DrawSprite(Drawing.GrassTexture, grassRect, new Vector2(20, 22), 0, game);
                    }
                }
            }
        }

        // Returns tileset index for given biome with given noise
        private int GetTilesetIndex(Biome biome, float noise, float biomeNoise)
        {
            // Return tile based on biome
            switch (biome)
            {
                case Biome.Ocean: return biomeNoise < 0.2f ? 4 : 1;
                case Biome.Desert: return 2;
                case Biome.Plains: return noise < 0.3f ? 3 : noise > 0.7f ? 5 : 0;
                case Biome.Rocks: return 8;
                case Biome.Tundra: return biomeNoise > 0.8f ? 7 : 6;
            }

            return -1;
        }
    }
}
