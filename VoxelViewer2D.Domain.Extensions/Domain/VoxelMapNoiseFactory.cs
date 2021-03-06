﻿namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.PerlinNoise;
    using System.Text;

    public static class VoxelMapNoiseFactory {


        public static VoxelMap Create(int width, int height) {
            var cells = EnumerableEx.GetIterator2D( width, height )
                .Select( i => GetValue( i.X, i.Y ) )
                .Normalize()
                .Select( i => i.Staircase( 1f / 15 ) )
                .Select( i => i.Pow( 1.5f ) )
                .Select( i => i.ToInt() )
                //.Mix()
                .Select( i => (VoxelCell) i )
                .ToArray2D( width, height );
            return new VoxelMap( cells, width, height );
        }


        // Helpers/Value
        private static float GetValue(float x, float y) {
            const float scale = 1f / 6;
            return PerlinNoise2D.GetNoise01( x * scale, y * scale, 1 );
        }
        // Helpers/Values
        private static IEnumerable<float> Normalize(this IEnumerable<float> values) {
            var buffer = values.ToList();
            var min = buffer.Min();
            var max = buffer.Max();
            return buffer.Select( i => i.Normalize( min, max ) );
        }
        // Helpers/Math
        private static float Normalize(this float value, float min, float max) {
            return (value - min) / (max - min);
        }
        private static float Staircase(this float value, float step) {
            value /= step;
            value = (int) value;
            value *= step;
            return value;
        }
        private static float Pow(this float value, float power) {
            return (float) Math.Pow( value, power );
        }
        private static int ToInt(this float value) {
            return (int) (value * VoxelCell.MaxValue);
        }
        // Helpers/Mix
        private static IEnumerable<int> Mix(this IEnumerable<int> values) {
            var table = CreateRandomTable();
            foreach (var item in values) {
                yield return table[ item ];
            }
        }
        private static int[] CreateRandomTable() {
            var rnd = new Random();
            var table = Enumerable.Range( 0, 256 ).ToArray();
            for (var i = 0; i < table.Length; i++) {
                var i2 = rnd.Next( table.Length );
                Swap( ref table[ i ], ref table[ i2 ] );
            }
            return table;
        }
        private static void Swap<T>(ref T a, ref T b) {
            var tmp = a;
            a = b;
            b = tmp;
        }


    }
}
