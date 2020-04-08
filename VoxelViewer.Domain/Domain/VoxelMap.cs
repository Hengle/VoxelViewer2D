namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class VoxelMap {

        public int Width { get; }
        public int Height { get; }
        public int Area { get; }
        private VoxelCell[,] Cells { get; }


        public VoxelMap(int width, int height) {
            (Width, Height, Area) = (width, height, width * height);
            Cells = new VoxelCell[ Width, Height ];
        }
        public VoxelMap(int width, int height, VoxelCell[,] cells) {
            (Width, Height, Area) = (width, height, width * height);
            Cells = cells;
            if (Cells.GetLength( 0 ) != Width || Cells.GetLength( 1 ) != Height) throw new Exception( $"Cells array is invalid: Actual={Cells.GetLength( 0 )}/{Cells.GetLength( 1 )}, Expected={Width}/{Height}" );
        }


        // Get/Cell
        public VoxelCell GetCell(Point2Int pos) {
            return Cells[ pos.X, pos.Y ];
        }
        // Get/Cells
        public IEnumerable<(VoxelCell Value, Point2Int Pos)> GetCells() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var cell = Cells[ x, y ];
                    var pos = new Point2Int( x, y );
                    yield return (cell, pos);
                }
            }
        }

        // Set/Cell
        public void SetCell(Point2Int pos, VoxelCell cell) {
            Cells[ pos.X, pos.Y ] = cell;
        }
        // Set/Cell/IsChanged
        public bool SetCellAndGetIsChanged(Point2Int pos, VoxelCell cell) {
            var oldCell = Cells[ pos.X, pos.Y ];
            Cells[ pos.X, pos.Y ] = cell;
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


    }
}
