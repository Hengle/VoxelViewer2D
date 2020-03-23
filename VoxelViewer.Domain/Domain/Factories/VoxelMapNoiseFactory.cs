namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PerlinNoise;

    public static class VoxelMapNoiseFactory {


        public static VoxelMap Create(int width, int height) {
            var cells = EnumerableEx.GetIterator2D( width, height )
                .Select( i => GetValue( i.X, i.Y ) )
                .Select( i => (VoxelCell) i )
                .ToArray2D( width, height );
            return new VoxelMap( width, height, cells );
        }


        // Helpers/Value
        private static int GetValue(float x, float y) {
            return PerlinNoise2D.GetNoise01( x / 6f, y / 6f, 1 ).ToInt( 255 ).Compress( 6 );
        }
        // Helpers/Math
        private static int ToInt(this float value, int factor) {
            return (int) (value * factor);
        }
        private static int Compress(this int value, int factor) {
            value /= factor;
            value *= factor;
            return value;
        }


    }
}
