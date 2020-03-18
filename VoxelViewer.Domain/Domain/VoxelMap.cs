namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class VoxelMap {

        public int Width { get; }
        public int Height { get; }
        private VoxelCell[,] Cells { get; }


        public VoxelMap(int width, int height) {
            (Width, Height) = (width, height);
            Cells = new VoxelCell[ Width, Height ];
        }


        // Get/Cell
        public VoxelCell GetCell(int x, int y) {
            return Cells[ x, y ];
        }
        public ref VoxelCell GetCellRef(int x, int y) {
            return ref Cells[ x, y ];
        }
        public VoxelCell GetCell((int X, int Y) pos) {
            return Cells[ pos.X, pos.Y ];
        }
        public ref VoxelCell GetCellRef((int X, int Y) pos) {
            return ref Cells[ pos.X, pos.Y ];
        }
        // Get/Cells
        public IEnumerable<(int X, int Y)> GetCells() {
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    yield return (x, y);
                }
            }
        }


        // Utils
        public override string ToString() {
            return $"VoxelMap: {Width}, {Height}";
        }


    }
}
