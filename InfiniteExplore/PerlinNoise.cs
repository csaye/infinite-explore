using Microsoft.Xna.Framework;
using System;

namespace InfiniteExplore
{
    public static class PerlinNoise
    {
        private static readonly byte[] Permutation = new byte[]
        {
            151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36,
            103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0,
            26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56,
            87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
            77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55,
            46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132,
            187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109,
            198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
            255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183,
            170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
            172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112,
            104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162,
            241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106,
            157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205,
            93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
        };

        private static byte P(int n) => Permutation[n % 256]; // Safely returns nth permutation
        private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10); // Perlin fade
        private static float Lerp(float a, float b, float t) => (1 - t) * a + t * b; // Linear interpolation
        private static int FastFloor(float n) => n < 0 ? (int)n - 1 : (int)n; // Fast floor through int casting

        // Returns hash gradient
        private static float Grad(int hash, float x) => Grad(hash, x, 0, 0);
        private static float Grad(int hash, float x, float y) => Grad(hash, x, y, 0);
        private static float Grad(int hash, float x, float y, float z)
        {
            switch (hash % 16)
            {
                case 0: return x + y;
                case 1: return -x + y;
                case 2: return x - y;
                case 3: return -x - y;
                case 4: return x + z;
                case 5: return -x + z;
                case 6: return x - z;
                case 7: return -x - z;
                case 8: return y + z;
                case 9: return -y + z;
                case 10: return y - z;
                case 11: return -y - z;
                case 12: return y + x;
                case 13: return -y + z;
                case 14: return y - x;
                case 15: return -y - z;

                default: return 0; // Never happens
            }
        }

        // Returns 3D perlin noise for given point
        public static float Get(Vector3 v3) => Get(v3.X, v3.Y, v3.Z);
        public static float Get(float x, float y, float z)
        {
            float floorX = FastFloor(x);
            float floorY = FastFloor(y);
            float floorZ = FastFloor(z);

            //// Find byte cube that contains point
            byte xi = (byte)(floorX % 256);
            byte yi = (byte)(floorY % 256);
            byte zi = (byte)(floorZ % 256);

            // Find relative position of point in cube
            float xf = x - floorX;
            float yf = y - floorY;
            float zf = z - floorZ;

            // Compute fade curves
            float u = Fade(xf);
            float v = Fade(yf);
            float w = Fade(zf);

            // Hash corner coordinates
            int a = P(xi) + yi;
            int aa = P(a) + zi;
            int ab = P(a + 1) + zi;
            int b = P(xi + 1) + yi;
            int ba = P(b) + zi;
            int bb = P(b + 1) + zi;

            //Add blended corner results
            float x1 = Lerp(Grad(P(aa), xf, yf, zf), Grad(P(ba), xf - 1, yf, zf), u);
            float x2 = Lerp(Grad(P(ab), xf, yf - 1, zf), Grad(P(bb), xf - 1, yf - 1, zf), u);
            float y1 = Lerp(x1, x2, v);
            float x3 = Lerp(Grad(P(aa + 1), xf, yf, zf - 1), Grad(P(ba + 1), xf - 1, yf, zf - 1), u);
            float x4 = Lerp(Grad(P(ab + 1), xf, yf - 1, zf - 1), Grad(P(bb + 1), xf - 1, yf - 1, zf - 1), u);
            float y2 = Lerp(x3, x4, v);
            float z1 = Lerp(y1, y2, w);
            return (z1 + 1) / 2;
        }

        // Returns 2D perlin noise for given point
        public static float Get(Vector2 v2) => Get(v2.X, v2.Y);
        public static float Get(float x, float y)
        {
            int floorX = FastFloor(x);
            int floorY = FastFloor(y);

            // Find byte square that contains point
            byte xi = (byte)(floorX % 256);
            byte yi = (byte)(floorY % 256);

            // Find relative position of point in square
            float xf = x - floorX;
            float yf = y - floorY;

            // Compute fade curves
            float u = Fade(xf);
            float v = Fade(yf);

            // Hash corner coordinates
            int a = P(xi) + yi;
            int aa = P(a);
            int ab = P(a + 1);
            int b = P(xi + 1) + yi;
            int ba = P(b);
            int bb = P(b + 1);

            // Add blended corner results
            float x1 = Lerp(Grad(P(aa), xf, yf), Grad(P(ba), xf - 1, yf), u);
            float x2 = Lerp(Grad(P(ab), xf, yf - 1), Grad(P(bb), xf - 1, yf - 1), u);
            float y1 = Lerp(x1, x2, v);
            return (y1 + 1) / 2;
        }

        // Returns 1D perlin noise for given point
        public static float Get(float x)
        {
            int floorX = FastFloor(x);

            // Find byte square that contains point
            byte xi = (byte)(floorX % 256);

            // Find relative position of point in square
            float xf = x - floorX;

            // Compute fade curves
            float u = Fade(xf);

            // Hash corner coordinates
            int a = P(xi);
            int aa = P(a);
            int b = P(xi + 1);
            int ba = P(b);

            // Add blended corner results
            float x1 = Lerp(Grad(P(aa), xf), Grad(P(ba), xf - 1, 0), u);
            return (x1 + 1) / 2;
        }
    }
}
