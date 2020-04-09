namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class VoxelMapCircleFactory {

        private static readonly Random Random = new Random();


        public static VoxelMap Create(int width, int height) {
            return Create( width, height, width / 2, height / 2, height / 2 );
        }
        public static VoxelMap Create(int width, int height, int cx, int cy, int radius) {
            var cells = EnumerableEx.GetIterator2D( width, height )
                .Select( i => GetValue( i.X, i.Y, cx, cy, radius ) )
                .Select( i => i.Pow( 1.5f ) )
                .Select( i => i.WithRandom( 0.3f ) )
                .Select( i => i.ToInt() )
                .Select( i => (VoxelCell) i )
                .ToArray2D( width, height );
            return new VoxelMap( width, height, cells );
        }


        // Helpers/Value
        private static float GetValue(int x, int y, int cx, int cy, int radius) {
            return GetDistance( x, y, cx, cy ).Normalize( 0, radius ).Clamp().Invert();
        }

        // Helpers/Math
        private static float GetDistance(int x, int y, int x2, int y2) {
            return (float) Math.Sqrt( GetSqrDistance( x, y, x2, y2 ) );
        }
        //private static int GetDistance(int x, int y, int x2, int y2) {
        //    var dx = Math.Abs( x - x2 );
        //    var dy = Math.Abs( y - y2 );
        //    return dx + dy;
        //}
        private static int GetSqrDistance(int x, int y, int x2, int y2) {
            var dx = Math.Abs( x - x2 );
            var dy = Math.Abs( y - y2 );
            return (dx * dx) + (dy * dy);
        }
        private static float Normalize(this float value, float min, float max) {
            return (value - min) / (max - min);
        }
        private static float Clamp(this float value) {
            value = Math.Min( value, 1 );
            value = Math.Max( value, 0 );
            return value;
        }
        private static float Invert(this float value) {
            return 1 - value;
        }
        private static float Pow(this float value, float power) {
            return (float) Math.Pow( value, power );
        }
        private static int ToInt(this float value) {
            return (int) (value * VoxelCell.MaxValue);
        }
        // Helpers/Random
        private static float WithRandom(this float value, float max) {
            var invValue = Math.Max( max - value, 0 );
            value -= (float) Random.NextDouble() * invValue;
            return Math.Max( value, 0 );
        }


    }
}
