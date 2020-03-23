namespace PerlinNoise {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Interpolations {


        public static float CubicInterpolation(float v0, float v1, float v2, float v3, float t) {
            //var v01 = Lerp( v0, v1, t );
            //var v12 = Lerp( v1, v2, t );
            //var v23 = Lerp( v2, v3, t );
            //var v012 = Lerp( v01, v12, t );
            //var v123 = Lerp( v12, v23, t );
            //return Lerp( v012, v123, t );
            var p = (v3 - v2) - (v0 - v1);
            var q = (v0 - v1) - p;
            var r = v2 - v0;
            var s = v1;
            return (p * t * 3) + (q * t * 2) + (r * t) + s;
            //var r = 1f - t;
            //var f0 = r * r * r;
            //var f1 = r * r * t * 3;
            //var f2 = r * t * t * 3;
            //var f3 = t * t * t;
            //return (v0 * f0) + (v1 * f1) + (v2 * f2) + (v3 * f3);
        }
        public static float QuadraticInterpolation(float v0, float v1, float v2, float t) {
            var v01 = Lerp( v0, v1, t );
            var v12 = Lerp( v1, v2, t );
            return Lerp( v01, v12, t );
        }
        public static float Lerp(float v1, float v2, float t) {
            return v1 + ((v2 - v1) * t);
        }


        public static float CosInterpolation(float t) {
            t = (float) -Math.Cos( t * Math.PI ); // [-1, 1]
            return (t + 1) / 2; // [0, 1]
        }
        public static float SmoothStepPerlinInterpolation(float t) {
            // Ken Perlin's version
            return t * t * t * ((t * ((6 * t) - 15)) + 10);
        }
        public static float SmoothStepInterpolation(float t) {
            return t * t * (3 - (2 * t));
        }


    }
}
