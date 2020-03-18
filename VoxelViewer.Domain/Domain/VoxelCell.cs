namespace VoxelViewer2D.Domain {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public struct VoxelCell {

        public const int MaxValue = 255;

        public int Value { get; private set; }
        public float Value01 => (float) Value / MaxValue;


        public VoxelCell(int value) {
            Value = value;
        }


        // Set
        public void SetValue(int value) {
            Value = value;
        }


        // Utils
        public override string ToString() {
            return $"VoxelCell: {Value}";
        }
        public string ToString(int x, int y) {
            return $"VoxelCell: {Value}, ({x}, {y})";
        }
        public string ToString((int X, int Y) pos) {
            return $"VoxelCell: {Value}, {pos}";
        }


    }
}
