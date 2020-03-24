﻿namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class VoxelMapCircleFactory {

        private static readonly Random Random = new Random();


        public static VoxelMap Create(int width, int height, int cx, int cy, int radius) {
            var cells = EnumerableEx.GetIterator2D( width, height )
                .Select( i => GetValue( i.X, i.Y, cx, cy, radius ) )
                .Select( i => (VoxelCell) i )
                .ToArray2D( width, height );
            return new VoxelMap( width, height, cells );
        }


        // Helpers/Value
        private static int GetValue(int x, int y, int cx, int cy, int radius) {
            return GetDistance( x, y, cx, cy ).ChangeRange( radius, VoxelCell.MaxValue ).Invert( VoxelCell.MaxValue ).WithRandom( 150 );
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
            var invValue = Math.Max( max - value, 0 );
            value -= Random.Next( invValue );
            return Math.Max( value, 0 );
        }


    }
}