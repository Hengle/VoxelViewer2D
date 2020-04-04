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
            if (Cells.GetLength( 0 ) != Width || Cells.GetLength( 1 ) != Height) throw new Exception( $"Cells array is invalid: {Cells.GetLength( 0 )}/{Cells.GetLength( 1 )}, {Width}/{Height}" );
        }


        // Get/Cell
        public VoxelCell GetCell(int x, int y) {
            return Cells[ x, y ];
        }
        public VoxelCell GetCell((int X, int Y) pos) {
            return Cells[ pos.X, pos.Y ];
        }
        // Get/Cells
        public IEnumerable<(VoxelCell Value, int X, int Y)> GetCells() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    yield return (Cells[ x, y ], x, y);
                }
            }
        }

        // Set/Cell
        public void SetCell(int x, int y, VoxelCell cell) {
            Cells[ x, y ] = cell;
        }
        public void SetCell((int X, int Y) pos, VoxelCell cell) {
            Cells[ pos.X, pos.Y ] = cell;
        }
        // Set/Cell/IsChanged
        public bool SetCellAndGetIsChanged(int x, int y, VoxelCell cell) {
            var oldCell = Cells[ x, y ];
            Cells[ x, y ] = cell;
            return oldCell != cell;
        }
        public bool SetCellAndGetIsChanged((int X, int Y) pos, VoxelCell cell) {
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
