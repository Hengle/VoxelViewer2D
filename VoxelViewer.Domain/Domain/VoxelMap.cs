namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class VoxelMap {

        private VoxelCell[,] Cells { get; }
        public int Width { get; }
        public int Height { get; }
        public int Area { get; }


        public VoxelMap(int width, int height) {
            Cells = new VoxelCell[ width, height ];
            (Width, Height, Area) = (width, height, width * height);
        }
        public VoxelMap(VoxelCell[,] cells, int width, int height) {
            EnsureValidArray( cells, width, height );
            Cells = cells;
            (Width, Height, Area) = (width, height, width * height);
        }


        // Get/Cell
        public VoxelCell GetCell(Pointer pnt) {
            return Cells[ pnt.X, pnt.Y ];
        }
        public VoxelCell? SafeGetCell(Pointer pnt) {
            if (!Check( pnt )) return null;
            return Cells[ pnt.X, pnt.Y ];
        }

        // Set/Cell
        public void SetCell(Pointer pnt, VoxelCell cell) {
            Cells[ pnt.X, pnt.Y ] = cell;
        }
        public void SafeSetCell(Pointer pnt, VoxelCell cell) {
            if (!Check( pnt )) return;
            Cells[ pnt.X, pnt.Y ] = cell;
        }

        // Change/Cell
        public bool ChangeCell(Pointer pnt, VoxelCell cell) {
            return ChangeValue( ref Cells[ pnt.X, pnt.Y ], cell );
        }
        public bool SafeChangeCell(Pointer pnt, VoxelCell cell) {
            if (!Check( pnt )) return false;
            return ChangeValue( ref Cells[ pnt.X, pnt.Y ], cell );
        }

        // Clear
        public void Clear() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    Cells[ x, y ] = default;
                }
            }
        }


        // Utils/Check
        public bool Check(Pointer pnt) {
            return (pnt.X >= 0 && pnt.X < Width) && (pnt.Y >= 0 && pnt.Y < Height);
        }
        // Utils/Iterator
        public IEnumerable<(VoxelCell Value, Pointer Pointer)> GetIterator() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var cell = Cells[ x, y ];
                    var pnt = new Pointer( x, y );
                    yield return (cell, pnt);
                }
            }
        }


        // Utils
        public override string ToString() {
            return $"VoxelMap: {Width}, {Height}";
        }


        // Helpers
        private static void EnsureValidArray<T>(T[,] array, int length1, int length2) {
            var actual = (array.GetLength( 0 ), array.GetLength( 1 ));
            var expected = (length1, length2);
            if (actual != expected) throw new Exception( $"Array is invalid: Actual={actual}, Expected={expected}" );
        }
        private static bool ChangeValue<T>(ref T value, T @new) {
            var old = value;
            value = @new;
            return !old.Equals( @new );
        }


    }
}
