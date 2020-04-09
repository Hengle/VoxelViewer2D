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
            Check( cells, width, height );
            Cells = cells;
            (Width, Height, Area) = (width, height, width * height);
        }


        // Get/Cell
        public VoxelCell GetCell(Pointer pnt) {
            return Cells[ pnt.X, pnt.Y ];
        }
        // Get/Cells
        public IEnumerable<(VoxelCell Value, Pointer Pnt)> GetCells() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var cell = Cells[ x, y ];
                    var pnt = new Pointer( x, y );
                    yield return (cell, pnt);
                }
            }
        }

        // Set/Cell
        public void SetCell(Pointer pnt, VoxelCell cell) {
            Cells[ pnt.X, pnt.Y ] = cell;
        }
        // Set/Cell/IsChanged
        public bool SetCellAndGetIsChanged(Pointer pnt, VoxelCell cell) {
            var oldCell = Cells[ pnt.X, pnt.Y ];
            Cells[ pnt.X, pnt.Y ] = cell;
            return oldCell != cell;
        }

        // Clear
        public void Clear() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    Cells[ x, y ] = default;
                }
            }
        }


        // Utils
        public override string ToString() {
            return $"VoxelMap: {Width}, {Height}";
        }


        // Helpers
        private static void Check<T>(T[,] array, int length1, int length2) {
            var actual = (array.GetLength( 0 ), array.GetLength( 1 ));
            var expected = (length1, length2);
            if (actual != expected) throw new Exception( $"Array is invalid: Actual={actual}, Expected={expected}" );
        }


    }
}
