namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class VoxelMapFiller {

        private static readonly Random Random = new Random();


        public static VoxelMap Fill(this VoxelMap map) {
            Fill( map, map.Width / 2, map.Height / 2, 32 );
            return map;
        }

        private static void Fill(VoxelMap map, int cx, int cy, int radius) {
            foreach (var (x, y) in map.GetCells()) {
                var value = GetDistance( x, y, cx, cy )
                    .ChangeRange( radius, VoxelCell.MaxValue )
                    .Invert( VoxelCell.MaxValue )
                    .WithRandom( 150 );
                map.GetCellRef( x, y ).SetValue( value );
            }
        }


        // Helpers/Distance
        private static int GetDistance(int x, int y, int x2, int y2) {
            return (int) Math.Sqrt( GetSqrDistance( x, y, x2, y2 ) );
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
        // Helpers/Range
        private static int ChangeRange(this int value, int from, int to) {
            return value * to / from; // from 0..from to 0..to (value / from * to)
        }
        // Helpers/Invert
        private static int Invert(this int value, int max) {
            return Math.Max( max - value, 0 );
        }
        // Helpers/Random
        private static int WithRandom(this int value, int max) {
            //var value01 = (double) value / VoxelCell.MaxValue;
            //value01 = Math.Pow( value01, 1.0 / 1.5 );
            //if (value01 < Random.NextDouble() * 0.6) {
            //    return 0;
            //}
            //return value;

            var invValue = Math.Max( max - value, 0 );
            value -= Random.Next( invValue );
            return Math.Max( value, 0 );
        }


    }
}
