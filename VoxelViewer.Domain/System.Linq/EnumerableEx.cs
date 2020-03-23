namespace System.Linq {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class EnumerableEx {


        public static IEnumerable<(int X, int Y)> GetIterator2D(int width, int height) {
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    yield return (x, y);
                }
            }
        }

        public static T[,] ToArray2D<T>(this IEnumerable<T> source, int width, int height) {
            var array = new T[ width, height ];
            var zip = Enumerable.Zip( source, GetIterator2D( width, height ), (i, ind) => (i, ind.X, ind.Y) );
            foreach (var (v, x, y) in zip) {
                array[ x, y ] = v;
            }
            return array;
        }


    }
}
