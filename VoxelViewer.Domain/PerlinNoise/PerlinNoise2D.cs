namespace PerlinNoise {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class PerlinNoise2D {

        private const float Persistence = 0.5f;


        // Noise
        public static float GetNoise(float x, float y) {
            return GetSmoothNoise( x, y );
        }
        public static float GetNoise(float x, float y, int count) {
            var frequency = 1f; // 1, 2,   4,    8
            var amplitude = 1f; // 1, 0.5, 0.25, 0.125
            var total = 0f;
            for (var i = 0; i < count; i++) {
                var v = GetSmoothNoise( x * frequency, y * frequency );
                frequency *= 2;
                amplitude *= Persistence;
                total += v * amplitude;
            }
            return total;
        }

        // Noise/01
        public static float GetNoise01(float x, float y) {
            return GetSmoothNoise( x, y ).To01();
        }
        public static float GetNoise01(float x, float y, int count) {
            var frequency = 1f; // 1, 2,   4,    8
            var amplitude = 1f; // 1, 0.5, 0.25, 0.125
            var total = 0f;
            for (var i = 0; i < count; i++) {
                var v = GetSmoothNoise( x * frequency, y * frequency ).To01();
                frequency *= 2;
                amplitude *= Persistence;
                total += v * amplitude;
            }
            return total;
        }


        // Helpers/Noise/Smooth
        private static float GetSmoothNoise(float x, float y) {
            x.Floor( out var ix, out var tx );
            y.Floor( out var iy, out var ty );
            var x1 = GetPlanNoise( ix + 0, iy + 0 );
            var x2 = GetPlanNoise( ix + 1, iy + 0 );
            var y1 = GetPlanNoise( ix + 0, iy + 1 );
            var y2 = GetPlanNoise( ix + 1, iy + 1 );
            var x3 = Interpolate( x1, x2, tx );
            var y3 = Interpolate( y1, y2, tx );
            return Interpolate( x3, y3, ty );
        }
        // Helpers/Noise/Plan
        private static float GetPlanNoise(int x, int y) { // [-1, 1]
            // 1/4 + 4/8 + 4/16 = 1
            var center = GetPlanNoise_( x, y );
            var sides = GetPlanNoise_( x - 1, y ) + GetPlanNoise_( x + 1, y ) + GetPlanNoise_( x, y - 1 ) + GetPlanNoise_( x, y + 1 );
            var corners = GetPlanNoise_( x - 1, y - 1 ) + GetPlanNoise_( x + 1, y - 1 ) + GetPlanNoise_( x - 1, y + 1 ) + GetPlanNoise_( x + 1, y + 1 );
            return (center / 4f) + (sides / 8f) + (corners / 16f);
        }
        private static float GetPlanNoise_(int x, int y) { // [-1, 1]
            var n = x + (y * 57);
            n = (n << 13) ^ n;
            return 1f - ((((n * ((n * n * 15731) + 789221)) + 1376312589) & 0x7fffffff) / 1073741824f);
        }


        // Helpers/Math
        private static float Interpolate(float v1, float v2, float t) {
            //t = Interpolations.SmoothStepInterpolation( t );
            //t = Interpolations.SmoothStepPerlinInterpolation( t );
            return Interpolations.Lerp( v1, v2, t );
        }
        private static void Floor(this float value, out int i, out float t) {
            i = value.Floor();
            t = value - i;
        }
        private static int Floor(this float value) {
            var result = (int) value;
            if (result <= value) return result;
            return result - 1;
        }
        private static float To01(this float value) {
            return (value + 1) / 2;
        }


    }
}